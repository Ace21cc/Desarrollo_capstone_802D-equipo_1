using System;
using System.Drawing;
using System.Windows.Forms;
using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public partial class ProveedoresForm : Form
    {
        private readonly ProveedorService _svc = new ProveedorService();

        private DataGridView dgvProv;
        private DataGridView dgvMovs;
        private TextBox txtNombre, txtTelefono, txtContacto;
        private Button btnNuevo, btnModificar;
        private TextBox txtMonto, txtDescripcion;
        private Button btnCargo, btnAbono;

        public ProveedoresForm()
        {
            InitializeComponent();
            CargarProveedores();
        }

        private void InitializeComponent()
        {
            this.Text = "Proveedores";
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 380
            };

            // -------------------- PANEL IZQUIERDO --------------------
            var left = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            dgvProv = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 240,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false
            };

            dgvProv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 40
            });
            dgvProv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 160
            });
            dgvProv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                Width = 90
            });
            dgvProv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Saldo",
                DataPropertyName = "SaldoActual",
                Width = 80
            });

            dgvProv.SelectionChanged += (s, e) => SeleccionarProveedor();

            var form = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Top = 245
            };

            txtNombre = new TextBox
            {
                Left = 0,
                Top = 5,
                Width = 350
            };
            txtNombre.PlaceholderText = "Nombre";

            txtTelefono = new TextBox
            {
                Left = 0,
                Top = 40,
                Width = 350
            };
            txtTelefono.PlaceholderText = "Teléfono";

            txtContacto = new TextBox
            {
                Left = 0,
                Top = 75,
                Width = 350
            };
            txtContacto.PlaceholderText = "Contacto";

            form.Controls.AddRange(new Control[] { txtNombre, txtTelefono, txtContacto });

            btnNuevo = new Button
            {
                Text = "Agregar",
                Left = 0,
                Top = 370,
                Width = 160
            };
            btnModificar = new Button
            {
                Text = "Modificar",
                Left = 170,
                Top = 370,
                Width = 160
            };

            btnNuevo.Click += (s, e) => AgregarProveedor();
            btnModificar.Click += (s, e) => ModificarProveedor();

            left.Controls.Add(btnModificar);
            left.Controls.Add(btnNuevo);
            left.Controls.Add(form);
            left.Controls.Add(dgvProv);

            // -------------------- PANEL DERECHO --------------------
            var right = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            dgvMovs = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 260,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoGenerateColumns = false
            };

            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha",
                DataPropertyName = "Fecha",
                Width = 140
            });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tipo",
                DataPropertyName = "Tipo",
                Width = 70
            });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Monto",
                DataPropertyName = "Monto",
                Width = 90
            });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Desc.",
                DataPropertyName = "Descripcion",
                Width = 170
            });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Saldo",
                DataPropertyName = "SaldoResultante",
                Width = 90
            });

            txtMonto = new TextBox
            {
                Left = 0,
                Top = 280,
                Width = 120
            };
            txtMonto.PlaceholderText = "Monto";

            txtDescripcion = new TextBox
            {
                Left = 130,
                Top = 280,
                Width = 300
            };
            txtDescripcion.PlaceholderText = "Descripción";

            btnCargo = new Button
            {
                Text = "Registrar Cargo",
                Left = 0,
                Top = 320,
                Width = 200
            };
            btnAbono = new Button
            {
                Text = "Registrar Abono",
                Left = 210,
                Top = 320,
                Width = 200
            };

            btnCargo.Click += (s, e) => RegistrarCargo();
            btnAbono.Click += (s, e) => RegistrarAbono();

            right.Controls.Add(btnAbono);
            right.Controls.Add(btnCargo);
            right.Controls.Add(txtDescripcion);
            right.Controls.Add(txtMonto);
            right.Controls.Add(dgvMovs);

            // SPLIT
            split.Panel1.Controls.Add(left);
            split.Panel2.Controls.Add(right);
            this.Controls.Add(split);
        }

        // Proveedor seleccionado en la grilla
        private Proveedor ProveedorSeleccionado
        {
            get
            {
                return dgvProv.CurrentRow != null
                    ? dgvProv.CurrentRow.DataBoundItem as Proveedor
                    : null;
            }
        }

        private void CargarProveedores()
        {
            dgvProv.DataSource = _svc.GetProveedores();
        }

        private void SeleccionarProveedor()
        {
            var p = ProveedorSeleccionado;
            if (p == null) return;

            txtNombre.Text = p.Nombre ?? string.Empty;
            txtTelefono.Text = p.Telefono ?? string.Empty;
            txtContacto.Text = p.Contacto ?? string.Empty;

            // Obtenemos movimientos directamente (el modelo ya tiene las propiedades adecuadas)
            var movs = _svc.GetMovimientos(p.Id);
            dgvMovs.DataSource = movs;
        }

        private void AgregarProveedor()
        {
            var nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Nombre requerido.");
                return;
            }

            var p = new Proveedor
            {
                Nombre = nombre,
                Telefono = txtTelefono.Text.Trim(),
                Contacto = txtContacto.Text.Trim()
            };

            try
            {
                _svc.AddProveedor(p);
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar proveedor: " + ex.Message);
            }
        }

        private void ModificarProveedor()
        {
            var p = ProveedorSeleccionado;
            if (p == null) return;

            p.Nombre = txtNombre.Text.Trim();
            p.Telefono = txtTelefono.Text.Trim();
            p.Contacto = txtContacto.Text.Trim();

            try
            {
                _svc.UpdateProveedor(p);
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar proveedor: " + ex.Message);
            }
        }

        private void RegistrarCargo()
        {
            var p = ProveedorSeleccionado;
            if (p == null) return;

            if (!double.TryParse(txtMonto.Text, out var monto) || monto <= 0)
            {
                MessageBox.Show("Monto inválido.");
                return;
            }

            try
            {
                _svc.RegistrarCargo(p.Id, monto, txtDescripcion.Text.Trim());
                SeleccionarProveedor();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar cargo: " + ex.Message);
            }
        }

        private void RegistrarAbono()
        {
            var p = ProveedorSeleccionado;
            if (p == null) return;

            if (!double.TryParse(txtMonto.Text, out var monto) || monto <= 0)
            {
                MessageBox.Show("Monto inválido.");
                return;
            }

            try
            {
                _svc.RegistrarAbono(p.Id, monto, txtDescripcion.Text.Trim());
                SeleccionarProveedor();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar abono: " + ex.Message);
            }
        }
    }
}
