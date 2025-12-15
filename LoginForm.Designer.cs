using System.Drawing;
using System.Windows.Forms;

namespace MiniMarket
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblClave;
        private TextBox txtUsuario;
        private TextBox txtClave;
        private CheckBox chkVerClave;
        private Button btnIngresar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();
            this.lblUsuario = new Label();
            this.lblClave = new Label();
            this.txtUsuario = new TextBox();
            this.txtClave = new TextBox();
            this.chkVerClave = new CheckBox();
            this.btnIngresar = new Button();

            this.Text = "MiniMarket - Login";
            this.ClientSize = new Size(380, 260);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo.Text = "Iniciar Sesión";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);

            lblUsuario.Text = "Usuario";
            lblUsuario.Location = new Point(22, 70);
            lblUsuario.AutoSize = true;

            txtUsuario.Location = new Point(100, 66);
            txtUsuario.Width = 240;
            txtUsuario.Text = "admin";

            lblClave.Text = "Clave";
            lblClave.Location = new Point(22, 110);
            lblClave.AutoSize = true;

            txtClave.Location = new Point(100, 106);
            txtClave.Width = 240;
            txtClave.UseSystemPasswordChar = true;
            txtClave.Text = "1234";

            chkVerClave.Text = "Ver";
            chkVerClave.Location = new Point(100, 136);
            chkVerClave.AutoSize = true;
            chkVerClave.CheckedChanged += new System.EventHandler(this.chkVerClave_CheckedChanged);

            btnIngresar.Text = "Ingresar";
            btnIngresar.Location = new Point(100, 175);
            btnIngresar.Size = new Size(240, 36);
            btnIngresar.BackColor = Color.FromArgb(14, 165, 233);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);

            this.Controls.AddRange(new Control[] {
                lblTitulo, lblUsuario, txtUsuario, lblClave, txtClave, chkVerClave, btnIngresar
            });
        }
    }
}
