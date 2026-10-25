using VeterinariaApp.Datos;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Formularios;

public class FrmClientes : Form
{
    private readonly ClienteDAO dao = new ClienteDAO();
    private readonly TextBox txtNombre, txtTelefono, txtDireccion;
    private readonly DataGridView dgv;
    private int idSeleccionado = 0;

    public FrmClientes()
    {
        Text = "Clientes";
        ClientSize = new Size(800, 560);
        StartPosition = FormStartPosition.CenterParent;

        UiHelper.Etiqueta(this, "Nombre:", 20, 20);
        txtNombre = UiHelper.Texto(this, 130, 20);
        UiHelper.Etiqueta(this, "Teléfono:", 20, 55);
        txtTelefono = UiHelper.Texto(this, 130, 55);
        UiHelper.Etiqueta(this, "Dirección:", 20, 90);
        txtDireccion = UiHelper.Texto(this, 130, 90, 400);

        UiHelper.Boton(this, "Guardar", 20, 135, (s, e) => Guardar());
        UiHelper.Boton(this, "Actualizar", 130, 135, (s, e) => Actualizar());
        UiHelper.Boton(this, "Eliminar", 240, 135, (s, e) => Eliminar());
        UiHelper.Boton(this, "Limpiar", 350, 135, (s, e) => Limpiar());

        dgv = UiHelper.Grid(this, 190, 360);
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
        if (dgv.SelectedRows[0].DataBoundItem is Cliente c)
        {
            idSeleccionado = c.Id;
            txtNombre.Text = c.Nombre;
            txtTelefono.Text = c.Telefono;
            txtDireccion.Text = c.Direccion;
        }
    }

    private void Limpiar()
    {
        idSeleccionado = 0;
        txtNombre.Clear();
        txtTelefono.Clear();
        txtDireccion.Clear();
        dgv.ClearSelection();
        txtNombre.Focus();
    }

    private Cliente LeerFormulario()
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            UiHelper.Aviso("El nombre es obligatorio.");
            return null;
        }
        return new Cliente
        {
            Id = idSeleccionado,
            Nombre = txtNombre.Text.Trim(),
            Telefono = txtTelefono.Text.Trim(),
            Direccion = txtDireccion.Text.Trim()
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
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione un cliente de la lista."); return; }
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
        if (idSeleccionado == 0) { UiHelper.Aviso("Seleccione un cliente de la lista."); return; }
        if (!UiHelper.Confirmar("¿Eliminar este cliente?")) return;
        if (UiHelper.Ejecutar(() => dao.Eliminar(idSeleccionado)))
        {
            CargarGrid();
            Limpiar();
        }
    }
}
