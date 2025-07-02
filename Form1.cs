using C__Intern_Testing.Data;
using C__Intern_Testing.Reports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C__Intern_Testing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGnReport_Click(object sender, EventArgs e)
        {
            string keyword = txtProductNfilter.Text.Trim();
            var data = DatabaseHelper.GetSales(dtpStart.Value, dtpEnd.Value, keyword);
            if (data.Count == 0)
            {
                MessageBox.Show("No results found.");
                return;
            }

            var report = new ProductSalesReport();
            report.DataSource = data;
            report.Parameters["StartDate"].Value = dtpStart.Value;
            report.Parameters["EndDate"].Value = dtpEnd.Value;
            report.Parameters["ProductNameFilter"].Value = keyword;

            new ReportPrintTool(report).ShowPreview();
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            string keyword = txtProductNfilter.Text.Trim();
            var data = DatabaseHelper.GetSales(dtpStart.Value, dtpEnd.Value, keyword);
            if (data.Count == 0)
            {
                MessageBox.Show("No results found.");
                return;
            }

            var report = new ProductSalesReport();
            report.DataSource = data;
            report.Parameters["StartDate"].Value = dtpStart.Value;
            report.Parameters["EndDate"].Value = dtpEnd.Value;
            report.Parameters["ProductNameFilter"].Value = keyword;

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = "ProductSalesReport.pdf"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                report.ExportToPdf(saveDialog.FileName);
                MessageBox.Show("Report exported to PDF successfully.");
            }
        }
    }
}
