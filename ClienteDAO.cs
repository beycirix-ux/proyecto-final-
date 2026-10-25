using Microsoft.Data.Sqlite;
using VeterinariaApp.Modelos;

namespace VeterinariaApp.Datos;

public class ClienteDAO
{
    public void Insertar(Cliente c)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            "INSERT INTO Clientes (Nombre, Telefono, Direccion) VALUES (@n, @t, @d)", con);
        cmd.Parameters.AddWithValue("@n", c.Nombre);
        cmd.Parameters.AddWithValue("@t", c.Telefono);
        cmd.Parameters.AddWithValue("@d", c.Direccion);
        cmd.ExecuteNonQuery();
    }

    public List<Cliente> Listar()
    {
        var lista = new List<Cliente>();
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            "SELECT Id, Nombre, Telefono, Direccion FROM Clientes ORDER BY Nombre", con);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Cliente
            {
                Id = r.GetInt32(0),
                Nombre = r.GetString(1),
                Telefono = r.IsDBNull(2) ? "" : r.GetString(2),
                Direccion = r.IsDBNull(3) ? "" : r.GetString(3)
            });
        }
        return lista;
    }

    public void Actualizar(Cliente c)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand(
            "UPDATE Clientes SET Nombre=@n, Telefono=@t, Direccion=@d WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@n", c.Nombre);
        cmd.Parameters.AddWithValue("@t", c.Telefono);
        cmd.Parameters.AddWithValue("@d", c.Direccion);
        cmd.Parameters.AddWithValue("@id", c.Id);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var con = Conexion.Obtener();
        using var cmd = new SqliteCommand("DELETE FROM Clientes WHERE Id=@id", con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
