namespace VeterinariaApp.Modelos;

public class Consulta
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int MascotaId { get; set; }
    public string MascotaNombre { get; set; }        // solo para mostrar (JOIN)
    public int VeterinarioId { get; set; }
    public string VeterinarioNombre { get; set; }    // solo para mostrar (JOIN)
    public string Motivo { get; set; }
    public string Diagnostico { get; set; }
    public string Tratamiento { get; set; }
}
