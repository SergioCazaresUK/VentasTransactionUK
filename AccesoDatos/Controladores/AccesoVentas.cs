﻿using AccesoDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Controladores
{
    public class AccesoVentas
    {
        public void crearVenta(Venta venta)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlTransaction transaction;
                    con.Open();
                    transaction = con.BeginTransaction();

                    try
                    {
                        string query = "select top(1) Folio from Folios";
                        int folioActual = 0;
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out folioActual))
                            {
                                throw new Exception("Ocurrio un error al obtener el folio");
                            }
                        }

                        

                        query = "INSERT INTO Ventas " +
                            "(Folio,Fecha,ClienteId,Total) " +
                            "VALUES " +
                            "(@Folio,@Fecha,@ClienteId,@Total);select scope_identity()";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@Folio", venta.Folio);
                            cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                            cmd.Parameters.AddWithValue("@ClienteId", venta.CLienteId);
                            cmd.Parameters.AddWithValue("@Total", venta.Total);


                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out int idVenta))
                            {
                                throw new Exception("Ocurrio un error al obtener el id de la venta");
                            }
                            venta.Id = idVenta;
                        }

                        foreach (VentaDetalle concepto in venta.Conceptos)
                        {

                            query = "INSERT INTO VentasDetalle" +
                                    "(VentaId,ProductoId,Cantidad,Descripcion,PrecioUnitario,Importe) " +
                                    "VALUES" +
                                    "(@VentaId,@ProductoId,@Cantidad,@Descripcion,@PrecioUnitario,@Importe)";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Transaction = transaction;

                                cmd.Parameters.AddWithValue("@VentaId", venta.Id);
                                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                                cmd.Parameters.AddWithValue("@Descripcion", concepto.Descripcion);
                                cmd.Parameters.AddWithValue("@PrecioUnitario", concepto.PrecioUnitario);
                                cmd.Parameters.AddWithValue("@Importe", concepto.Importe);
                                cmd.ExecuteNonQuery();

                            }

                            
                            query = "UPDATE Existencias " +
                                    "SET Existencia -= @Cantidad " +
                                    "WHERE ProductoId = @ProductoId";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Transaction = transaction;

                                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        query = "Update Folios set Folio = Folio + 1 ";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.ExecuteNonQuery();
                        }



                        transaction.Commit();

                        //MessageBox.Show($"Venta guardada correctamente con folio {venta.Folio}");

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                //MessageBox.Show($"Ocurrio un error al guardar la venta {ex.Message}");
            }
        }
    }
}
