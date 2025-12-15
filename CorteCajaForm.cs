using System;
using System.Drawing;
using System.Windows.Forms;
using MiniMarket.Services;

namespace MiniMarket
{
    public class CorteCajaForm : Form
    {
        private Label lblVentasHoy;
        private TextBox txtCajaInicial;
        private TextBox txtIngresosExtra;
        private TextBox txtPagos;
        private TextBox txtEfectivoFinal;
        private Label lblTotalFinal;
        private Label lblDiferencia;
        private Button btnRecalcular;
        private Button btnGuardarCorte;

        private readonly CorteCajaService _corteService = new CorteCajaService();

        public CorteCajaForm()
        {
            InicializarUI();
            CargarVentasHoy();
            RecalcularUI();
        }

        private void InicializarUI()
        {
            this.Text = "Corte de Caja";
            this.Size = new Size(420, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            var lbl1 = new Label() { Text = "Ventas del día:", Left = 20, Top = 20, AutoSize = true };
            lblVentasHoy = new Label()
            {
                Left = 150,
                Top = 20,
                Width = 220,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lbl2 = new Label() { Text = "Caja inicial:", Left = 20, Top = 70, AutoSize = true };
            txtCajaInicial = new TextBox() { Left = 150, Top = 65, Width = 180, Text = "0" };
            txtCajaInicial.TextChanged += (s, e) => RecalcularUI();

            var lbl3 = new Label() { Text = "Ingresos extra:", Left = 20, Top = 110, AutoSize = true };
            txtIngresosExtra = new TextBox() { Left = 150, Top = 105, Width = 180, Text = "0" };
            txtIngresosExtra.TextChanged += (s, e) => RecalcularUI();

            var lbl4 = new Label() { Text = "Pagos:", Left = 20, Top = 150, AutoSize = true };
            txtPagos = new TextBox() { Left = 150, Top = 145, Width = 180, Text = "0" };
            txtPagos.TextChanged += (s, e) => RecalcularUI();

            var lbl5 = new Label() { Text = "Efectivo final:", Left = 20, Top = 190, AutoSize = true };
            txtEfectivoFinal = new TextBox() { Left = 150, Top = 185, Width = 180, Text = "0" };
            txtEfectivoFinal.TextChanged += (s, e) => RecalcularUI();

            btnRecalcular = new Button()
            {
                Text = "Recalcular",
                Left = 20,
                Top = 240,
                Width = 150,
                Height = 35
            };
            btnRecalcular.Click += (s, e) =>
            {
                CargarVentasHoy();
                RecalcularUI();
            };

            btnGuardarCorte = new Button()
            {
                Text = "Guardar Corte",
                Left = 180,
                Top = 240,
                Width = 150,
                Height = 35
            };
            btnGuardarCorte.Click += BtnGuardarCorte_Click;

            lblTotalFinal = new Label()
            {
                Left = 20,
                Top = 295,
                Width = 360,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "Total final: $0"
            };

            lblDiferencia = new Label()
            {
                Left = 20,
                Top = 325,
                Width = 360,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "Diferencia: $0"
            };

            this.Controls.AddRange(new Control[]
            {
                lbl1, lblVentasHoy,
                lbl2, txtCajaInicial,
                lbl3, txtIngresosExtra,
                lbl4, txtPagos,
                lbl5, txtEfectivoFinal,
                btnRecalcular, btnGuardarCorte,
                lblTotalFinal, lblDiferencia
            });
        }

        private void CargarVentasHoy()
        {
            double total = _corteService.ObtenerTotalVentasHoy();
            lblVentasHoy.Text = "$ " + total.ToString("N0");
        }

        private void RecalcularUI()
        {
            double cajaInicial = ParseDouble(txtCajaInicial.Text);
            double ingresosExtra = ParseDouble(txtIngresosExtra.Text);
            double pagos = ParseDouble(txtPagos.Text);
            double efectivoFinal = ParseDouble(txtEfectivoFinal.Text);

            double ventasHoy = _corteService.ObtenerTotalVentasHoy();
            double totalFinal = cajaInicial + ventasHoy + ingresosExtra - pagos;
            double diferencia = efectivoFinal - totalFinal;

            lblTotalFinal.Text = "Total final: $ " + totalFinal.ToString("N0");
            lblDiferencia.Text = "Diferencia: $ " + diferencia.ToString("N0");
        }

        private void BtnGuardarCorte_Click(object sender, EventArgs e)
        {
            try
            {
                double cajaInicial = ParseDouble(txtCajaInicial.Text);
                double ingresosExtra = ParseDouble(txtIngresosExtra.Text);
                double pagos = ParseDouble(txtPagos.Text);
                double efectivoFinal = ParseDouble(txtEfectivoFinal.Text);

                _corteService.RegistrarCorte(cajaInicial, ingresosExtra, pagos, efectivoFinal);

                MessageBox.Show("Corte guardado correctamente.");
                CargarVentasHoy();
                RecalcularUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private double ParseDouble(string s)
        {
            if (double.TryParse(s, out var v)) return v;
            return 0;
        }
    }
}
