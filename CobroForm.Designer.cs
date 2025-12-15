using System.Drawing;
using System.Windows.Forms;

namespace MiniMarket
{
    partial class CobroForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblTotal;
        private RadioButton rbEfectivo;
        private RadioButton rbTarjeta;
        private Label lblEntregado;
        private TextBox txtEntregado;
        private Label lblVuelto;
        private Button btnConfirmar;
        private Button btnCancelar;

        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();
            this.lblTotal = new Label();
            this.rbEfectivo = new RadioButton();
            this.rbTarjeta = new RadioButton();
            this.lblEntregado = new Label();
            this.txtEntregado = new TextBox();
            this.lblVuelto = new Label();
            this.btnConfirmar = new Button();
            this.btnCancelar = new Button();

            // Form
            this.Text = "Cobro";
            this.ClientSize = new Size(360, 220);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.CobroForm_Load);

            // Título
            lblTitulo.Text = "F12 - Cobrar";
            lblTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitulo.Location = new Point(15, 10);
            lblTitulo.AutoSize = true;

            // Total
            lblTotal.Text = "Total: $0";
            lblTotal.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblTotal.Location = new Point(15, 40);
            lblTotal.AutoSize = true;

            // Métodos
            rbEfectivo.Text = "Efectivo";
            rbEfectivo.Location = new Point(18, 70);
            rbEfectivo.CheckedChanged += new System.EventHandler(this.rbEfectivo_CheckedChanged);

            rbTarjeta.Text = "Tarjeta";
            rbTarjeta.Location = new Point(110, 70);
            rbTarjeta.CheckedChanged += new System.EventHandler(this.rbEfectivo_CheckedChanged);

            // Entregado
            lblEntregado.Text = "Entregado:";
            lblEntregado.Location = new Point(18, 100);
            lblEntregado.AutoSize = true;

            txtEntregado.Location = new Point(95, 96);
            txtEntregado.Width = 100;
            txtEntregado.TextChanged += new System.EventHandler(this.txtEntregado_TextChanged);

            // Vuelto
            lblVuelto.Text = "Vuelto: $0";
            lblVuelto.Location = new Point(210, 100);
            lblVuelto.AutoSize = true;

            // Botones
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.Location = new Point(180, 160);
            btnConfirmar.Size = new Size(80, 30);
            btnConfirmar.BackColor = Color.FromArgb(34, 197, 94);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);

            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(270, 160);
            btnCancelar.Size = new Size(80, 30);
            btnCancelar.BackColor = Color.FromArgb(248, 113, 113);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            this.Controls.AddRange(new Control[] {
                lblTitulo, lblTotal, rbEfectivo, rbTarjeta, lblEntregado, txtEntregado, lblVuelto, btnConfirmar, btnCancelar
            });
        }
    }
}
