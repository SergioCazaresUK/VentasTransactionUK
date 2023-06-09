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
    public class AccesoClientes
    {
        // Metodos De Cliente (Crear, Actualizar, Eliminar) //
        public SqlDataAdapter ObtenerClientes()
        {
            try
            {
                string query = "SELECT * FROM Clientes";
                SqlDataAdapter clientes = new SqlDataAdapter(query, Conexion.ConnectionString);

                return clientes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void CrearCliente(Clientes cliente)
        {
            try
            {
                // Query para insertar un cliente nuevo //
                string query = "INSERT INTO Clientes" +
                    "(Nombre) " +
                    "VALUES" +
                    "(@Nombre)";

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
                            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
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

                throw new Exception(ex.Message);
            }
        }
        public void EliminarCliente(int id)
        {
            try
            {
                // Query para Borrar un cliente //
                string query = "DELETE FROM Clientes where Id = @Id";

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

                            cmd.Parameters.AddWithValue("@Id", id);

                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }catch(Exception ex)
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
        public void ActualizarCliente(int id, string nombre)
        {
            try
            {
                // Query para Actualizar un cliente //
                string query = "UPDATE Clientes SET Nombre = @Nombre WHERE Id= @Id";

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

                            cmd.Parameters.AddWithValue("@Nombre", nombre);
                            cmd.Parameters.AddWithValue("@Id", id);

                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch(Exception ex)
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
    }
}
