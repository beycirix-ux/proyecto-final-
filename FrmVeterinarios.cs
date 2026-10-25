using VeterinariaApp.Datos;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Formularios;

public class FrmVeterinarios : Form
{
    private readonly VeterinarioDAO dao = new VeterinarioDAO();
    private readonly TextBox txtNombre, txtEspecialidad;
    private readonly DataGridView dgv;
    private int idSeleccionado = 0;

    public FrmVeterinarios()
    {
        Text = "Veterinarios";
        ClientSize = new Size(800, 520);
        StartPosition = FormStartPosition.CenterParent;

        UiHelper.Etiqueta(this, "Nombre:", 20, 20);
        txtNombre = UiHelper.Texto(this, 130, 20);
        UiHelper.Etiqueta(this, "Especialidad:", 20, 55);
        txtEspecialidad = UiHelper.Texto(this, 130, 55);

        UiHelper.Boton(this, "Guardar", 20, 100, (s, e) => Guardar());
        UiHelper.Boton(this, "Actualizar", 130, 100, (s, e) => Actualizar());
        UiHelper.Boton(this, "Eliminar", 240, 100, (s, e) => Eliminar());
        UiHelper.Boton(this, "Limpiar", 350, 100, (s, e) => Limpiar());

        dgv = UiHelper.Grid(this, 155, 355);
        dgv.SelectionChanged += (s, e) => SeleccionarFila();

        CargarGrid();
        Limpiar();
    }

    private void CargarGrid()
    {
        UiHelper.Ejecutar(() => dgv.DataSource = dao.Listar());
    }

    private void SeleccionarFila()
    {
        if (dgv.SelectedRows.Count == 0) return;
        if (dgv.SelectedRows[0].DataBoundItem is Veterinario v)
        {
            idSeleccionado = v.Id;
            txtNombre.Text = v.Nombre;
            txtEspecialidad.Text = v.Especialidad;
        }
    }

    private void Limpiar()
    {
        idSeleccionado = 0;
        txtNombre.Clear();
        txtEspecialidad.Clear();
        dgv.ClearSelection();
        txtNombre.Focus();
    }

    private Veterinario LeerFormulario()
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            UiHelper.Aviso("El nombre es obligatorio.");
            return null;
        }
        return new Veterinario
        {
            Id = idSeleccionado,
            Nombre = txtNombre.Text.Trim(),
            Especialidad = txtEspecialidad.Text.Trim()
        };
    }

    private void Guardar()
    {
        var v = LeerFormulario();
        if (v == null) return;
        if (UiHelper.Ejecutar(() => dao.Insertar(v)))
        {
            CargarGrid();
            Limpiar();
        }
    }

    private void Actualizar()
    {
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione un veterinario de la lista."); return; }
        var v = LeerFormulario();
        if (v == null) return;
        if (UiHelper.Ejecutar(() => dao.Actualizar(v)))
        {
            CargarGrid();
            Limpiar();
        }
    }

    private void Eliminar()
    {
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione un veterinario de la lista."); return; }
        if (!UiHelper.Confirmar("¿Eliminar este veterinario?")) return;
        if (UiHelper.Ejecutar(() => dao.Eliminar(idSeleccionado)))
        {
            CargarGrid();
            Limpiar();
        }
    }
}
