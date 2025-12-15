using System;
using System.Windows.Forms;

namespace MiniMarket
{
    public partial class CobroForm : Form
    {
        public enum MetodoPago { Efectivo, Tarjeta }

        public double Total { get; }
        public MetodoPago Metodo { get; private set; } = MetodoPago.Efectivo;
        public double MontoEntregado { get; private set; } = 0;
        public double Vuelto => Math.Max(0, MontoEntregado - Total);

        public CobroForm(double total)
        {
            Total = total;
            InitializeComponent();
        }

        private void CobroForm_Load(object sender, EventArgs e)
        {
            lblTotal.Text = $"Total: ${Total}";
            rbEfectivo.Checked = true;
            txtEntregado.Enabled = true;
            txtEntregado.Focus();
        }

        private void rbEfectivo_CheckedChanged(object sender, EventArgs e)
        {
            bool ef = rbEfectivo.Checked;
            Metodo = ef ? MetodoPago.Efectivo : MetodoPago.Tarjeta;
            txtEntregado.Enabled = ef;
            if (!ef) { txtEntregado.Text = ""; lblVuelto.Text = "Vuelto: $0"; }
            txtEntregado.Focus();
        }

        private void txtEntregado_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(txtEntregado.Text.Replace(",", "."), out var entregado))
                entregado = 0;
            MontoEntregado = entregado;
            lblVuelto.Text = $"Vuelto: ${Math.Max(0, entregado - Total)}";
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Metodo == MetodoPago.Efectivo && MontoEntregado < Total)
            {
                MessageBox.Show("El monto entregado es menor al total.");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
