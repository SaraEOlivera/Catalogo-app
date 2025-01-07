using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    internal class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        //leer el lector desde el exterior
        public SqlDataReader Lector 
        {
            get { return lector;}
        }

        public AccesoDatos() 
        {
            conexion = new SqlConnection("server= .\\SQLEXPRESS;  database = CATALOGO_DB; Integrated Security = true");
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta) 
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        public void ejecutarConsulta() 
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void cerrarConexion() 
        {
            if (lector != null) 
            {
                lector.Close();
                conexion.Close();
            }

        }







    }
}
