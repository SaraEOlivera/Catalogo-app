﻿using System;
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
    }
}