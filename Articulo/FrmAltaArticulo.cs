using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio datos = new ArticuloNegocio();
            try
            {
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
    }
}
