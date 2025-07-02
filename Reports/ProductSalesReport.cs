using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace C__Intern_Testing.Reports
{
    public partial class ProductSalesReport : DevExpress.XtraReports.UI.XtraReport
    {
        //public ProductSalesReport()
        //{
        //    InitializeComponent();
        //}
        public ProductSalesReport()
        {
            var paramStartDate = new DevExpress.XtraReports.Parameters.Parameter
            {
                Name = "StartDate",
                Type = typeof(DateTime),
                Description = "Start Date",
                Visible = false
            };

            var paramEndDate = new DevExpress.XtraReports.Parameters.Parameter
            {
                Name = "EndDate",
                Type = typeof(DateTime),
                Description = "End Date",
                Visible = false
            };

            var paramProductName = new DevExpress.XtraReports.Parameters.Parameter
            {
                Name = "ProductNameFilter",
                Type = typeof(string),
                Description = "Product Name Filter",
                Visible = false
            };
            this.Parameters.Add(paramProductName);


            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
                paramStartDate, paramEndDate
            });
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);

            ReportHeaderBand header = new ReportHeaderBand { HeightF = 40 };
            XRLabel title = new XRLabel
            {
                Text = "Product Sales Report",
                Font = headerFont,
                BoundsF = new RectangleF(0, 0, 500, 40),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };
            header.Controls.Add(title);
            this.Bands.Add(header);
            XRLabel lblStart = new XRLabel
            {
                BoundsF = new RectangleF(100, 45, 250, 25),
                Font = new Font("Arial", 10),
                ExpressionBindings = {
                 new ExpressionBinding("BeforePrint", "Text", "'Start Date: ' + FormatString('{0:dd/MM/yyyy}', ?StartDate)")
                }
            };
            header.Controls.Add(lblStart);

            XRLabel lblEnd = new XRLabel
            {
                BoundsF = new RectangleF(200, 45, 250, 25),
                Font = new Font("Arial", 10),
                ExpressionBindings = {
                new ExpressionBinding("BeforePrint", "Text", "'End Date: ' + FormatString('{0:dd/MM/yyyy}', ?EndDate)")
                },
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };
            header.Controls.Add(lblEnd);

            XRLabel lblFilter = new XRLabel
            {
                BoundsF = new RectangleF(0, 70, 500, 25),
                Font = new Font("Arial", 9),
                ExpressionBindings = {
                    new ExpressionBinding("BeforePrint", "Text", "'Filter: ' + ?ProductNameFilter")
                }
            };
            header.Controls.Add(lblFilter);


            // Group Header
            GroupHeaderBand groupHeader = new GroupHeaderBand { HeightF = 25 };
            groupHeader.GroupFields.Add(new GroupField("ProductCode"));
            XRLabel groupLabel = new XRLabel
            {
                ExpressionBindings = {
                new ExpressionBinding("BeforePrint", "Text", "'Product Code: ' + [ProductCode]")
            },
                Font = new Font("Arial", 10, FontStyle.Bold),
                BoundsF = new RectangleF(0, 0, 500, 25)
            };
            groupHeader.Controls.Add(groupLabel);
            this.Bands.Add(groupHeader);

            // Detail Band (No SaleDate column)
            DetailBand detail = new DetailBand { HeightF = 25 };
            XRTable detailTable = new XRTable { WidthF = 500, Borders = DevExpress.XtraPrinting.BorderSide.All };
            XRTableRow detailRow = new XRTableRow();

            detailRow.Cells.Add(CreateCell("[ProductName]", 150));
            detailRow.Cells.Add(CreateCell("[Quantity]", 80));
            detailRow.Cells.Add(CreateCell("[UnitPrice]", 80));
            detailRow.Cells.Add(CreateCell("[Quantity] * [UnitPrice]", 120));
            detailTable.Rows.Add(detailRow);
            detail.Controls.Add(detailTable);
            this.Bands.Add(detail);

            // Group Footer
            GroupFooterBand groupFooter = new GroupFooterBand { HeightF = 30 };
            groupFooter.Controls.Add(CreateGroupSummaryTable());
            this.Bands.Add(groupFooter);

            // Report Footer
            ReportFooterBand reportFooter = new ReportFooterBand { HeightF = 30 };
            reportFooter.Controls.Add(CreateGrandSummaryTable());
            this.Bands.Add(reportFooter);
        }


        private XRTableCell CreateCell(string expression, float width, FontStyle fontStyle = FontStyle.Regular)
        {
            return new XRTableCell
            {
                WidthF = width,
                Font = new Font("Arial", 10, fontStyle),
                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", expression) },
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Borders = DevExpress.XtraPrinting.BorderSide.All
            };
        }

        private XRTableCell CreateSummaryCell(string expression, float width, bool isReportTotal = false)
        {
            XRTableCell cell = new XRTableCell
            {
                WidthF = width,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            cell.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", expression));
            cell.Summary = new XRSummary
            {
                Func = SummaryFunc.Sum,
                Running = isReportTotal ? SummaryRunning.Report : SummaryRunning.Group,
                FormatString = "{0:n2}"
            };

            return cell;
        }

        private XRTable CreateGroupSummaryTable()
        {
            XRTable table = new XRTable { WidthF = 500 };
            XRTableRow row = new XRTableRow();

            row.Cells.Add(new XRTableCell
            {
                Text = "Group Total: Qty =",
                WidthF = 150,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            });

            row.Cells.Add(CreateSummaryCell("sumSum([Quantity])", 80));

            row.Cells.Add(new XRTableCell
            {
                Text = "  Total = $",
                WidthF = 80,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            });

            row.Cells.Add(CreateSummaryCell("sumSum([Quantity] * [UnitPrice])", 120));

            table.Rows.Add(row);
            return table;
        }

        private XRTable CreateGrandSummaryTable()
        {
            XRTable table = new XRTable { WidthF = 500 };
            XRTableRow row = new XRTableRow();

            row.Cells.Add(new XRTableCell
            {
                Text = "GRAND TOTAL: Qty =",
                WidthF = 150,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            });

            row.Cells.Add(CreateSummaryCell("sumSum([Quantity])", 80, true));

            row.Cells.Add(new XRTableCell
            {
                Text = "  Total = $",
                WidthF = 80,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            });

            row.Cells.Add(CreateSummaryCell("sumSum([Quantity] * [UnitPrice])", 120, true));

            table.Rows.Add(row);
            return table;
        }
    }
}
