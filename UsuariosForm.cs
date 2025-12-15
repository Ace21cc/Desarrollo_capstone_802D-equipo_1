using System;
using System.Collections.Generic;
using System.Drawing;     // Color, Font, FontStyle
using System.Linq;        // Linq para .First, .Select
using System.Windows.Forms;

using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public partial class UsuariosForm : Form
    {
        private DataGridView dgv;
        private TextBox txtUsuario, txtPassword;
        private ComboBox cbRol;
        private Button btnNuevo, btnGuardar, btnEliminar, btnRefrescar;

        private readonly UserService _svc = new UserService();
        private List<UsuarioRow> _lista = new();
        private int? _editId = null;

        public UsuariosForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Usuarios";
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // GRID
            dgv = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(680, 420),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            // Usuario
            var lblU = new Label
            {
                Text = "Usuario:",
                Location = new Point(720, 40),
                AutoSize = true
            };
            txtUsuario = new TextBox
            {
                Location = new Point(780, 36),
                Width = 220
            };

            // Clave
            var lblC = new Label
            {
                Text = "Clave:",
                Location = new Point(720, 80),
                AutoSize = true
            };
            txtPassword = new TextBox
            {
                Location = new Point(780, 76),
                Width = 220,
                UseSystemPasswordChar = true
            };

            // Rol
            var lblR = new Label
            {
                Text = "Rol:",
                Location = new Point(720, 120),
                AutoSize = true
            };
            cbRol = new ComboBox
            {
                Location = new Point(780, 116),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbRol.Items.AddRange(new object[] { "admin", "trabajador" });
            cbRol.SelectedIndex = 1; // "trabajador" por defecto

            // Botones
            btnNuevo = new Button
            {
                Text = "Nuevo",
                Location = new Point(720, 170),
                Size = new Size(80, 30)
            };
            btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new Point(810, 170),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new Point(910, 170),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(248, 113, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.FlatAppearance.BorderSize = 0;

            btnRefrescar = new Button
            {
                Text = "Refrescar",
                Location = new Point(1010, 170),
                Size = new Size(90, 30)
            };

            // Eventos
            btnNuevo.Click += (s, e) => Limpiar();
            btnGuardar.Click += (s, e) => Guardar();
            btnEliminar.Click += (s, e) => Eliminar();
            btnRefrescar.Click += (s, e) => Cargar();
            dgv.CellClick += (s, e) => CargarSeleccion();

            // Agregar controles
            this.Controls.AddRange(new Control[]
            {
                dgv,
                lblU, txtUsuario,
                lblC, txtPassword,
                lblR, cbRol,
                btnNuevo, btnGuardar, btnEliminar, btnRefrescar
            });

            // Load
            this.Load += (s, e) => Cargar();
        }

        private void Cargar()
        {
            _lista = _svc.GetAll();
            // No mostramos la clave en la grilla
            dgv.DataSource = _lista
                .Select(u => new { u.Id, u.Usuario, u.Rol })
                .ToList();

            Limpiar();
        }

        private void CargarSeleccion()
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Index < 0)
                return;

            var idObj = dgv.CurrentRow.Cells["Id"].Value;
            if (idObj == null)
                return;

            int id = Convert.ToInt32(idObj);
            var u = _lista.FirstOrDefault(x => x.Id == id);
            if (u == null)
                return;

            _editId = u.Id;
            txtUsuario.Text = u.Usuario;
            txtPassword.Text = u.Password;
            cbRol.SelectedItem = u.Rol;
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Usuario requerido");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Clave requerida");
                return;
            }
            if (cbRol.SelectedItem == null)
            {
                MessageBox.Show("Seleccione rol");
                return;
            }

            var u = new UsuarioRow
            {
                Id = _editId ?? 0,
                Usuario = txtUsuario.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Rol = cbRol.SelectedItem?.ToString() ?? "trabajador"
            };

            try
            {
                if (_editId == null)
                    _svc.Add(u);
                else
                    _svc.Update(u);

                Cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void Eliminar()
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Index < 0)
                return;

            var idObj = dgv.CurrentRow.Cells["Id"].Value;
            if (idObj == null)
                return;

            int id = Convert.ToInt32(idObj);

            if (MessageBox.Show("¿Eliminar usuario?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                try
                {
                    _svc.Delete(id);
                    Cargar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo eliminar: " + ex.Message);
                }
            }
        }

        private void Limpiar()
        {
            _editId = null;
            txtUsuario.Clear();
            txtPassword.Clear();
            if (cbRol.Items.Count > 0)
                cbRol.SelectedIndex = 1; // "trabajador"
            txtUsuario.Focus();
        }
    }
}
