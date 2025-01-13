using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Dominio;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;

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
                comando.CommandText = "Select Codigo, Nombre, A.Descripcion as Descripcion, Precio, ImagenUrl, M.Descripcion as Marca, C.Descripcion as Categoria, M.Id as MarcaId, c.Id as CategoriaId, A.Id as ArticuloId from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo auxiliar = new Articulo();
                    auxiliar.Id = (int)lector["ArticuloId"];
                    auxiliar.Codigo = (string)lector["Codigo"];
                    auxiliar.Nombre = (string)lector["Nombre"];
                    auxiliar.Precio = lector.GetDecimal(lector.GetOrdinal("Precio"));

                    if (!(lector["ImagenUrl"] is DBNull))
                        auxiliar.ImagenUrl = (string)lector["ImagenUrl"];


                    //Validar descripcion para la vista
                    if (lector["Descripcion"] is DBNull)                   
                        auxiliar.Descripcion = "Este artículo no contiene una descripción";                    
                    else 
                        auxiliar.Descripcion = (string)lector["Descripcion"];


                    auxiliar.Marca = new Marca();
                    auxiliar.Marca.Id = (int)lector["CategoriaId"];
                    auxiliar.Marca.Descripcion = (string)lector["Marca"];

                    auxiliar.Categoria = new Categoria();
                    auxiliar.Categoria.Id = (int)lector["MarcaId"];
                    auxiliar.Categoria.Descripcion = (string)lector["Categoria"];

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

                datos.setearConsulta("INSERT INTO ARTICULOS(Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria, ImagenUrl) VALUES (@Codigo, @Nombre,@Descripcion, @Precio, @IdMarca, @IdCategoria, @ImagenUrl)");
                 datos.setearParametro("@Codigo", nuevo.Codigo);
                 datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Descripcion", nuevo.Descripcion); 
                 datos.setearParametro("@Precio", nuevo.Precio);
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

        public void modificar(Articulo art)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(" Update ARTICULOS Set Codigo = @codigo, Nombre = @nombre, ImagenUrl = @imagenUrl,Descripcion = @descripcion, Precio = @precio, IdMarca = @idMarca, IdCategoria = @idCategoria Where Id = @id");
                datos.setearParametro("@codigo ", art.Codigo);
                datos.setearParametro("@nombre", art.Nombre);
                datos.setearParametro("@imagenUrl", art.ImagenUrl);
                datos.setearParametro("@descripcion", art.Descripcion);
                datos.setearParametro("@precio", art.Precio);
                datos.setearParametro("@idMarca", art.Marca.Id);
                datos.setearParametro("@idCategoria", art.Categoria.Id);
                datos.setearParametro("@id", art.Id);

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

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("Delete from ARTICULOS Where Id= @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List <Articulo>filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo>lista = new List<Articulo> ();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "Select Codigo, Nombre, Precio, ImagenUrl, M.Descripcion as Marca, C.Descripcion as Categoria, M.Id as MarcaId, c.Id as CategoriaId, A.Id as ArticuloId from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria and ";
                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' "; 
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "' "; 
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%' ";
                            break;
                    }
                }
                else if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%' "; 
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "' "; 
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%' ";
                            break;
                    }
                }


                datos.setearConsulta(consulta);
                datos.ejecutarConsulta();

                //while de listar()
                while (datos.Lector.Read())
                {
                    Articulo auxiliar = new Articulo();
                    auxiliar.Id = (int)datos.Lector["ArticuloId"];
                    auxiliar.Codigo = (string)datos.Lector["Codigo"];
                    auxiliar.Nombre = (string)datos.Lector["Nombre"];
                    auxiliar.Precio = datos.Lector.GetDecimal(datos.Lector.GetOrdinal("Precio"));

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        auxiliar.ImagenUrl = (string)datos.Lector["ImagenUrl"];


                    auxiliar.Marca = new Marca();
                    auxiliar.Marca.Id = (int)datos.Lector["CategoriaId"];
                    auxiliar.Marca.Descripcion = (string)datos.Lector["Marca"];

                    auxiliar.Categoria = new Categoria();
                    auxiliar.Categoria.Id = (int)datos.Lector["MarcaId"];
                    auxiliar.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    lista.Add(auxiliar);
                }


                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


}
