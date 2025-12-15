using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public class ClientesFiadosForm : Form
    {
        private ClienteFiadoService _svc = new ClienteFiadoService();

        private DataGridView dgvClientes;
        private DataGridView dgvMovs;
        private TextBox txtNombre, txtTelefono, txtDireccion;
        private Button btnNuevo, btnModificar, btnEliminar;
        private TextBox txtMonto, txtDescripcion;
        private Button btnCargo, btnAbono;

        public ClientesFiadosForm()
        {
            InitializeComponent();
            CargarClientes();
        }

        private void InitializeComponent()
        {
            this.Text = "Clientes Fiados";
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, SplitterDistance = 380 };

            // IZQ
            var left = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            dgvClientes = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 240,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false
            };
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "Id", Width = 40 });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 150 });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Teléfono", DataPropertyName = "Telefono", Width = 90 });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Saldo", DataPropertyName = "SaldoActual", Width = 80 });
            dgvClientes.SelectionChanged += (s, e) => SeleccionarCliente();

            var form = new Panel { Dock = DockStyle.Top, Height = 120, Top = 245 };
            txtNombre = new TextBox { PlaceholderText = "Nombre", Left = 0, Width = 350, Top = 5 };
            txtTelefono = new TextBox { PlaceholderText = "Teléfono", Left = 0, Width = 350, Top = 40 };
            txtDireccion = new TextBox { PlaceholderText = "Dirección", Left = 0, Width = 350, Top = 75 };
            form.Controls.AddRange(new Control[] { txtNombre, txtTelefono, txtDireccion });

            btnNuevo = new Button { Text = "Agregar", Left = 0, Top = 370, Width = 110 };
            btnModificar = new Button { Text = "Modificar", Left = 120, Top = 370, Width = 110 };
            btnEliminar = new Button { Text = "Eliminar", Left = 240, Top = 370, Width = 110 };

            btnNuevo.Click += (s, e) => AgregarCliente();
            btnModificar.Click += (s, e) => ModificarCliente();
            btnEliminar.Click += (s, e) => EliminarCliente();

            left.Controls.Add(btnEliminar);
            left.Controls.Add(btnModificar);
            left.Controls.Add(btnNuevo);
            left.Controls.Add(form);
            left.Controls.Add(dgvClientes);

            // DER
            var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            dgvMovs = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 260,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoGenerateColumns = false
            };
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = "fecha", Width = 140 });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tipo", DataPropertyName = "tipo", Width = 70 });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Monto", DataPropertyName = "monto", Width = 90 });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Desc.", DataPropertyName = "descripcion", Width = 170 });
            dgvMovs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Saldo", DataPropertyName = "saldoResultante", Width = 90 });

            txtMonto = new TextBox { PlaceholderText = "Monto", Left = 0, Top = 280, Width = 120 };
            txtDescripcion = new TextBox { PlaceholderText = "Descripción", Left = 130, Top = 280, Width = 300 };

            btnCargo = new Button { Text = "Registrar Cargo", Left = 0, Top = 320, Width = 200 };
            btnAbono = new Button { Text = "Registrar Abono", Left = 210, Top = 320, Width = 200 };
            btnCargo.Click += (s, e) => RegistrarCargo();
            btnAbono.Click += (s, e) => RegistrarAbono();

            right.Controls.Add(btnAbono);
            right.Controls.Add(btnCargo);
            right.Controls.Add(txtDescripcion);
            right.Controls.Add(txtMonto);
            right.Controls.Add(dgvMovs);

            split.Panel1.Controls.Add(left);
            split.Panel2.Controls.Add(right);

            this.Controls.Add(split);
        }

        private ClienteFiado? ClienteSeleccionado =>
            dgvClientes.CurrentRow?.DataBoundItem as ClienteFiado;

        private void CargarClientes()
        {
            dgvClientes.DataSource = _svc.GetClientes();
        }

        private void SeleccionarCliente()
        {
            var c = ClienteSeleccionado;
            if (c == null) return;

            txtNombre.Text = c.Nombre;
            txtTelefono.Text = c.Telefono;
            txtDireccion.Text = c.Direccion;

            var movs = _svc.GetMovimientos(c.Id)
                .Select(x => new { x.fecha, x.tipo, x.monto, x.descripcion, x.saldoResultante })
                .ToList();

            dgvMovs.DataSource = movs;
        }

        private void AgregarCliente()
        {
            var c = new ClienteFiado
            {
                Nombre = txtNombre.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Direccion = txtDireccion.Text.Trim()
            };

            if (string.IsNullOrWhiteSpace(c.Nombre))
            {
                MessageBox.Show("Nombre requerido.");
                return;
            }

            _svc.AddCliente(c);
            CargarClientes();
        }

        private void ModificarCliente()
        {
            var c = ClienteSeleccionado;
            if (c == null) return;

            c.Nombre = txtNombre.Text.Trim();
            c.Telefono = txtTelefono.Text.Trim();
            c.Direccion = txtDireccion.Text.Trim();

            _svc.UpdateCliente(c);
            CargarClientes();
        }

        private void EliminarCliente()
        {
            var c = ClienteSeleccionado;
            if (c == null) return;

            MessageBox.Show("Eliminación no implementada por seguridad.");
        }

        private void RegistrarCargo()
        {
            var c = ClienteSeleccionado;
            if (c == null) return;

            if (!double.TryParse(txtMonto.Text, out var monto) || monto <= 0)
            {
                MessageBox.Show("Monto inválido.");
                return;
            }

            _svc.RegistrarCargo(c.Id, monto, txtDescripcion.Text.Trim());
            SeleccionarCliente();
            CargarClientes();
        }

        private void RegistrarAbono()
        {
            var c = ClienteSeleccionado;
            if (c == null) return;

            if (!double.TryParse(txtMonto.Text, out var monto) || monto <= 0)
            {
                MessageBox.Show("Monto inválido.");
                return;
            }

            _svc.RegistrarAbono(c.Id, monto, txtDescripcion.Text.Trim());
            SeleccionarCliente();
            CargarClientes();
        }
    }
}
