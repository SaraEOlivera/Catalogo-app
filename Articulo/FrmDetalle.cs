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
        private Dominio.Articulo seleccionado;  
        public FrmDetalle()
        {
            InitializeComponent();
        }
        
        public FrmDetalle(Dominio.Articulo articulo)
        {
            InitializeComponent();
            this.seleccionado = articulo;
        }

        private void btnVolverDetalle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmDetalle_Load(object sender, EventArgs e)
        {
            txtCodigoDetalle.Text = seleccionado.Codigo;
            txtNombreDetalle.Text = seleccionado.Nombre;
            txtDescripcionDetalle.Text = seleccionado.Descripcion;
            txtMarcaDetalle.Text = seleccionado.Marca.Descripcion;
            txtCategoriaDetalle.Text = seleccionado.Categoria.Descripcion;
            txtPrecioDetalle.Text = seleccionado.Precio.ToString();

            if (!(string.IsNullOrEmpty(seleccionado.ImagenUrl)))
            {
                try
                {
                    pboImagenUrlDetalle.Load(seleccionado.ImagenUrl);

                }
                catch (Exception)
                {
                    pboImagenUrlDetalle.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
                }
            }
        }
    }
}
