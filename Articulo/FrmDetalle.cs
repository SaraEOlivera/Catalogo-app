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
using Presentacion;

namespace Articulo
{
    public partial class FrmDetalle : Form
    {
        public FrmDetalle()
        {
            InitializeComponent();
        }
        
        public FrmDetalle(Dominio.Articulo articulo)
        {
            InitializeComponent();

            txtCodigoDetalle.Text = articulo.Codigo;
            txtNombreDetalle.Text = articulo.Nombre;
            txtDescripcionDetalle.Text = articulo.Descripcion;
            txtMarcaDetalle.Text = articulo.Marca.Descripcion;
            txtCategoriaDetalle.Text = articulo.Categoria.Descripcion;
            txtPrecioDetalle.Text = articulo.Precio.ToString();

            if (!(string.IsNullOrEmpty(articulo.ImagenUrl))) 
            {
                try
                {
                    pboImagenUrlDetalle.Load(articulo.ImagenUrl);

                }
                catch (Exception)
                {

                    pboImagenUrlDetalle.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
                    ;
                }
            }


        }

        private void btnVolverDetalle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmDetalle_Load(object sender, EventArgs e)
        {

        }
    }
}
