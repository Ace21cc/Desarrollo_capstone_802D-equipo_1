using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MiniMarket.Models;
using MiniMarket.Services;

namespace MiniMarket
{
    public class AlertasFiadosForm : Form
    {
        private readonly ClienteFiadoService _service = new ClienteFiadoService();
        private DataGridView dgv;
        private Button btnRefrescar;

        public AlertasFiadosForm()
        {
            InitializeComponent();
            Cargar();
        }

        private void InitializeComponent()
        {
            this.Text = "Alertas de Fiados Vencidos";
            this.Size = new Size(700, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoGenerateColumns = false
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cliente",
                DataPropertyName = "Nombre",
                Width = 200
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                Width = 120
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Dirección",
                DataPropertyName = "Direccion",
                Width = 200
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Saldo",
                DataPropertyName = "SaldoActual",
                Width = 120
            });

            btnRefrescar = new Button
            {
                Text = "Refrescar",
                Dock = DockStyle.Top,
                Height = 35
            };
            btnRefrescar.Click += (s, e) => Cargar();

            this.Controls.Add(dgv);
            this.Controls.Add(btnRefrescar);
        }

        private void Cargar()
        {
            List<ClienteFiado> fiados = _service.GetFiadosVencidos(30, 1);
            dgv.DataSource = fiados;
        }
    }
}
