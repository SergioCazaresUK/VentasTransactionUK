using AccesoDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Controladores
{
    public class ProductoExistencia
    {
        public void ActualizarExistencia(int id, decimal valor)
        {
            try
            {
                // Query para Actualizar una Existencia //
                string query = "UPDATE Existencias SET Existencia = @Valor WHERE Id= @Id";

                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@Valor", valor);
                            cmd.Parameters.AddWithValue("@Id", id);

                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AgregarExistenciaEnCero(SqlConnection con, SqlTransaction transaction, int productoId)
        {
            string query = "INSERT INTO Existencias (ProductoId, Existencia) " +
                "VALUES (@ProductoId, @Existencia)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@ProductoId", productoId);
                    cmd.Parameters.AddWithValue("@Existencia", 0);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public SqlDataAdapter ObtenerExistencias()
        {
            try
            {
                string query = "SELECT Productos.Id, Descripcion, PrecioUnitario, Existencia FROM Existencias INNER JOIN Productos ON ProductoId = Productos.Id";
                SqlDataAdapter productos = new SqlDataAdapter(query, Conexion.ConnectionString);

                return productos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
