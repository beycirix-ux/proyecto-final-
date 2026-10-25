using Microsoft.Data.Sqlite;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Datos;

public class VeterinarioDAO
{
    public void Insertar(Veterinario v)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            "INSERT INTO Veterinarios (Nombre, Especialidad) VALUES (@n, @e)", con);
        cmd.Parameters.AddWithValue("@n", v.Nombre);
        cmd.Parameters.AddWithValue("@e", v.Especialidad);
        cmd.ExecuteNonQuery();
    }

    public List<Veterinario> Listar()
    {
        var lista = new List<Veterinario>();
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            "SELECT Id, Nombre, Especialidad FROM Veterinarios ORDER BY Nombre", con);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Veterinario
            {
                Id = r.GetInt32(0),
                Nombre = r.GetString(1),
                Especialidad = r.IsDBNull(2) ? "" : r.GetString(2)
            });
        }
        return lista;
    }

    public void Actualizar(Veterinario v)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            "UPDATE Veterinarios SET Nombre=@n, Especialidad=@e WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@n", v.Nombre);
        cmd.Parameters.AddWithValue("@e", v.Especialidad);
        cmd.Parameters.AddWithValue("@id", v.Id);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand("DELETE FROM Veterinarios WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
