using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MiniMarket.Models;
using MiniMarket.Services;
using MiniMarket.Views;

namespace MiniMarket
{
    public partial class VentasForm : Form
    {
        private readonly ProductService _productService = new();
        private readonly VentaService _ventaService = new();
        private readonly ClienteFiadoService _fiadoService = new();
        private readonly ProveedorService _proveedorService = new();

        private List<Producto> _productos = new();
        private double _total = 0;

        public VentasForm()
        {
            InitializeComponent();
            CargarInicial();
        }

        private void CargarInicial()
        {
            _productos = _productService.ObtenerTodos();

            cboMetodoPago.Items.AddRange(new[] { "EFECTIVO", "TARJETA", "FIADO" });
            cboMetodoPago.SelectedIndex = 0;
            cboMetodoPago.SelectedIndexChanged += MetodoPagoChanged;

            cboClienteFiado.DisplayMember = "Nombre";
            cboClienteFiado.ValueMember = "Id";
            cboClienteFiado.DataSource = _fiadoService.GetClientes();

            txtCodigo.KeyPress += TxtCodigo_KeyPress;
            btnEliminar.Click += BtnEliminar_Click;
            btnFinalizar.Click += BtnFinalizar_Click;
            btnPagarProveedor.Click += BtnPagarProveedor_Click;
        }

        private void MetodoPagoChanged(object? sender, EventArgs e)
        {
            cboClienteFiado.Visible = cboMetodoPago.SelectedItem!.ToString() == "FIADO";
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;

            var codigo = txtCodigo.Text.Trim();
            var p = _productos.FirstOrDefault(x => x.CodigoBarra == codigo);
            if (p == null)
            {
                MessageBox.Show("Producto no encontrado");
                return;
            }

            dgvCarrito.Rows.Add(
                p.Id,
                p.Nombre,
                1,
                p.Precio,
                p.Precio
            );

            _productService.ActualizarStockUnidades(p.Id, 1);

            _total += p.Precio;
            lblTotal.Text = _total.ToString("N0");

            txtCodigo.Clear();
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count == 0) return;

            var fila = dgvCarrito.SelectedRows[0];
            _total -= Convert.ToDouble(fila.Cells["Subtotal"].Value);
            dgvCarrito.Rows.Remove(fila);
            lblTotal.Text = _total.ToString("N0");
        }

        private void BtnFinalizar_Click(object? sender, EventArgs e)
        {
            var items = new List<(int, double, double)>();

            foreach (DataGridViewRow r in dgvCarrito.Rows)
            {
                items.Add((
                    Convert.ToInt32(r.Cells["ProductoId"].Value),
                    1,
                    Convert.ToDouble(r.Cells["Precio"].Value)
                ));
            }

            int? clienteFiadoId = null;
            if (cboMetodoPago.SelectedItem!.ToString() == "FIADO")
                clienteFiadoId = (int)cboClienteFiado.SelectedValue;

            _ventaService.RegistrarVenta(
                _total,
                cboMetodoPago.SelectedItem.ToString()!,
                items,
                clienteFiadoId
            );

            MessageBox.Show("Venta registrada");
            dgvCarrito.Rows.Clear();
            _total = 0;
            lblTotal.Text = "0";
        }

        private void BtnPagarProveedor_Click(object? sender, EventArgs e)
        {
            using var f = new ProveedoresForm();
            f.ShowDialog();
        }
    }
}
