using System.IO;
using System.Windows.Forms;
using MiniMarket.Models;

namespace MiniMarket
{
    public static class Exportadores
    {
        public static void ExportarExcel(ReporteGuardado r)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Excel (*.csv)|*.csv",
                FileName = r.Nombre + ".csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            File.WriteAllText(sfd.FileName, r.Contenido);

            MessageBox.Show("Excel exportado correctamente");
        }
        public static void ExportarPdf(ReporteGuardado r)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = r.Nombre + ".pdf"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            var contenido =
                $"REPORTE: {r.Nombre}\n\n" +
                $"Fecha: {r.FechaGuardado}\n\n" +
                r.Contenido;

            File.WriteAllText(sfd.FileName, contenido);

            MessageBox.Show("PDF exportado correctamente");
        }
    }
}
