using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Entidades;

namespace AccesoDatos.Controladores
{
    public class AccesoProductos
    {
        // Metodos De Producto (Crear, Actualizar, Eliminar) //
        public void CrearProducto(Productos producto)
        {
            try
            {
                string query = "INSERT INTO Productos" +
                    "(Descripcion,PrecioUnitario) " +
                    "VALUES" +
                    "(@Descripcion,@PrecioUnitario);select scope_identity()";

                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlTransaction transaction;
                    con.Open();
                    transaction = con.BeginTransaction();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                            cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);

                            
                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out int idProducto))
                            {
                                throw new Exception("Ocurrio un error al obtener el id del Producto");
                            }
                            Console.WriteLine("ID: " + idProducto);
                            ProductoExistencia productoExistencia = new ProductoExistencia();
                            productoExistencia.AgregarExistenciaEnCero(con, transaction, idProducto);
                        }
                        transaction.Commit();
                    }catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ActualizarProducto(Productos producto)
        {
            try
            {
                string query = "UPDATE Productos SET Descripcion = @Descripcion, PrecioUnitario = @PrecioUnitario WHERE Id = @Id";

                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Id", producto.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EliminarProducto(int id)
        {
            try
            {
                string query = "DELETE FROM Existencias WHERE ProductoId = @Id; DELETE FROM Productos where Id = @Id";

                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@Id", id);

                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SqlDataAdapter ObtenerProductos()
        {
            try
            {   
                string query = "SELECT * FROM Productos";
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
