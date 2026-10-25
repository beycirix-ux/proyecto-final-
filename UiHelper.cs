using Microsoft.Data.Sqlite;

namespace VeterinariaApp.Formularios;

/// <summary>Métodos de apoyo para construir los formularios por código.</summary>
public static class UiHelper
{
    public static Label Etiqueta(Control padre, string texto, int x, int y)
    {
        var l = new Label { Text = texto, Left = x, Top = y + 3, AutoSize = true };
        padre.Controls.Add(l);
        return l;
    }

    public static TextBox Texto(Control padre, int x, int y, int ancho = 280)
    {
        var t = new TextBox { Left = x, Top = y, Width = ancho };
        padre.Controls.Add(t);
        return t;
    }

    public static ComboBox Combo(Control padre, int x, int y, int ancho = 280)
    {
        var c = new ComboBox { Left = x, Top = y, Width = ancho, DropDownStyle = ComboBoxStyle.DropDownList };
        padre.Controls.Add(c);
        return c;
    }

    public static DateTimePicker Fecha(Control padre, int x, int y, int ancho = 140)
    {
        var d = new DateTimePicker { Left = x, Top = y, Width = ancho, Format = DateTimePickerFormat.Short };
        padre.Controls.Add(d);
        return d;
    }

    public static Button Boton(Control padre, string texto, int x, int y, EventHandler click)
    {
        var b = new Button { Text = texto, Left = x, Top = y, Width = 100, Height = 30 };
        b.Click += click;
        padre.Controls.Add(b);
        return b;
    }

    public static DataGridView Grid(Control padre, int top, int alto)
    {
        var g = new DataGridView
        {
            Left = 10,
            Top = top,
            Width = padre.ClientSize.Width - 20,
            Height = alto,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false
        };
        padre.Controls.Add(g);
        return g;
    }

    public static void Aviso(string mensaje) =>
        MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Error(string mensaje) =>
        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

    public static bool Confirmar(string mensaje) =>
        MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            == DialogResult.Yes;

    /// <summary>Ejecuta una acción de BD mostrando un mensaje amigable si falla.</summary>
    public static bool Ejecutar(Action accion)
    {
        try
        {
            accion();
            return true;
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 19)   // restricción (FK, NOT NULL...)
        {
            Error("No se pudo completar la operación: el registro está relacionado con otros datos.");
        }
        catch (Exception ex)
        {
            Error("Error de base de datos: " + ex.Message);
        }
        return false;
    }
}
