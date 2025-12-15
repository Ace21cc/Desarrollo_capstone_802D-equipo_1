using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public partial class ProductosForm : Form
    {
        private DataGridView dgvProductos;
        private TextBox txtNombre, txtCodigo, txtPrecio, txtStock, txtBuscar;
        private ComboBox cmbCategoria;
        private Button btnNuevo, btnGuardar, btnEliminar, btnBuscar;

        private int? _editId = null;
        private readonly DataTable _tabla = new DataTable();

        public ProductosForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Productos";
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            var lblTitulo = new Label
            {
                Text = "Gestión de Productos",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 61, 110),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            // Buscador
            var lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(20, 55),
                AutoSize = true
            };
            txtBuscar = new TextBox
            {
                Location = new Point(75, 50),
                Width = 200
            };
            btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(285, 48),
                Size = new Size(70, 26)
            };
            btnBuscar.Click += (s, e) => CargarProductos(txtBuscar.Text.Trim());

            // Grilla
            dgvProductos = new DataGridView
            {
                Location = new Point(20, 85),
                Size = new Size(600, 260),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgvProductos.CellClick += (s, e) => CargarSeleccion();

            // Formulario lateral
            var lblNombre = new Label { Text = "Nombre:", Location = new Point(640, 85), AutoSize = true };
            txtNombre = new TextBox { Location = new Point(710, 80), Width = 200 };

            var lblCodigo = new Label { Text = "Código (barra):", Location = new Point(640, 115), AutoSize = true };
            txtCodigo = new TextBox { Location = new Point(740, 110), Width = 170 };

            var lblCategoria = new Label { Text = "Categoría:", Location = new Point(640, 145), AutoSize = true };
            cmbCategoria = new ComboBox
            {
                Location = new Point(710, 140),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategoria.Items.AddRange(new object[]
            {
                "General",
                "Abarrotes",
                "Bebidas",
                "Lácteos",
                "Panadería",
                "Confites",
                "Limpieza",
                "Congelados",
                "Otros"
            });
            cmbCategoria.SelectedIndex = 0;

            var lblPrecio = new Label { Text = "Precio:", Location = new Point(640, 175), AutoSize = true };
            txtPrecio = new TextBox { Location = new Point(710, 170), Width = 80 };

            var lblStock = new Label { Text = "Stock:", Location = new Point(640, 205), AutoSize = true };
            txtStock = new TextBox { Location = new Point(710, 200), Width = 80 };

            btnNuevo = new Button
            {
                Text = "Nuevo",
                Location = new Point(640, 245),
                Size = new Size(80, 30)
            };
            btnNuevo.Click += (s, e) => LimpiarFormulario();

            btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new Point(730, 245),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) => GuardarProducto();

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new Point(830, 245),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(248, 113, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) => EliminarProducto();

            this.Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblBuscar, txtBuscar, btnBuscar,
                dgvProductos,
                lblNombre, txtNombre,
                lblCodigo, txtCodigo,
                lblCategoria, cmbCategoria,
                lblPrecio, txtPrecio,
                lblStock, txtStock,
                btnNuevo, btnGuardar, btnEliminar
            });

            this.Load += (s, e) => CargarProductos();
        }

        // Carga de productos desde MySQL
        private void CargarProductos(string filtro = "")
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT id, nombre, codigo, categoria, precio, stock
                    FROM productos";

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    cmd.CommandText +=
                        " WHERE nombre LIKE @f OR codigo LIKE @f OR categoria LIKE @f";
                    cmd.Parameters.AddWithValue("@f", "%" + filtro + "%");
                }

                using var da = new MySqlDataAdapter(cmd);
                var tabla = new DataTable();
                da.Fill(tabla);

                dgvProductos.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarSeleccion()
        {
            if (dgvProductos.CurrentRow == null || dgvProductos.CurrentRow.Index < 0)
                return;

            var row = dgvProductos.CurrentRow;
            _editId = Convert.ToInt32(row.Cells["id"].Value);
            txtNombre.Text = row.Cells["nombre"].Value?.ToString();
            txtCodigo.Text = row.Cells["codigo"].Value?.ToString();
            txtPrecio.Text = row.Cells["precio"].Value?.ToString();
            txtStock.Text = row.Cells["stock"].Value?.ToString();

            string categoria = row.Cells["categoria"].Value?.ToString() ?? "General";
            if (!cmbCategoria.Items.Contains(categoria))
                cmbCategoria.Items.Add(categoria);
            cmbCategoria.SelectedItem = categoria;
        }

        private void LimpiarFormulario()
        {
            _editId = null;
            txtNombre.Clear();
            txtCodigo.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            if (cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedIndex = 0;
            txtNombre.Focus();
        }

        private void GuardarProducto()
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            if (!double.TryParse(txtPrecio.Text.Replace(",", "."), out var precio) || precio < 0)
            {
                MessageBox.Show("Precio inválido.");
                return;
            }

            if (!int.TryParse(txtStock.Text, out var stock) || stock < 0)
            {
                MessageBox.Show("Stock inválido.");
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string? codigo = string.IsNullOrWhiteSpace(txtCodigo.Text) ? null : txtCodigo.Text.Trim();
            string categoria = cmbCategoria.SelectedItem?.ToString() ?? "General";

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string sql;
                if (_editId == null)
                {
                    sql = @"
                        INSERT INTO productos (nombre, codigo, categoria, precio, stock)
                        VALUES (@n, @c, @cat, @p, @s);";
                }
                else
                {
                    sql = @"
                        UPDATE productos
                        SET nombre = @n,
                            codigo = @c,
                            categoria = @cat,
                            precio = @p,
                            stock = @s
                        WHERE id = @id;";
                }

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@c", (object?)codigo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cat", categoria);
                cmd.Parameters.AddWithValue("@p", precio);
                cmd.Parameters.AddWithValue("@s", stock);

                if (_editId != null)
                    cmd.Parameters.AddWithValue("@id", _editId.Value);

                cmd.ExecuteNonQuery();

                CargarProductos();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar producto: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarProducto()
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvProductos.CurrentRow.Cells["id"].Value);

            if (MessageBox.Show("¿Eliminar el producto seleccionado?",
                                "Confirmar",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(
                    "DELETE FROM productos WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                CargarProductos();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar producto: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
