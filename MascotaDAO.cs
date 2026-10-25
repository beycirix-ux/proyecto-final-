using System.Globalization;
using Microsoft.Data.Sqlite;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Datos;

public class MascotaDAO
{
    private const string Formato = "yyyy-MM-dd";

    public void Insertar(Mascota m)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            @"INSERT INTO Mascotas (Nombre, Especie, Raza, FechaNacimiento, ClienteId)
              VALUES (@n, @e, @r, @f, @c)", con);
        cmd.Parameters.AddWithValue("@n", m.Nombre);
        cmd.Parameters.AddWithValue("@e", m.Especie);
        cmd.Parameters.AddWithValue("@r", m.Raza);
        cmd.Parameters.AddWithValue("@f", m.FechaNacimiento.ToString(Formato));
        cmd.Parameters.AddWithValue("@c", m.ClienteId);
        cmd.ExecuteNonQuery();
    }

    public List<Mascota> Listar()
    {
        var lista = new List<Mascota>();
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            @"SELECT m.Id, m.Nombre, m.Especie, m.Raza, m.FechaNacimiento, m.ClienteId, c.Nombre
              FROM Mascotas m
              JOIN Clientes c ON c.Id = m.ClienteId
              ORDER BY m.Nombre", con);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Mascota
            {
                Id = r.GetInt32(0),
                Nombre = r.GetString(1),
                Especie = r.IsDBNull(2) ? "" : r.GetString(2),
                Raza = r.IsDBNull(3) ? "" : r.GetString(3),
                FechaNacimiento = r.IsDBNull(4)
                    ? DateTime.Today
                    : DateTime.Parse(r.GetString(4), CultureInfo.InvariantCulture),
                ClienteId = r.GetInt32(5),
                ClienteNombre = r.GetString(6)
            });
        }
        return lista;
    }

    public void Actualizar(Mascota m)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            @"UPDATE Mascotas
              SET Nombre=@n, Especie=@e, Raza=@r, FechaNacimiento=@f, ClienteId=@c
              WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@n", m.Nombre);
        cmd.Parameters.AddWithValue("@e", m.Especie);
        cmd.Parameters.AddWithValue("@r", m.Raza);
        cmd.Parameters.AddWithValue("@f", m.FechaNacimiento.ToString(Formato));
        cmd.Parameters.AddWithValue("@c", m.ClienteId);
        cmd.Parameters.AddWithValue("@id", m.Id);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand("DELETE FROM Mascotas WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
