using System;
using System.Windows.Forms;
using MiniMarket.Services;
using MiniMarket.Models;

namespace MiniMarket
{
    public partial class LoginForm : Form
    {
        private readonly UserService _userService = new UserService();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void chkVerClave_CheckedChanged(object sender, EventArgs e)
        {
            txtClave.UseSystemPasswordChar = !chkVerClave.Checked;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();
                string clave   = txtClave.Text.Trim();

                if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
                {
                    MessageBox.Show("Debe ingresar usuario y clave.",
                        "Login",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // usa el nuevo método Login del UserService
                UsuarioRow? u = _userService.Login(usuario, clave);

                if (u == null)
                {
                    MessageBox.Show("Usuario o clave incorrectos.",
                        "Login",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtClave.Clear();
                    txtClave.Focus();
                    return;
                }

                // DashboardForm espera (string usuario, string rol)
                this.Hide();
                using (var dash = new DashboardForm(u.Usuario, u.Rol))
                {
                    dash.ShowDialog();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
