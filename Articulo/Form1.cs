using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Articulo;
using Dominio;
using Negocio;


namespace Presentacion
{

    public partial class formPrincipal : Form
    {
        private List<Dominio.Articulo> listaArticulo;

        public formPrincipal()
        {
            InitializeComponent();
        }



        private void formPrincipal_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
             Dominio.Articulo seleccionado = (Dominio.Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.ImagenUrl);
        }

        private void cargar() 
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                //limpiar el datasource
                dgvArticulo.DataSource = null;
                dgvArticulo.DataSource = listaArticulo;
                dgvArticulo.Columns["ImagenUrl"].Visible = false;
                dgvArticulo.Columns["Categoria"].Visible = false;
                dgvArticulo.Columns["Id"].Visible = false;


                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAltaArticulo alta = new FrmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Dominio.Articulo seleccionado;
            seleccionado = (Dominio.Articulo)dgvArticulo.CurrentRow.DataBoundItem;

            FrmAltaArticulo modificar = new FrmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio datos = new ArticuloNegocio();
            Dominio.Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Quiere eliminar este artículo?",
                    "Eliminar artículo", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes) 
                {
                    seleccionado = (Dominio.Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                    datos.eliminar(seleccionado.Id);
                    //actualizar la grilla sin los eliminados
                    cargar();
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
    }
}
