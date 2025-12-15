using System.Drawing;
using System.Windows.Forms;

namespace MiniMarket
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelSide;
        private Panel panelTop;
        private Panel panelContenido;
        private Label lblTituloApp;
        private Label lblUsuario;

        private Button btnVentas;
        private Button btnProductos;
        private Button btnPrediccion;
        private Button btnCorte;
        private Button btnInventario;
        private Button btnConfig;

        // ✅ NUEVOS
        private Button btnFiados;
        private Button btnProveedores;

        private Button btnSalir;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelSide = new Panel();
            this.btnSalir = new Button();
            this.btnConfig = new Button();
            this.btnInventario = new Button();
            this.btnCorte = new Button();
            this.btnPrediccion = new Button();
            this.btnProductos = new Button();
            this.btnVentas = new Button();

            // ✅ NUEVOS
            this.btnFiados = new Button();
            this.btnProveedores = new Button();

            this.lblTituloApp = new Label();
            this.panelTop = new Panel();
            this.lblUsuario = new Label();
            this.panelContenido = new Panel();

            this.panelSide.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();

            // panelSide
            this.panelSide.BackColor = Color.FromArgb(15, 23, 42);
            this.panelSide.Dock = DockStyle.Left;
            this.panelSide.Location = new Point(0, 0);
            this.panelSide.Name = "panelSide";
            this.panelSide.Size = new Size(200, 600);
            this.panelSide.TabIndex = 0;

            // lblTituloApp
            this.lblTituloApp.AutoSize = true;
            this.lblTituloApp.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTituloApp.ForeColor = Color.White;
            this.lblTituloApp.Location = new Point(12, 15);
            this.lblTituloApp.Name = "lblTituloApp";
            this.lblTituloApp.Size = new Size(160, 21);
            this.lblTituloApp.TabIndex = 0;
            this.lblTituloApp.Text = "MiniMarket System";

            // btnVentas
            this.btnVentas.FlatAppearance.BorderSize = 0;
            this.btnVentas.FlatStyle = FlatStyle.Flat;
            this.btnVentas.Font = new Font("Segoe UI", 10F);
            this.btnVentas.ForeColor = Color.White;
            this.btnVentas.Location = new Point(0, 60);
            this.btnVentas.Name = "btnVentas";
            this.btnVentas.Padding = new Padding(15, 0, 0, 0);
            this.btnVentas.Size = new Size(200, 40);
            this.btnVentas.TabIndex = 1;
            this.btnVentas.Text = "Ventas (F1)";
            this.btnVentas.TextAlign = ContentAlignment.MiddleLeft;

            // btnProductos
            this.btnProductos.FlatAppearance.BorderSize = 0;
            this.btnProductos.FlatStyle = FlatStyle.Flat;
            this.btnProductos.Font = new Font("Segoe UI", 10F);
            this.btnProductos.ForeColor = Color.White;
            this.btnProductos.Location = new Point(0, 100);
            this.btnProductos.Name = "btnProductos";
            this.btnProductos.Padding = new Padding(15, 0, 0, 0);
            this.btnProductos.Size = new Size(200, 40);
            this.btnProductos.TabIndex = 2;
            this.btnProductos.Text = "Productos (F3)";
            this.btnProductos.TextAlign = ContentAlignment.MiddleLeft;

            // btnPrediccion
            this.btnPrediccion.FlatAppearance.BorderSize = 0;
            this.btnPrediccion.FlatStyle = FlatStyle.Flat;
            this.btnPrediccion.Font = new Font("Segoe UI", 10F);
            this.btnPrediccion.ForeColor = Color.White;
            this.btnPrediccion.Location = new Point(0, 140);
            this.btnPrediccion.Name = "btnPrediccion";
            this.btnPrediccion.Padding = new Padding(15, 0, 0, 0);
            this.btnPrediccion.Size = new Size(200, 40);
            this.btnPrediccion.TabIndex = 3;
            this.btnPrediccion.Text = "Predicción (F5)";
            this.btnPrediccion.TextAlign = ContentAlignment.MiddleLeft;

            // btnCorte
            this.btnCorte.FlatAppearance.BorderSize = 0;
            this.btnCorte.FlatStyle = FlatStyle.Flat;
            this.btnCorte.Font = new Font("Segoe UI", 10F);
            this.btnCorte.ForeColor = Color.White;
            this.btnCorte.Location = new Point(0, 180);
            this.btnCorte.Name = "btnCorte";
            this.btnCorte.Padding = new Padding(15, 0, 0, 0);
            this.btnCorte.Size = new Size(200, 40);
            this.btnCorte.TabIndex = 4;
            this.btnCorte.Text = "Corte de Caja";
            this.btnCorte.TextAlign = ContentAlignment.MiddleLeft;

            // ✅ btnFiados (nuevo)
            this.btnFiados.FlatAppearance.BorderSize = 0;
            this.btnFiados.FlatStyle = FlatStyle.Flat;
            this.btnFiados.Font = new Font("Segoe UI", 10F);
            this.btnFiados.ForeColor = Color.White;
            this.btnFiados.Location = new Point(0, 220);
            this.btnFiados.Name = "btnFiados";
            this.btnFiados.Padding = new Padding(15, 0, 0, 0);
            this.btnFiados.Size = new Size(200, 40);
            this.btnFiados.TabIndex = 5;
            this.btnFiados.Text = "Clientes Fiados";
            this.btnFiados.TextAlign = ContentAlignment.MiddleLeft;

            // ✅ btnProveedores (nuevo)
            this.btnProveedores.FlatAppearance.BorderSize = 0;
            this.btnProveedores.FlatStyle = FlatStyle.Flat;
            this.btnProveedores.Font = new Font("Segoe UI", 10F);
            this.btnProveedores.ForeColor = Color.White;
            this.btnProveedores.Location = new Point(0, 260);
            this.btnProveedores.Name = "btnProveedores";
            this.btnProveedores.Padding = new Padding(15, 0, 0, 0);
            this.btnProveedores.Size = new Size(200, 40);
            this.btnProveedores.TabIndex = 6;
            this.btnProveedores.Text = "Proveedores";
            this.btnProveedores.TextAlign = ContentAlignment.MiddleLeft;

            // btnInventario (Reportes)
            this.btnInventario.FlatAppearance.BorderSize = 0;
            this.btnInventario.FlatStyle = FlatStyle.Flat;
            this.btnInventario.Font = new Font("Segoe UI", 10F);
            this.btnInventario.ForeColor = Color.White;
            this.btnInventario.Location = new Point(0, 300);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Padding = new Padding(15, 0, 0, 0);
            this.btnInventario.Size = new Size(200, 40);
            this.btnInventario.TabIndex = 7;
            this.btnInventario.Text = "Reportes";
            this.btnInventario.TextAlign = ContentAlignment.MiddleLeft;

            // btnConfig
            this.btnConfig.FlatAppearance.BorderSize = 0;
            this.btnConfig.FlatStyle = FlatStyle.Flat;
            this.btnConfig.Font = new Font("Segoe UI", 10F);
            this.btnConfig.ForeColor = Color.White;
            this.btnConfig.Location = new Point(0, 340);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Padding = new Padding(15, 0, 0, 0);
            this.btnConfig.Size = new Size(200, 40);
            this.btnConfig.TabIndex = 8;
            this.btnConfig.Text = "Configuración";
            this.btnConfig.TextAlign = ContentAlignment.MiddleLeft;

            // btnSalir
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = FlatStyle.Flat;
            this.btnSalir.Font = new Font("Segoe UI", 10F);
            this.btnSalir.ForeColor = Color.FromArgb(248, 250, 252);
            this.btnSalir.Location = new Point(0, 550);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Padding = new Padding(15, 0, 0, 0);
            this.btnSalir.Size = new Size(200, 40);
            this.btnSalir.TabIndex = 9;
            this.btnSalir.Text = "Salir";
            this.btnSalir.TextAlign = ContentAlignment.MiddleLeft;

            // panelTop
            this.panelTop.BackColor = Color.FromArgb(248, 250, 252);
            this.panelTop.Controls.Add(this.lblUsuario);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(200, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(800, 50);
            this.panelTop.TabIndex = 1;

            // lblUsuario
            this.lblUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new Font("Segoe UI", 9F);
            this.lblUsuario.ForeColor = Color.FromArgb(30, 64, 175);
            this.lblUsuario.Location = new Point(600, 18);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new Size(120, 15);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario (rol)";

            // panelContenido
            this.panelContenido.BackColor = Color.White;
            this.panelContenido.Dock = DockStyle.Fill;
            this.panelContenido.Location = new Point(200, 50);
            this.panelContenido.Name = "panelContenido";
            this.panelContenido.Size = new Size(800, 550);
            this.panelContenido.TabIndex = 2;

            // agregar controles al side
            this.panelSide.Controls.Add(this.btnSalir);
            this.panelSide.Controls.Add(this.btnConfig);
            this.panelSide.Controls.Add(this.btnInventario);
            this.panelSide.Controls.Add(this.btnProveedores);
            this.panelSide.Controls.Add(this.btnFiados);
            this.panelSide.Controls.Add(this.btnCorte);
            this.panelSide.Controls.Add(this.btnPrediccion);
            this.panelSide.Controls.Add(this.btnProductos);
            this.panelSide.Controls.Add(this.btnVentas);
            this.panelSide.Controls.Add(this.lblTituloApp);

            // DashboardForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 600);
            this.Controls.Add(this.panelContenido);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelSide);
            this.MinimumSize = new Size(900, 550);
            this.Name = "DashboardForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MiniMarket - Dashboard";

            this.panelSide.ResumeLayout(false);
            this.panelSide.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
