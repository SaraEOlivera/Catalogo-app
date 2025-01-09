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
            //Desplegables para elegir el campo
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Marca");
            validarEstadoBotones();
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null) 
            {
                Dominio.Articulo seleccionado = (Dominio.Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
            validarEstadoBotones();
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
            validarEstadoBotones();

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
                    validarEstadoBotones();

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //carga 2do desplegable segun 1ro
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else 
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");


            }
        }

        //validar filtro
        private bool validarFiltro() 
        {
            if (cboCampo.SelectedIndex < 0) 
            {
                MessageBox.Show("Debe seleccionar un Campo", "Filtrar por criterios");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un Criterio", "Filtrar por criterios");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio") 
            {
                //validar que el filtro no este vacio
                if (string.IsNullOrEmpty(txtFiltro.Text)) 
                {
                    MessageBox.Show("Hay que completar el filtro", "Filtrar por criterios");
                    return true;
                }
                if (!(soloNumeros(txtFiltro.Text))) 
                {
                    MessageBox.Show("Solo se pueden ingresar números bajo este campo", "Filtrar por criterios");
                    return true;
                }
            }
            return false;
        }


        private bool soloNumeros(string cadena) 
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        //capturar campos
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio datos = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;

                dgvArticulo.DataSource = datos.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            validarEstadoBotones();

        }

        //Validar botones
        private void validarEstadoBotones() 
        {
            if (dgvArticulo.Rows.Count == 0)
            {
                btnModificar.Enabled = false;
                btnEliminar.Enabled = false;
            }
            else 
            {
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }
    }
}
