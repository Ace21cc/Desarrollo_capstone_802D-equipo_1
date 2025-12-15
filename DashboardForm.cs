using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniMarket
{
    public partial class DashboardForm : Form
    {
        private readonly string _usuario;
        private readonly string _rol;

        public DashboardForm(string usuario, string rol)
        {
            _usuario = usuario;
            _rol = rol;
            InitializeComponent();

            this.Load += DashboardForm_Load;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (lblUsuario != null)
                    lblUsuario.Text = $"{_usuario} ({_rol})";
            }
            catch { }

            MostrarInicio();

            if (_rol == "trabajador")
            {
                if (btnConfig != null) btnConfig.Enabled = false;
                if (btnCorte != null) btnCorte.Enabled = false;
            }

            if (btnVentas != null) btnVentas.Click += (s, ev) => AbrirVentas();
            if (btnProductos != null) btnProductos.Click += (s, ev) => AbrirProductos();
            if (btnPrediccion != null) btnPrediccion.Click += (s, ev) => AbrirPrediccion();
            if (btnConfig != null) btnConfig.Click += (s, ev) => AbrirUsuarios();
            if (btnCorte != null) btnCorte.Click += (s, ev) => AbrirCorte();
            if (btnInventario != null) btnInventario.Click += (s, ev) => AbrirReportes();

            // ✅ NUEVOS
            if (btnFiados != null) btnFiados.Click += (s, ev) => AbrirFiados();
            if (btnProveedores != null) btnProveedores.Click += (s, ev) => AbrirProveedores();

            if (btnSalir != null) btnSalir.Click += (s, ev) => Application.Exit();
        }

        private void MostrarInicio()
        {
            if (panelContenido == null) return;

            panelContenido.Controls.Clear();

            var lbl = new Label
            {
                Text = "Bienvenido al Sistema de Ventas MiniMarket",
                AutoSize = true,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 61, 110),
                Left = 30,
                Top = 30
            };

            var lbl2 = new Label
            {
                Text = "Use el menú de la izquierda para acceder a Ventas, Productos, Predicción, Corte, Fiados, Proveedores y Reportes.",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Left = 35,
                Top = 75,
                MaximumSize = new Size(800, 0)
            };

            panelContenido.Controls.Add(lbl);
            panelContenido.Controls.Add(lbl2);
        }

        private void CargarFormularioEnPanel(Form formHijo)
        {
            if (panelContenido == null || formHijo == null) return;

            panelContenido.Controls.Clear();

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;

            panelContenido.Controls.Add(formHijo);
            formHijo.Show();
        }

        private void AbrirVentas()
        {
            var vf = new VentasForm();
            CargarFormularioEnPanel(vf);
        }

        private void AbrirFiados()
        {
            var f = new ClientesFiadosForm();
            CargarFormularioEnPanel(f);
        }

        private void AbrirProveedores()
        {
            var f = new ProveedoresForm();
            CargarFormularioEnPanel(f);
        }

        private void AbrirProductos()
        {
            var f = new ProductosForm();
            CargarFormularioEnPanel(f);
        }

        private void AbrirUsuarios()
        {
            var f = new UsuariosForm();
            CargarFormularioEnPanel(f);
        }

        private void AbrirPrediccion()
        {
            var pf = new PrediccionForm();
            CargarFormularioEnPanel(pf);
        }

        private void AbrirCorte()
        {
            var f = new CorteCajaForm();
            CargarFormularioEnPanel(f);
        }

        private void AbrirReportes()
        {
            var f = new ReportesForm();
            CargarFormularioEnPanel(f);
        }
    }
}
