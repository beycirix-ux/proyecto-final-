using VeterinariaApp.Datos;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Formularios;

public class FrmConsultas : Form
{
    private readonly ConsultaDAO dao = new ConsultaDAO();
    private readonly DateTimePicker dtpFecha;
    private readonly ComboBox cmbMascota, cmbVeterinario;
    private readonly TextBox txtMotivo, txtDiagnostico, txtTratamiento;
    private readonly DataGridView dgv;
    private int idSeleccionado = 0;

    public FrmConsultas()
    {
        Text = "Consultas";
        ClientSize = new Size(900, 650);
        StartPosition = FormStartPosition.CenterParent;

        UiHelper.Etiqueta(this, "Fecha:", 20, 20);
        dtpFecha = UiHelper.Fecha(this, 150, 20);
        UiHelper.Etiqueta(this, "Mascota:", 20, 55);
        cmbMascota = UiHelper.Combo(this, 150, 55);
        UiHelper.Etiqueta(this, "Veterinario:", 20, 90);
        cmbVeterinario = UiHelper.Combo(this, 150, 90);
        UiHelper.Etiqueta(this, "Motivo:", 20, 125);
        txtMotivo = UiHelper.Texto(this, 150, 125, 500);
        UiHelper.Etiqueta(this, "Diagnóstico:", 20, 160);
        txtDiagnostico = UiHelper.Texto(this, 150, 160, 500);
        UiHelper.Etiqueta(this, "Tratamiento:", 20, 195);
        txtTratamiento = UiHelper.Texto(this, 150, 195, 500);

        UiHelper.Boton(this, "Guardar", 20, 240, (s, e) => Guardar());
        UiHelper.Boton(this, "Actualizar", 130, 240, (s, e) => Actualizar());
        UiHelper.Boton(this, "Eliminar", 240, 240, (s, e) => Eliminar());
        UiHelper.Boton(this, "Limpiar", 350, 240, (s, e) => Limpiar());

        dgv = UiHelper.Grid(this, 295, 345);
        dgv.SelectionChanged += (s, e) => SeleccionarFila();

        CargarCombos();
        CargarGrid();
        Limpiar();
    }

    private void CargarCombos()
    {
        UiHelper.Ejecutar(() =>
        {
            cmbMascota.DataSource = new MascotaDAO().Listar();
            cmbMascota.DisplayMember = "Nombre";
            cmbMascota.ValueMember = "Id";

            cmbVeterinario.DataSource = new VeterinarioDAO().Listar();
            cmbVeterinario.DisplayMember = "Nombre";
            cmbVeterinario.ValueMember = "Id";
        });
    }

    private void CargarGrid()
    {
        UiHelper.Ejecutar(() =>
        {
            dgv.DataSource = dao.Listar();
            if (dgv.Columns["MascotaId"] != null) dgv.Columns["MascotaId"].Visible = false;
            if (dgv.Columns["VeterinarioId"] != null) dgv.Columns["VeterinarioId"].Visible = false;
            if (dgv.Columns["Fecha"] != null) dgv.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
        });
    }

    private void SeleccionarFila()
    {
        if (dgv.SelectedRows.Count == 0) return;
        if (dgv.SelectedRows[0].DataBoundItem is Consulta c)
        {
            idSeleccionado = c.Id;
            dtpFecha.Value = c.Fecha;
            cmbMascota.SelectedValue = c.MascotaId;
            cmbVeterinario.SelectedValue = c.VeterinarioId;
            txtMotivo.Text = c.Motivo;
            txtDiagnostico.Text = c.Diagnostico;
            txtTratamiento.Text = c.Tratamiento;
        }
    }

    private void Limpiar()
    {
        idSeleccionado = 0;
        dtpFecha.Value = DateTime.Today;
        if (cmbMascota.Items.Count > 0) cmbMascota.SelectedIndex = 0;
        if (cmbVeterinario.Items.Count > 0) cmbVeterinario.SelectedIndex = 0;
        txtMotivo.Clear();
        txtDiagnostico.Clear();
        txtTratamiento.Clear();
        dgv.ClearSelection();
    }

    private Consulta LeerFormulario()
    {
        if (cmbMascota.SelectedValue == null)
        {
            UiHelper.Aviso("Primero debe registrar una mascota.");
            return null;
        }
        if (cmbVeterinario.SelectedValue == null)
        {
            UiHelper.Aviso("Primero debe registrar un veterinario.");
            return null;
        }
        if (string.IsNullOrWhiteSpace(txtMotivo.Text))
        {
            UiHelper.Aviso("El motivo de la consulta es obligatorio.");
            return null;
        }
        return new Consulta
        {
            Id = idSeleccionado,
            Fecha = dtpFecha.Value.Date,
            MascotaId = (int)cmbMascota.SelectedValue,
            VeterinarioId = (int)cmbVeterinario.SelectedValue,
            Motivo = txtMotivo.Text.Trim(),
            Diagnostico = txtDiagnostico.Text.Trim(),
            Tratamiento = txtTratamiento.Text.Trim()
        };
    }

    private void Guardar()
    {
        var c = LeerFormulario();
        if (c == null) return;
        if (UiHelper.Ejecutar(() => dao.Insertar(c)))
        {
            CargarGrid();
            Limpiar();
        }
    }

    private void Actualizar()
    {
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione una consulta de la lista."); return; }
        var c = LeerFormulario();
        if (c == null) return;
        if (UiHelper.Ejecutar(() => dao.Actualizar(c)))
        {
            CargarGrid();
            Limpiar();
        }
    }

    private void Eliminar()
    {
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione una consulta de la lista."); return; }
        if (!UiHelper.Confirmar("¿Eliminar esta consulta?")) return;
        if (UiHelper.Ejecutar(() => dao.Eliminar(idSeleccionado)))
        {
            CargarGrid();
            Limpiar();
        }
    }
}
