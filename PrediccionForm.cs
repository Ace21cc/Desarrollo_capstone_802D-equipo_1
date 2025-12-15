using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public class PrediccionForm : Form
    {
        private Label lblTitulo;
        private Label lblSub;
        private NumericUpDown nudDias;
        private Button btnActualizar;
        private Button btnGuardar;
        private DataGridView dgv;
        private Panel topPanel;

        private readonly PrediccionService _service = new PrediccionService();

        // Lista enlazada a la grilla
        private BindingList<PrediccionResultado> _bindingList = new BindingList<PrediccionResultado>();
        private readonly BindingSource _source = new BindingSource();

        public PrediccionForm()
        {
            InitializeComponent();
            CargarPrediccion();
        }

        private void InitializeComponent()
        {
            this.Text = "Predicción de Ventas";
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // Panel superior
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = Color.FromArgb(240, 248, 255)
            };

            lblTitulo = new Label
            {
                Text = "Predicción de Ventas y Productos de Alta Rotación",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 61, 110),
                AutoSize = true,
                Left = 15,
                Top = 10
            };

            lblSub = new Label
            {
                Text = "Se analizan ventas históricas por producto para estimar el promedio diario y proyectar 7 días.",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Left = 18,
                Top = 45
            };

            var lblDias = new Label
            {
                Text = "Últimos días:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Left = 18,
                Top = 75
            };

            nudDias = new NumericUpDown
            {
                Minimum = 7,
                Maximum = 365,
                Value = 30,
                Left = 95,
                Top = 72,
                Width = 70
            };

            btnActualizar = new Button
            {
                Text = "Actualizar",
                Left = 175,
                Top = 70,
                Width = 110,
                Height = 28,
                BackColor = Color.FromArgb(0, 92, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += (s, e) => CargarPrediccion();

            btnGuardar = new Button
            {
                Text = "Guardar predicción",
                Left = 295,
                Top = 70,
                Width = 150,
                Height = 28,
                BackColor = Color.FromArgb(0, 140, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            topPanel.Controls.Add(lblTitulo);
            topPanel.Controls.Add(lblSub);
            topPanel.Controls.Add(lblDias);
            topPanel.Controls.Add(nudDias);
            topPanel.Controls.Add(btnActualizar);
            topPanel.Controls.Add(btnGuardar);

            // Grilla
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Las DataPropertyName deben coincidir con las propiedades de PrediccionResultado
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Producto",
                DataPropertyName = "Producto",
                Width = 220
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "StockActual",
                DataPropertyName = "StockActual",
                Width = 90
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "VendidoÚltimosDías",
                DataPropertyName = "VendidoUltimosDias",
                Width = 120
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PromedioDiario",
                DataPropertyName = "PromedioDiario",
                Width = 90
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Proyección7Días",
                DataPropertyName = "Proyeccion7Dias",
                Width = 100
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "IngresoTotal",
                DataPropertyName = "IngresoTotal",
                Width = 100
            });

            _source.DataSource = _bindingList;
            dgv.DataSource = _source;

            this.Controls.Add(dgv);
            this.Controls.Add(topPanel);
        }

        private void CargarPrediccion()
        {
            try
            {
                int dias = (int)nudDias.Value;

                // Debe existir en PrediccionService:
                // List<PrediccionResultado> CalcularTopRotacion(int dias)
                var datos = _service.CalcularTopRotacion(dias);

                _bindingList = new BindingList<PrediccionResultado>(datos);
                _source.DataSource = _bindingList;
                dgv.DataSource = _source;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar predicción: " + ex.Message);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int dias = (int)nudDias.Value;

                // Debe existir en PrediccionService:
                // void GuardarPrediccion(int dias, List<PrediccionResultado> lista)
                var datos = _bindingList.ToList();
                _service.GuardarPrediccion(dias, datos);

                MessageBox.Show("Predicción guardada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar predicción: " + ex.Message);
            }
        }
    }
}
