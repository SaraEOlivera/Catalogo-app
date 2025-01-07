﻿using System;
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

namespace Articulo
{
    public partial class FrmAltaArticulo : Form
    {
        public FrmAltaArticulo()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Dominio.Articulo nuevoArticulo = new Dominio.Articulo();
            ArticuloNegocio datos = new ArticuloNegocio();
            try
            {
                nuevoArticulo.Codigo = txtCodigo.Text;
                nuevoArticulo.Nombre = txtNombre.Text;
                nuevoArticulo.Precio = decimal.Parse(txtPrecio.Text);

                nuevoArticulo.Marca = (Marca)cboMarca.SelectedItem;
                nuevoArticulo.Categoria = (Categoria)cboCategoria.SelectedItem;

                datos.agregar(nuevoArticulo);
                MessageBox.Show("Articulo Agregado");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
