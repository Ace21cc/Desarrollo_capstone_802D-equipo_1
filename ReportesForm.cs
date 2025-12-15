using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public class ReportesForm : Form
    {
        private readonly ReportesService _service = new ReportesService();

        private DateTimePicker dtDesde, dtHasta;
        private Button btnBuscar, btnGuardar, btnExportarPdf, btnExportarExcel;
        private DataGridView dgv;
        private Label lblTotales;

        private List<ReporteVenta> _ultimos = new();

        public ReportesForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Reportes";
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            var top = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = Color.FromArgb(245, 245, 255)
            };

            var lblTitle = new Label
            {
                Text = "Reportes de Ventas",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 61, 110),
                AutoSize = true,
                Left = 15,
                Top = 8
            };

            var lblDesde = new Label { Text = "Desde:", Left = 20, Top = 50, AutoSize = true };
            dtDesde = new DateTimePicker { Left = 70, Top = 45, Width = 140 };

            var lblHasta = new Label { Text = "Hasta:", Left = 230, Top = 50, AutoSize = true };
            dtHasta = new DateTimePicker { Left = 280, Top = 45, Width = 140 };

            btnBuscar = new Button
            {
                Text = "Buscar",
                Left = 440,
                Top = 43,
                Width = 110,
                Height = 30,
                BackColor = Color.FromArgb(0, 92, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += BtnBuscar_Click;

            btnGuardar = new Button
            {
                Text = "Guardar Reporte",
                Left = 560,
                Top = 43,
                Width = 150,
                Height = 30,
                BackColor = Color.FromArgb(0, 140, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnExportarPdf = new Button
            {
                Text = "Exportar PDF",
                Left = 720,
                Top = 43,
                Width = 120,
                Height = 30,
                BackColor = Color.FromArgb(120, 120, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportarPdf.FlatAppearance.BorderSize = 0;
            btnExportarPdf.Click += BtnExportarPdf_Click;

            btnExportarExcel = new Button
            {
                Text = "Exportar Excel",
                Left = 845,
                Top = 43,
                Width = 130,
                Height = 30,
                BackColor = Color.FromArgb(16, 122, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportarExcel.FlatAppearance.BorderSize = 0;
            btnExportarExcel.Click += BtnExportarExcel_Click;

            top.Controls.AddRange(new Control[]
            {
                lblTitle, lblDesde, dtDesde, lblHasta, dtHasta,
                btnBuscar, btnGuardar, btnExportarPdf, btnExportarExcel
            });

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha",
                DataPropertyName = "Fecha",
                Width = 200
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total",
                DataPropertyName = "Total",
                Width = 120
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Método Pago",
                DataPropertyName = "MetodoPago",
                Width = 160
            });

            lblTotales = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(10),
                Text = "Totales: 0"
            };

            this.Controls.Add(dgv);
            this.Controls.Add(lblTotales);
            this.Controls.Add(top);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var datos = _service.VentasEntre(
                    dtDesde.Value.ToString("yyyy-MM-dd"),
                    dtHasta.Value.ToString("yyyy-MM-dd")
                ).ToList();

                _ultimos = datos;
                dgv.DataSource = datos;

                lblTotales.Text = $"Ventas: {datos.Count} | Total: {datos.Sum(x => x.Total):N0}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando reporte: " + ex.Message);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_ultimos.Count == 0)
            {
                MessageBox.Show("No hay datos para guardar.");
                return;
            }

            try
            {
                var sb = new StringBuilder();
                foreach (var v in _ultimos)
                    sb.AppendLine($"{v.Fecha} | {v.MetodoPago} | {v.Total}");

                _service.GuardarReporte(
                    "Ventas",
                    $"{{\"desde\":\"{dtDesde.Value:yyyy-MM-dd}\",\"hasta\":\"{dtHasta.Value:yyyy-MM-dd}\"}}",
                    sb.ToString()
                );

                MessageBox.Show("Reporte guardado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando reporte: " + ex.Message);
            }
        }

        // ✅ PDF SIMPLE (sin iText)
        private void BtnExportarPdf_Click(object sender, EventArgs e)
        {
            if (_ultimos.Count == 0)
            {
                MessageBox.Show("Primero busca un reporte.");
                return;
            }

            using var sfd = new SaveFileDialog();
            sfd.Filter = "PDF (*.pdf)|*.pdf";
            sfd.FileName = $"ReporteVentas_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using var sw = new System.IO.StreamWriter(sfd.FileName, false, Encoding.UTF8);

                sw.WriteLine("REPORTE DE VENTAS");
                sw.WriteLine("=================");
                sw.WriteLine($"Desde: {dtDesde.Value:yyyy-MM-dd}");
                sw.WriteLine($"Hasta: {dtHasta.Value:yyyy-MM-dd}");
                sw.WriteLine("");
                sw.WriteLine("Fecha | Método Pago | Total");
                sw.WriteLine("--------------------------------");

                foreach (var v in _ultimos)
                {
                    sw.WriteLine($"{v.Fecha} | {v.MetodoPago} | {v.Total}");
                }

                sw.WriteLine("--------------------------------");
                sw.WriteLine($"TOTAL GENERAL: {_ultimos.Sum(x => x.Total):N0}");

                MessageBox.Show("PDF exportado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exportando PDF: " + ex.Message);
            }
        }

        // ✅ EXCEL SIN LIBRERÍAS (CSV que Excel abre)
        private void BtnExportarExcel_Click(object sender, EventArgs e)
        {
            if (_ultimos.Count == 0)
            {
                MessageBox.Show("Primero busca un reporte.");
                return;
            }

            using var sfd = new SaveFileDialog();
            sfd.Filter = "Excel (CSV) (*.csv)|*.csv";
            sfd.FileName = $"ReporteVentas_{DateTime.Now:yyyyMMdd_HHmm}.csv";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                // CSV con separador ; (Excel en ES suele usar ;)
                var sb = new StringBuilder();
                sb.AppendLine("Fecha;MetodoPago;Total");

                foreach (var v in _ultimos)
                {
                    var fecha = (v.Fecha ?? "").Replace(";", " ");
                    var metodo = (v.MetodoPago ?? "").Replace(";", " ");
                    var total = v.Total.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    sb.AppendLine($"{fecha};{metodo};{total}");
                }

                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);

                MessageBox.Show("Excel (CSV) exportado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exportando Excel: " + ex.Message);
            }
        }
    }
}
