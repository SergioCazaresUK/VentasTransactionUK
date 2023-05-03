using System;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos.Entidades
{
    public class Productos
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

}