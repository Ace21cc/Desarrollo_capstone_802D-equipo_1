using System.Drawing;
using System.Windows.Forms;

namespace MiniMarket
{
    partial class VentasForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtCodigo;
        private DataGridView dgvCarrito;
        private Label lblTotal;
        private Label lblTotalTitulo;
        private Button btnEliminar;
        private Button btnFinalizar;

        private ComboBox cboMetodoPago;
        private ComboBox cboClienteFiado;
        private Button btnPagarProveedor;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtCodigo = new TextBox();
            this.dgvCarrito = new DataGridView();
            this.lblTotal = new Label();
            this.lblTotalTitulo = new Label();
            this.btnEliminar = new Button();
            this.btnFinalizar = new Button();
            this.cboMetodoPago = new ComboBox();
            this.cboClienteFiado = new ComboBox();
            this.btnPagarProveedor = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();
            this.SuspendLayout();

            // txtCodigo
            this.txtCodigo.Font = new Font("Segoe UI", 12F);
            this.txtCodigo.Location = new Point(20, 20);
            this.txtCodigo.Size = new Size(250, 29);

            // dgvCarrito
            this.dgvCarrito.Location = new Point(20, 70);
            this.dgvCarrito.Size = new Size(700, 300);
            this.dgvCarrito.ReadOnly = true;
            this.dgvCarrito.AllowUserToAddRows = false;
            this.dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 🔴 COLUMNA OCULTA ID PRODUCTO
            var colId = new DataGridViewTextBoxColumn
            {
                Name = "ProductoId",
                Visible = false
            };
            this.dgvCarrito.Columns.Add(colId);

            this.dgvCarrito.Columns.Add("Producto", "Producto");
            this.dgvCarrito.Columns.Add("Cantidad", "Cantidad");
            this.dgvCarrito.Columns.Add("Precio", "Precio");
            this.dgvCarrito.Columns.Add("Subtotal", "Subtotal");

            // TOTAL
            this.lblTotalTitulo.Text = "TOTAL:";
            this.lblTotalTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTotalTitulo.Location = new Point(20, 390);

            this.lblTotal.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTotal.Location = new Point(120, 390);
            this.lblTotal.Text = "0";

            // MÉTODO PAGO
            this.cboMetodoPago.Location = new Point(450, 20);
            this.cboMetodoPago.Width = 200;
            this.cboMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;

            // CLIENTE FIADO
            this.cboClienteFiado.Location = new Point(450, 55);
            this.cboClienteFiado.Width = 200;
            this.cboClienteFiado.Visible = false;

            // BOTONES
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Location = new Point(740, 120);

            this.btnFinalizar.Text = "Finalizar Venta";
            this.btnFinalizar.Location = new Point(740, 170);

            this.btnPagarProveedor.Text = "Pagar Proveedor";
            this.btnPagarProveedor.Location = new Point(740, 230);

            // FORM
            this.ClientSize = new Size(900, 450);
            this.Controls.AddRange(new Control[] {
                txtCodigo, dgvCarrito, lblTotal, lblTotalTitulo,
                cboMetodoPago, cboClienteFiado,
                btnEliminar, btnFinalizar, btnPagarProveedor
            });

            this.Text = "Ventas";

            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
