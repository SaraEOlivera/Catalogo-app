﻿using System;
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
            btnVolver.Visible = false;
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

                // Formater los ceros
                dgvArticulo.Columns["Precio"].DefaultCellStyle.Format = "N2";

                dgvArticulo.Columns["ImagenUrl"].Visible = false;
                dgvArticulo.Columns["Categoria"].Visible = false;
                dgvArticulo.Columns["Id"].Visible = false;
                dgvArticulo.Columns["Descripcion"].Visible = false;


                cargarImagen(listaArticulo[0].ImagenUrl);
                limitarAnchoColumnas();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void limitarAnchoColumnas() 
        {
            foreach (DataGridViewColumn columna in dgvArticulo.Columns)
            {
                columna.Resizable = DataGridViewTriState.False;
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
                MessageBox.Show("Debe seleccionar un Campo", "Filtrar por criterios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un Criterio", "Filtrar por criterios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation); 
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio") 
            {
                //validar que el filtro no este vacio
                if (string.IsNullOrEmpty(txtFiltro.Text)) 
                {
                    MessageBox.Show("Hay que completar el filtro", "Filtrar por criterios",
                        MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                    return true;
                }
                if (!(soloDecimales(txtFiltro.Text))) 
                {
                    MessageBox.Show("Solo se pueden ingresar números bajo este campo", "Filtrar por criterios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return true;
                }
            }
            return false;
        }

        private bool soloDecimales(string cadena) 
        {
            foreach (char caracter in cadena)
            {

                //para que acepte punto en el filtro
                decimal ingresoFiltro;
                if (!(decimal.TryParse(cadena, out ingresoFiltro)))
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

                if (campo == "Precio")
                    filtro = filtro.Replace(',', '.');


                dgvArticulo.DataSource = datos.filtrar(campo, criterio, filtro);
                btnVolver.Visible = true;

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

        private void btnVolver_Click(object sender, EventArgs e)
        {
            cargar();
            btnVolver.Visible = false;
        }

        private void dgvArticulo_CellDoubleClick(object sender, DataGridViewCellEventArgs evento)
        {
            if (evento.RowIndex >= 0) 
            {
                Dominio.Articulo filaSeleccionada = (Dominio.Articulo)dgvArticulo.CurrentRow.DataBoundItem;

                FrmDetalle vista = new FrmDetalle(filaSeleccionada);
                vista.ShowDialog();

            }
            
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
