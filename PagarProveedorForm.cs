using System;
using System.Drawing;
using System.Windows.Forms;
using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public class PagarProveedorForm : Form
    {
        private ComboBox cboProveedor;
        private TextBox txtMonto;
        private TextBox txtDescripcion;
        private Button btnPagar;
        private readonly ProveedorService _svc = new ProveedorService();

        public PagarProveedorForm()
        {
            InitializeComponent();
            CargarProveedores();
        }

        private void InitializeComponent()
        {
            this.Text = "Pagar a Proveedor";
            this.Size = new Size(520, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            var lblP = new Label { Text = "Proveedor:", Left = 20, Top = 25, AutoSize = true };
            cboProveedor = new ComboBox
            {
                Left = 110,
                Top = 20,
                Width = 360,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var lblM = new Label { Text = "Monto:", Left = 20, Top = 65, AutoSize = true };
            txtMonto = new TextBox { Left = 110, Top = 60, Width = 150, Text = "0" };

            var lblD = new Label { Text = "Descripción:", Left = 20, Top = 105, AutoSize = true };
            txtDescripcion = new TextBox { Left = 110, Top = 100, Width = 360 };

            btnPagar = new Button
            {
                Text = "Pagar",
                Left = 110,
                Top = 145,
                Width = 120,
                Height = 35
            };
            btnPagar.Click += (s, e) => Pagar();

            this.Controls.AddRange(new Control[] { lblP, cboProveedor, lblM, txtMonto, lblD, txtDescripcion, btnPagar });
        }

        private void CargarProveedores()
        {
            var provs = _svc.GetProveedores();
            cboProveedor.DataSource = provs;
            cboProveedor.DisplayMember = "Nombre";
            cboProveedor.ValueMember = "Id";
        }

        private void Pagar()
        {
            if (cboProveedor.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un proveedor.");
                return;
            }

            if (!double.TryParse(txtMonto.Text, out var monto) || monto <= 0)
            {
                MessageBox.Show("Monto inválido.");
                return;
            }

            int proveedorId = (int)cboProveedor.SelectedValue;
            string desc = (txtDescripcion.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(desc)) desc = "Pago proveedor";

            try
            {
                // ABONO = pago
                _svc.RegistrarAbono(proveedorId, monto, desc);
                MessageBox.Show("Pago registrado.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error pagando proveedor: " + ex.Message);
            }
        }
    }
}
