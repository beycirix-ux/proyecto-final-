using VeterinariaApp.Datos;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Formularios;

public class FrmMascotas : Form
{
    private readonly MascotaDAO dao = new MascotaDAO();
    private readonly TextBox txtNombre, txtEspecie, txtRaza;
    private readonly DateTimePicker dtpNacimiento;
    private readonly ComboBox cmbCliente;
    private readonly DataGridView dgv;
    private int idSeleccionado = 0;

    public FrmMascotas()
    {
        Text = "Mascotas";
        ClientSize = new Size(800, 580);
        StartPosition = FormStartPosition.CenterParent;

        UiHelper.Etiqueta(this, "Nombre:", 20, 20);
        txtNombre = UiHelper.Texto(this, 150, 20);
        UiHelper.Etiqueta(this, "Especie:", 20, 55);
        txtEspecie = UiHelper.Texto(this, 150, 55);
        UiHelper.Etiqueta(this, "Raza:", 20, 90);
        txtRaza = UiHelper.Texto(this, 150, 90);
        UiHelper.Etiqueta(this, "Fecha nacimiento:", 20, 125);
        dtpNacimiento = UiHelper.Fecha(this, 150, 125);
        UiHelper.Etiqueta(this, "Dueño:", 20, 160);
        cmbCliente = UiHelper.Combo(this, 150, 160);

        UiHelper.Boton(this, "Guardar", 20, 205, (s, e) => Guardar());
        UiHelper.Boton(this, "Actualizar", 130, 205, (s, e) => Actualizar());
        UiHelper.Boton(this, "Eliminar", 240, 205, (s, e) => Eliminar());
        UiHelper.Boton(this, "Limpiar", 350, 205, (s, e) => Limpiar());

        dgv = UiHelper.Grid(this, 260, 310);
        dgv.SelectionChanged += (s, e) => SeleccionarFila();

        CargarClientes();
        CargarGrid();
        Limpiar();
    }

    private void CargarClientes()
    {
        UiHelper.Ejecutar(() =>
        {
            cmbCliente.DataSource = new ClienteDAO().Listar();
            cmbCliente.DisplayMember = "Nombre";
            cmbCliente.ValueMember = "Id";
        });
    }

    private void CargarGrid()
    {
        UiHelper.Ejecutar(() =>
        {
            dgv.DataSource = dao.Listar();
            if (dgv.Columns["ClienteId"] != null) dgv.Columns["ClienteId"].Visible = false;
            if (dgv.Columns["FechaNacimiento"] != null)
                dgv.Columns["FechaNacimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
        });
    }

    private void SeleccionarFila()
    {
        if (dgv.SelectedRows.Count == 0) return;
        if (dgv.SelectedRows[0].DataBoundItem is Mascota m)
        {
            idSeleccionado = m.Id;
            txtNombre.Text = m.Nombre;
            txtEspecie.Text = m.Especie;
            txtRaza.Text = m.Raza;
            dtpNacimiento.Value = m.FechaNacimiento;
            cmbCliente.SelectedValue = m.ClienteId;
        }
    }

    private void Limpiar()
    {
        idSeleccionado = 0;
        txtNombre.Clear();
        txtEspecie.Clear();
        txtRaza.Clear();
        dtpNacimiento.Value = DateTime.Today;
        if (cmbCliente.Items.Count > 0) cmbCliente.SelectedIndex = 0;
        dgv.ClearSelection();
        txtNombre.Focus();
    }

    private Mascota LeerFormulario()
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            UiHelper.Aviso("El nombre es obligatorio.");
            return null;
        }
        if (cmbCliente.SelectedValue == null)
        {
            UiHelper.Aviso("Primero debe registrar un cliente (dueño).");
            return null;
        }
        if (dtpNacimiento.Value.Date > DateTime.Today)
        {
            UiHelper.Aviso("La fecha de nacimiento no puede ser futura.");
            return null;
        }
        return new Mascota
        {
            Id = idSeleccionado,
            Nombre = txtNombre.Text.Trim(),
            Especie = txtEspecie.Text.Trim(),
            Raza = txtRaza.Text.Trim(),
            FechaNacimiento = dtpNacimiento.Value.Date,
            ClienteId = (int)cmbCliente.SelectedValue
        };
    }

    private void Guardar()
    {
        var m = LeerFormulario();
        if (m == null) return;
        if (UiHelper.Ejecutar(() => dao.Insertar(m)))
        {
            CargarGrid();
            Limpiar();
        }
    }

    private void Actualizar()
    {
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione una mascota de la lista."); return; }
        var m = LeerFormulario();
        if (m == null) return;
        if (UiHelper.Ejecutar(() => dao.Actualizar(m)))
        {
            CargarGrid();
            Limpiar();
        }
    }

    private void Eliminar()
    {
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione una mascota de la lista."); return; }
        if (!UiHelper.Confirmar("¿Eliminar esta mascota?")) return;
        if (UiHelper.Ejecutar(() => dao.Eliminar(idSeleccionado)))
        {
            CargarGrid();
            Limpiar();
        }
    }
}
