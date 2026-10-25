namespace VeterinariaApp.Formularios;

public class FrmPrincipal : Form
{
    public FrmPrincipal()
    {
        Text = "Sistema de Control Clínico Veterinario v2";
        ClientSize = new Size(700, 400);
        StartPosition = FormStartPosition.CenterScreen;

        var menu = new MenuStrip();
        menu.Items.Add("Clientes", null, (s, e) => Abrir(new FrmClientes()));
        menu.Items.Add("Mascotas", null, (s, e) => Abrir(new FrmMascotas()));
        menu.Items.Add("Veterinarios", null, (s, e) => Abrir(new FrmVeterinarios()));
        menu.Items.Add("Consultas", null, (s, e) => Abrir(new FrmConsultas()));
        menu.Items.Add("Salir", null, (s, e) => Close());
        MainMenuStrip = menu;
        Controls.Add(menu);

        Controls.Add(new Label
        {
            Text = "Sistema de Control Clínico Veterinario v2\n\nSeleccione un módulo del menú superior.",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14)
        });
    }

    private void Abrir(Form formulario)
    {
        using (formulario)
            formulario.ShowDialog(this);
    }
}
