using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using static System.Net.Mime.MediaTypeNames;

namespace Articulo
{
    public partial class FrmAltaArticulo : Form
    {
        private Dominio.Articulo articulo = null;
        private OpenFileDialog archivo = null;

        public FrmAltaArticulo()
        {
            InitializeComponent();
        }

        public FrmAltaArticulo( Dominio.Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Artículo";
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Validar form de alta
        private bool validarCamposAlta() 
        {
            if (string.IsNullOrEmpty(txtCodigo.Text)) 
            {
                MessageBox.Show("Debe completar el campo Código", "Ata de artículo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return true;
            }
            if (string.IsNullOrEmpty(txtNombre.Text)) 
            {
                MessageBox.Show("Debe completar el campo Nombre", "Ata de artículo",
                     MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            return false;
        }


        private bool validarRepetidos(string cod, string nombre ,string imgUrl ) 
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Dominio.Articulo> listaArticulos = negocio.listar();

            foreach (Dominio.Articulo articulo in listaArticulos)
            {
                if (articulo.Codigo == cod)
                {
                    MessageBox.Show("Este código ya está registrado");
                    return false;
                }
                if (articulo.Nombre == nombre)
                {
                    MessageBox.Show("Este nombre ya está registrado");
                    return false;
                }
                if (articulo.ImagenUrl == imgUrl)
                {
                    MessageBox.Show("Esta imagen ya está registrada");
                    return false;
                }


            }
            return true;
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio datos = new ArticuloNegocio();
            string imagenUrl = txtImagenUrl.Text;
            string codigo = txtCodigo.Text;
            string nombre = txtNombre.Text;

            try
            {
                if (validarCamposAlta())
                    return;

                if (!(validarRepetidos(codigo, nombre, imagenUrl)))
                {
                    return;
                }

                if (articulo == null)
                    articulo = new Dominio.Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.ImagenUrl = txtImagenUrl.Text;

                //desplegables
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;

                if (articulo.Id != 0)
                {
                    datos.modificar(articulo);
                    MessageBox.Show("Artículo Modificado");
                }
                else 
                {
                    datos.agregar(articulo);
                    MessageBox.Show("Artículo Agregado");
                }

                //si la img se levanta local
                if(archivo != null && !(txtImagenUrl.Text.Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["img-folder"] + archivo.SafeFileName);

                    Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); ;
            }
        }

        private void FrmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboCategoria.DataSource = categoriaNegocio.listar();

                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null) 
                {
                    txtCodigo.Text = articulo.Codigo.ToString();
                    txtNombre.Text = articulo.Nombre;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtImagenUrl.Text = articulo.ImagenUrl;

                    //preseleccionar los valores del usuario
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;

                    //img precargada en picturebox
                    cargarImagen(articulo.ImagenUrl);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }



        //Evento copiado del form1
        private void cargarImagen(string imagen)
        {
            try
            {
                pboArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pboArticulo.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            //archivo.ShowDialog();
            if (archivo.ShowDialog() == DialogResult.OK) 
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["img-folder"] + archivo.SafeFileName);


            }
        }
    }
}
