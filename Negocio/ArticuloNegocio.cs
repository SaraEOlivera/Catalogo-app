using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar() 
        {
            List<Articulo> lista = new List<Articulo>();

            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server= .\\SQLEXPRESS;  database = CATALOGO_DB; Integrated Security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Codigo, Nombre, Precio, ImagenUrl, M.Descripcion as Marca from ARTICULOS A, MARCAS M where M.Id = A.IdMarca";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo auxiliar = new Articulo();
                    auxiliar.Codigo = (string)lector["Codigo"];
                    auxiliar.Nombre = (string)lector["Nombre"];
                    auxiliar.Precio = lector.GetDecimal(lector.GetOrdinal("Precio"));

                    if (!(lector["ImagenUrl"] is DBNull))
                        auxiliar.ImagenUrl = (string)lector["ImagenUrl"];


                    auxiliar.Marca = new Marca();
                    auxiliar.Marca.Descripcion = (string)lector["Marca"];

                    lista.Add(auxiliar);
                }
                return lista;

            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                conexion.Close();
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Precio, IdMarca, IdCategoria, ImagenUrl) VALUES ("
                            + nuevo.Codigo + ", '"
                            + nuevo.Nombre + "', "
                            + nuevo.Precio + ", @IdMarca, @IdCategoria, @ImagenUrl)");
                datos.setearParametro("@IdMarca", nuevo.Marca.Id);
                datos.setearParametro("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@ImagenUrl", nuevo.ImagenUrl);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                datos.cerrarConexion();
            }
        }


    }



}
