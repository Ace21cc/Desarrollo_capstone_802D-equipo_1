using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public class ReportesGuardadosForm : Form
    {
        private DataGridView dgvReportes;
        private Button btnRefrescar;
        private Button btnVer;
        private Button btnExportarPdf;

        private readonly ReportesGuardadosService _svc = new ReportesGuardadosService();

        public ReportesGuardadosForm()
        {
            InitializeComponent();
            Cargar();
        }

        private void InitializeComponent()
        {
            this.Text = "Reportes Guardados";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 500);
            this.BackColor = Color.White;

            dgvReportes = new DataGridView
            {
                Left = 20,
                Top = 20,
                Width = 840,
                Height = 360,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = false
            };

            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 60
            });

            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 260
            });

            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha Guardado",
                DataPropertyName = "FechaGuardado",
                Width = 180
            });

            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Filtro",
                DataPropertyName = "Filtro",
                Width = 320
            });

            btnRefrescar = new Button
            {
                Text = "Refrescar",
                Left = 20,
                Top = 400,
                Width = 130,
                Height = 35
            };
            btnRefrescar.Click += (s, e) => Cargar();

            btnVer = new Button
            {
                Text = "Ver Contenido",
                Left = 160,
                Top = 400,
                Width = 150,
                Height = 35
            };
            btnVer.Click += BtnVer_Click;

            btnExportarPdf = new Button
            {
                Text = "Exportar PDF",
                Left = 320,
                Top = 400,
                Width = 150,
                Height = 35
            };
            btnExportarPdf.Click += BtnExportarPdf_Click;

            this.Controls.Add(dgvReportes);
            this.Controls.Add(btnRefrescar);
            this.Controls.Add(btnVer);
            this.Controls.Add(btnExportarPdf);
        }

        private void Cargar()
        {
            try
            {
                List<ReporteGuardado> lista = _svc.Listar();
                dgvReportes.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando reportes guardados: " + ex.Message);
            }
        }

        private ReporteGuardado? Seleccionado()
        {
            if (dgvReportes.CurrentRow == null) return null;
            return dgvReportes.CurrentRow.DataBoundItem as ReporteGuardado;
        }

        private void BtnVer_Click(object sender, EventArgs e)
        {
            var rep = Seleccionado();
            if (rep == null)
            {
                MessageBox.Show("Selecciona un reporte.");
                return;
            }

            MessageBox.Show(rep.Contenido ?? "", rep.Nombre, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportarPdf_Click(object sender, EventArgs e)
        {
            var rep = Seleccionado();
            if (rep == null)
            {
                MessageBox.Show("Selecciona un reporte.");
                return;
            }

            using var sfd = new SaveFileDialog();
            sfd.Filter = "PDF (*.pdf)|*.pdf";
            sfd.FileName = $"{rep.Nombre}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var pdf = new ExportPdfService();
                pdf.ExportarReporteGuardado(sfd.FileName, rep);

                MessageBox.Show("PDF exportado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exportando PDF: " + ex.Message);
            }
        }
    }
}
