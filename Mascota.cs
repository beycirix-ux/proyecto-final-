namespace VeterinariaApp.Modelos;

public class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Especie { get; set; }
    public string Raza { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; }   // solo para mostrar (JOIN)
}
