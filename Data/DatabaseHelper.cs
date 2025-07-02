using C__Intern_Testing.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__Intern_Testing.Data
{
    public static class DatabaseHelper
    {
        private static string connString = @"Data Source=DESKTOP-BB8V27R\SQLEXPRESS01;Initial Catalog=ProductSales;Integrated Security=True";

        public static List<SaleDto> GetSales(DateTime startDate, DateTime endDate, string productNameFilter = "")
        {
            List<SaleDto> sales = new List<SaleDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = @"
                        SELECT PRODUCTCODE, PRODUCTNAME, QUANTITY, UNITPRICE, SALEDATE
                        FROM PRODUCTSALES
                        WHERE SALEDATE BETWEEN @START AND @END
                        AND PRODUCTNAME LIKE @PRODUCTNAME";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@START", startDate);
                        cmd.Parameters.AddWithValue("@END", endDate);
                        cmd.Parameters.AddWithValue("@PRODUCTNAME", $"%{productNameFilter}%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sales.Add(new SaleDto
                                {
                                    ProductCode = reader["PRODUCTCODE"].ToString(),
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    Quantity = Convert.ToInt32(reader["QUANTITY"]),
                                    UnitPrice = Convert.ToDecimal(reader["UNITPRICE"]),
                                    SaleDate = Convert.ToDateTime(reader["SALEDATE"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Directory.CreateDirectory("logs");
                File.AppendAllText("logs/errors.txt", $"{DateTime.Now}: {ex.Message}{Environment.NewLine}");
            }

            return sales;
        }
    }
}
