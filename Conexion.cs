using Microsoft.Data.Sqlite;

namespace VeterinariaApp.Datos;

public static class Conexion
{
    // La BD se crea junto al ejecutable
    private static readonly string Cadena =
        $"Data Source={Path.Combine(AppContext.BaseDirectory, "veterinaria.db")}";

    public static SqliteConnection Obtener()
    {
        var con = new SqliteConnection(Cadena);
        con.Open();

        // SQLite no valida llaves foráneas por defecto: se activan en cada conexión
        using var pragma = con.CreateCommand();
        pragma.CommandText = "PRAGMA foreign_keys = ON;";
        pragma.ExecuteNonQuery();

        return con;
    }

    public static void Inicializar()
    {
        using var con = Obtener();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Clientes (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre    TEXT NOT NULL,
                Telefono  TEXT,
                Direccion TEXT
            );

            CREATE TABLE IF NOT EXISTS Mascotas (
                Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre          TEXT NOT NULL,
                Especie         TEXT,
                Raza            TEXT,
                FechaNacimiento TEXT,
                ClienteId       INTEGER NOT NULL,
                FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
            );

            CREATE TABLE IF NOT EXISTS Veterinarios (
                Id           INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre       TEXT NOT NULL,
                Especialidad TEXT
            );

            CREATE TABLE IF NOT EXISTS Consultas (
                Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha         TEXT NOT NULL,
                MascotaId     INTEGER NOT NULL,
                VeterinarioId INTEGER NOT NULL,
                Motivo        TEXT,
                Diagnostico   TEXT,
                Tratamiento   TEXT,
                FOREIGN KEY (MascotaId)     REFERENCES Mascotas(Id),
                FOREIGN KEY (VeterinarioId) REFERENCES Veterinarios(Id)
            );";
        cmd.ExecuteNonQuery();
    }
}
