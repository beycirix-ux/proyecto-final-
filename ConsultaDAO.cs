using System.Globalization;
using Microsoft.Data.Sqlite;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Datos;

public class ConsultaDAO
{
    private const string Formato = "yyyy-MM-dd";

    public void Insertar(Consulta c)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            @"INSERT INTO Consultas (Fecha, MascotaId, VeterinarioId, Motivo, Diagnostico, Tratamiento)
              VALUES (@f, @m, @v, @mo, @d, @t)", con);
        cmd.Parameters.AddWithValue("@f", c.Fecha.ToString(Formato));
        cmd.Parameters.AddWithValue("@m", c.MascotaId);
        cmd.Parameters.AddWithValue("@v", c.VeterinarioId);
        cmd.Parameters.AddWithValue("@mo", c.Motivo);
        cmd.Parameters.AddWithValue("@d", c.Diagnostico);
        cmd.Parameters.AddWithValue("@t", c.Tratamiento);
        cmd.ExecuteNonQuery();
    }

    public List<Consulta> Listar()
    {
        var lista = new List<Consulta>();
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            @"SELECT c.Id, c.Fecha, c.MascotaId, m.Nombre, c.VeterinarioId, v.Nombre,
                     c.Motivo, c.Diagnostico, c.Tratamiento
              FROM Consultas c
              JOIN Mascotas m     ON m.Id = c.MascotaId
              JOIN Veterinarios v ON v.Id = c.VeterinarioId
              ORDER BY c.Fecha DESC, c.Id DESC", con);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Consulta
            {
                Id = r.GetInt32(0),
                Fecha = DateTime.Parse(r.GetString(1), CultureInfo.InvariantCulture),
                MascotaId = r.GetInt32(2),
                MascotaNombre = r.GetString(3),
                VeterinarioId = r.GetInt32(4),
                VeterinarioNombre = r.GetString(5),
                Motivo = r.IsDBNull(6) ? "" : r.GetString(6),
                Diagnostico = r.IsDBNull(7) ? "" : r.GetString(7),
                Tratamiento = r.IsDBNull(8) ? "" : r.GetString(8)
            });
        }
        return lista;
    }

    public void Actualizar(Consulta c)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            @"UPDATE Consultas
              SET Fecha=@f, MascotaId=@m, VeterinarioId=@v, Motivo=@mo, Diagnostico=@d, Tratamiento=@t
              WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@f", c.Fecha.ToString(Formato));
        cmd.Parameters.AddWithValue("@m", c.MascotaId);
        cmd.Parameters.AddWithValue("@v", c.VeterinarioId);
        cmd.Parameters.AddWithValue("@mo", c.Motivo);
        cmd.Parameters.AddWithValue("@d", c.Diagnostico);
        cmd.Parameters.AddWithValue("@t", c.Tratamiento);
        cmd.Parameters.AddWithValue("@id", c.Id);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand("DELETE FROM Consultas WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
