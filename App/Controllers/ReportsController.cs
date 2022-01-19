using Data;
using Microsoft.Reporting.WebForms;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reports
        public ActionResult ProductStock(string id)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "ProductStock.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
           
            List<Product> cm = new List<Product>();

            cm = db.Products.ToList();
            ReportDataSource rd = new ReportDataSource("DataSetProductStock", cm);
            lr.DataSources.Add(rd);

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult SaleRecordReport(string id, string fd, string td)
        {
            LocalReport lr = new LocalReport();

          
            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleRecordReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,Sales.STAmount, Sales.TSSD,Sales.TSVat, Sales.TransportCost, Sales.Discount, Sales.CashReceive, Sales.DueAmount,Sales.OtherCost,  Sales.NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate Between '" + fd + "' AND '" + td + "' ";
            
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                
                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                 lr.DataSources.Add(rd);
            }
            
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult SaleRecordReports(string id, string fd, string td)
        {
            LocalReport lr = new LocalReport();

            var vat = db.VatEntrys.Select(x=>x.VatAmount).FirstOrDefault();

            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleRecordReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            // string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,Sales.STAmount, Sales.TSSD,Sales.TSVat, Sales.TransportCost, Sales.Discount, Sales.CashReceive, Sales.DueAmount,Sales.OtherCost,  Sales.NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate Between '" + fd + "' AND '" + td + "' ";

            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,((Sales.STAmount*"+ vat + ")/100) as STAmount, Sales.TSSD,((Sales.TSVat*"+vat+")/100) as TSVat, ((Sales.TransportCost*"+ vat + ")/100) as TransportCost, Sales.Discount, ((Sales.CashReceive*"+ vat + ")/100) as CashReceive, ((Sales.DueAmount *"+vat+")/100) as DueAmount,Sales.OtherCost,  ((Sales.NetTotal*"+ vat + ")/100) as NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate Between '" + fd + "' AND '" + td + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DueList()
        {
            string reportType = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "DueListReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            List<Customer> cm = new List<Customer>();

            cm = db.Customers.Where(c => c.TotalDueAmount > 0).ToList();
            ReportDataSource rd = new ReportDataSource("DueListDataSet", cm);
            lr.DataSources.Add(rd);
           
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
             "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public ActionResult BottledAndJarSale(string id, string fd, string td)
        {
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleRecordReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,Sales.STAmount, Sales.TSSD,Sales.TSVat, Sales.TransportCost, Sales.Discount, Sales.CashReceive, Sales.DueAmount,Sales.OtherCost,  Sales.NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  Where Sales.SaleDate Between '" + fd + "' AND '" + td + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //using (SqlConnection conn2 = new SqlConnection(connString2))
            //{
            //    SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
            //    da2.Fill(cm);
            //    ReportDataSource rd = new ReportDataSource("SaleRecordDataSet", cm);
            //    lr.DataSources.Add(rd);
            //}

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DueCollectionRecords(string id, int Acc_id, string fd, string td)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "CustomerDueListReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT DueTransactions.DueTransactionId, DueTransactions.DueTR_Id, DueTransactions.PaymentDate, DueTransactions.InAmount, DueTransactions.IsDeleted, Customers.CustomerName, Customers.ContactPerson, Customers.Phone FROM DueTransactions INNER JOIN Customers ON DueTransactions.CustomerId = Customers.CustomerId Where DueTransactions.PaymentDate Between '" + fd + "' AND '" + td + "' AND DueTransactions.CustomerId=" + Acc_id;

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                ReportDataSource rd = new ReportDataSource("CustomerDueDataSet", cm);
               
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DueAllRecordsList(string id, string fd, string td)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "AllDueListReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT DueTransactions.DueTransactionId, DueTransactions.DueTR_Id, DueTransactions.PaymentDate, DueTransactions.InAmount, DueTransactions.IsDeleted, Customers.CustomerName, Customers.ContactPerson, Customers.Phone FROM  DueTransactions INNER JOIN Customers ON DueTransactions.CustomerId = Customers.CustomerId  Where DueTransactions.PaymentDate Between '" + fd + "' AND '" + td + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllDueDataSet", cm);
                lr.DataSources.Add(rd);
            }
         

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult WaterProductionRecords( string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "WaterProductionreport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT ProductionDetails.ProductionDetailId, ProductionDetails.Quantity, ProductionDetails.QtyCase, ProductionDetails.Litter, Productions.ProductionDate, Products.Size FROM ProductionDetails INNER JOIN Productions ON ProductionDetails.ProductionId = Productions.ProductionId INNER JOIN Products ON ProductionDetails.ProductId = Products.ProductId Where Productions.ProductionDate Between '" + fromdate + "' AND '" + todate + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("WaterProductionDataSet", cm);
                lr.DataSources.Add(rd);
            }


            
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>.75in</MarginLeft>" +
            "  <MarginRight>.75in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult WaterSalesRecords(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "WaterSaleReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT SaleDetails.SaleDetailId, SaleDetails.Quantity,Customers.CustomerName, SaleDetails.TotalPrice, SaleDetails.QtyCase,Sales.InvoiceId, Sales.SaleDate,Sales.NetTotal,Sales.DiscountAmount,Sales.TransportCost, Products.ProductName, Products.Size FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  Where Sales.IsDeleted='" + false + "' AND Sales.SaleDate Between '" + fromdate + "' AND '" + todate + "' Group By SaleDetails.SaleDetailId, SaleDetails.Quantity, SaleDetails.TotalPrice, SaleDetails.QtyCase,Sales.InvoiceId, Sales.SaleDate,Sales.NetTotal,Sales.DiscountAmount,Sales.TransportCost, Products.ProductName, Products.Size,Customers.CustomerName";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("WaterSalesDataSet", cm);
                lr.DataSources.Add(rd);
            }



            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult WaterSalesCustomerRecords(long CustomerId, string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "WaterSaleCustomerReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.SaleDate, Sales.NetTotal, Sales.DiscountAmount , SaleDetails.Quantity, SaleDetails.QtyCase, SaleDetails.TotalPrice, Products.ProductName, Products.Size, Customers.CustomerName, Customers.ContactPerson, Customers.AddressLine1, Customers.Phone, Sales.InvoiceId, Sales.SaleId, Sales.TransportCost FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId INNER JOIN SaleDetails ON Sales.SaleId = SaleDetails.SaleId INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId  Where Sales.IsDeleted='" + false + "' AND Sales.SaleDate Between '" + fromdate + "' AND '" + todate + "'AND Sales.CustomerId=" + CustomerId;

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("WaterSalesCustomerDataSet", cm);
                lr.DataSources.Add(rd);
            }



            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult WaterSalesUserRecords(string UserId, string fromdate, string todate)
        {
            string id = "pdf";
            
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "WaterSaleUserReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.SaleId, Sales.InvoiceId,Sales.AvgDueAmount,Sales.AvgCashReceive, Sales.TransportCost,Sales.OtherCost, Sales.Discount, Sales.CashReceive, Sales.DueAmount, Sales.SaleDate, Sales.NetTotal,Sales.AddedById, Sales.DiscountAmount, Sales.AddedById as UserName, SaleDetails.Quantity,SaleDetails.TotalPrice, SaleDetails.QtyCase, Products.ProductName, Products.Size, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  INNER JOIN  SaleDetails ON Sales.SaleId = SaleDetails.SaleId INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId  Where Sales.IsDeleted='" + false + "' AND Sales.SaleDate Between '" + fromdate + "' AND '" + todate + "'AND Sales.AddedById='" + UserId+"'";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("WaterSalesUserDataSet", cm);
                lr.DataSources.Add(rd);
            }



            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult GatePassRecordReport(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "GatePassRecordReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.SaleId, Sales.InvoiceId, Sales.SaleDate, Customers.CustomerName, Products.ProductName, SaleDetails.Quantity, SaleDetails.QtyCase FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId INNER JOIN  SaleDetails ON Sales.SaleId = SaleDetails.SaleId INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate Between '" + fromdate + "' AND '" + todate + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("GatePassDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        //Production Report Bottle And Jar 
        public ActionResult BottleAndJarProduction(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "BottleAndJarProduction.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            List<ProductionStatement> cm = new List<ProductionStatement>();

            cm = db.ProductionStatements.Where(c => c.ProductionDate >= sdate && c.ProductionDate <= edate).ToList();

            ReportDataSource rd = new ReportDataSource("ProductionStatementDataSet", cm);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult BottleProduction(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "BottleProduction.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            List<ProductionStatement> cm = new List<ProductionStatement>();
            cm = db.ProductionStatements.Where(c => c.ProductionDate >= sdate && c.ProductionDate <= edate).ToList();
            ReportDataSource rd = new ReportDataSource("ProductionStatementDataSet", cm);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult JarProduction(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "JarProduction.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            List<ProductionStatement> cm = new List<ProductionStatement>();

            cm = db.ProductionStatements.Where(c => c.ProductionDate >= sdate && c.ProductionDate <= edate).ToList();

            ReportDataSource rd = new ReportDataSource("ProductionStatementDataSet", cm);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.9in</MarginLeft>" +
            "  <MarginRight>0.9in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        //Bottle And Jar Sales
        public ActionResult BottleAndJarSale(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "BottleAndJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);

            List<SalesStatement> cm = new List<SalesStatement>();

            cm = db.SalesStatements.Where(c => c.StatementDate >= sdate && c.StatementDate <= edate).ToList();

            ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);

            lr.DataSources.Add(rd);


            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult BottleAndJarSaleWithServiceCharge(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "BottleAndJarSaleWithSvcCharge.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = $@"
                        SELECT SalesStatementId
                              ,StatementDate
                              ,Swml250Quantity
                              ,Swml250TotalPrice
                              ,Sw5mlQuantity
                              ,Sw5mlTotalPrice
                              ,Sw1lQuantity
                              ,Sw1lTotalPrice
                              ,Sw15lQuintity
                              ,Sw15lTotalPrice
                              ,Sw2lQuintity
                              ,Sw2lTotalPrice
                              ,Sw5lQuintity
                              ,Sw5lTotalPrice
                              ,Sw20lQuintity
                              ,Sw20lTotalPrice
                              ,Sw20l4Quintity
                              ,Sw20l4TotalPrice
                              ,Status
                              ,AddedById
                              ,AddedDate
                              ,ModifiedById
                              ,ModifiedDate
                              ,IpAddress
                              ,IsDeleted
                              ,Product_ProductId
                              ,Customer_CustomerId
	                          ,(SELECT ISNULL(SUM(TransportCost),0) FROM Sales WHERE SaleDate = StatementDate AND IsDeleted = 0) AS TransportCost
	                          ,(SELECT ISNULL(SUM(OtherCost),0) FROM Sales WHERE SaleDate = StatementDate AND IsDeleted = 0) AS OtherCost
                              ,CONVERT(DATETIME,'{fromdate}') AS DateFrom
                              ,CONVERT(DATETIME,'{todate}') AS DateTo
                          FROM SalesStatements 
                          WHERE StatementDate BETWEEN '{fromdate}' AND '{todate}'";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }




            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult BottleAndJarSales(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "BottleAndJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);
            var vat=db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT SalesStatementId,StatementDate,(Swml250Quantity*"+ vat +")/100 as Swml250Quantity, (Swml250TotalPrice/NULLIF(Swml250Quantity,0)*(Swml250Quantity*"+ vat +"/100)) as Swml250TotalPrice,(Sw5mlQuantity*"+ vat +")/100 as Sw5mlQuantity,(Sw5mlTotalPrice/NULLIF(Sw5mlQuantity,0)*(Sw5mlQuantity*"+ vat +"/100)) as Sw5mlTotalPrice,(Sw1lQuantity*"+ vat +")/100 as Sw1lQuantity,(Sw1lTotalPrice/NULLIF(Sw1lQuantity,0)*(Sw1lQuantity*"+ vat +"/100)) as Sw1lTotalPrice,(Sw15lQuintity*"+ vat +")/100 as Sw15lQuintity,(Sw15lTotalPrice/NULLIF(Sw15lQuintity,0)*(Sw15lQuintity*"+ vat +"/100)) as Sw15lTotalPrice, (Sw2lQuintity*"+ vat +")/100 as Sw2lQuintity,(Sw2lTotalPrice/NULLIF(Sw2lQuintity,0)*(Sw2lQuintity*"+ vat +"/100)) as Sw2lTotalPrice, (Sw5lQuintity*"+ vat +")/100 as Sw5lQuintity,(Sw5lTotalPrice/NULLIF(Sw5lQuintity,0)*(Sw5lQuintity*"+ vat +"/100)) as Sw5lTotalPrice,(Sw20lQuintity*"+ vat +")/100 as Sw20lQuintity,(Sw20lTotalPrice/NULLIF(Sw20lQuintity,0)*(Sw20lQuintity*"+ vat +"/100)) as Sw20lTotalPrice,(Sw20l4Quintity*"+ vat +")/100 as Sw20l4Quintity,(Sw20l4TotalPrice/NULLIF(Sw20l4Quintity,0)*(Sw20l4Quintity*"+ vat +"/100)) as Sw20l4TotalPrice,Status,AddedById,AddedDate,ModifiedById, ModifiedDate, IpAddress,IsDeleted FROM SalesStatements Where StatementDate Between '" + fromdate + "' AND '" + todate + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult BottleSale(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "BottleSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);
            List<SalesStatement> cm = new List<SalesStatement>();

            cm = db.SalesStatements.Where(c => c.StatementDate >= sdate && c.StatementDate <= edate).ToList();
            ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult BottleSales(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "BottleSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT SalesStatementId,StatementDate,(Swml250Quantity*" + vat + ")/100 as Swml250Quantity, (Swml250TotalPrice/NULLIF(Swml250Quantity,0)*(Swml250Quantity*" + vat + "/100)) as Swml250TotalPrice,(Sw5mlQuantity*" + vat + ")/100 as Sw5mlQuantity,(Sw5mlTotalPrice/NULLIF(Sw5mlQuantity,0)*(Sw5mlQuantity*" + vat + "/100)) as Sw5mlTotalPrice,(Sw1lQuantity*" + vat + ")/100 as Sw1lQuantity,(Sw1lTotalPrice/NULLIF(Sw1lQuantity,0)*(Sw1lQuantity*" + vat + "/100)) as Sw1lTotalPrice,(Sw15lQuintity*" + vat + ")/100 as Sw15lQuintity,(Sw15lTotalPrice/NULLIF(Sw15lQuintity,0)*(Sw15lQuintity*" + vat + "/100)) as Sw15lTotalPrice, (Sw2lQuintity*" + vat + ")/100 as Sw2lQuintity,(Sw2lTotalPrice/NULLIF(Sw2lQuintity,0)*(Sw2lQuintity*" + vat + "/100)) as Sw2lTotalPrice, (Sw5lQuintity*" + vat + ")/100 as Sw5lQuintity,(Sw5lTotalPrice/NULLIF(Sw5lQuintity,0)*(Sw5lQuintity*" + vat + "/100)) as Sw5lTotalPrice,(Sw20lQuintity*" + vat + ")/100 as Sw20lQuintity,(Sw20lTotalPrice/NULLIF(Sw20lQuintity,0)*(Sw20lQuintity*" + vat + "/100)) as Sw20lTotalPrice,(Sw20l4Quintity*" + vat + ")/100 as Sw20l4Quintity,(Sw20l4TotalPrice/NULLIF(Sw20l4Quintity,0)*(Sw20l4Quintity*" + vat + "/100)) as Sw20l4TotalPrice,Status,AddedById,AddedDate,ModifiedById, ModifiedDate, IpAddress,IsDeleted FROM SalesStatements Where StatementDate Between '" + fromdate + "' AND '" + todate + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult JarSale(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "JarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);
            List<SalesStatement> cm = new List<SalesStatement>();

            cm = db.SalesStatements.Where(c => c.StatementDate >= sdate && c.StatementDate <= edate).ToList();
            ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.9in</MarginLeft>" +
            "  <MarginRight>0.9in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult JarSales(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "JarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime edate = Convert.ToDateTime(todate);
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT SalesStatementId,StatementDate,(Swml250Quantity*" + vat + ")/100 as Swml250Quantity, (Swml250TotalPrice/NULLIF(Swml250Quantity,0)*(Swml250Quantity*" + vat + "/100)) as Swml250TotalPrice,(Sw5mlQuantity*" + vat + ")/100 as Sw5mlQuantity,(Sw5mlTotalPrice/NULLIF(Sw5mlQuantity,0)*(Sw5mlQuantity*" + vat + "/100)) as Sw5mlTotalPrice,(Sw1lQuantity*" + vat + ")/100 as Sw1lQuantity,(Sw1lTotalPrice/NULLIF(Sw1lQuantity,0)*(Sw1lQuantity*" + vat + "/100)) as Sw1lTotalPrice,(Sw15lQuintity*" + vat + ")/100 as Sw15lQuintity,(Sw15lTotalPrice/NULLIF(Sw15lQuintity,0)*(Sw15lQuintity*" + vat + "/100)) as Sw15lTotalPrice, (Sw2lQuintity*" + vat + ")/100 as Sw2lQuintity,(Sw2lTotalPrice/NULLIF(Sw2lQuintity,0)*(Sw2lQuintity*" + vat + "/100)) as Sw2lTotalPrice, (Sw5lQuintity*" + vat + ")/100 as Sw5lQuintity,(Sw5lTotalPrice/NULLIF(Sw5lQuintity,0)*(Sw5lQuintity*" + vat + "/100)) as Sw5lTotalPrice,(Sw20lQuintity*" + vat + ")/100 as Sw20lQuintity,(Sw20lTotalPrice/NULLIF(Sw20lQuintity,0)*(Sw20lQuintity*" + vat + "/100)) as Sw20lTotalPrice,(Sw20l4Quintity*" + vat + ")/100 as Sw20l4Quintity,(Sw20l4TotalPrice/NULLIF(Sw20l4Quintity,0)*(Sw20l4Quintity*" + vat + "/100)) as Sw20l4TotalPrice,Status,AddedById,AddedDate,ModifiedById, ModifiedDate, IpAddress,IsDeleted FROM SalesStatements Where StatementDate Between '" + fromdate + "' AND '" + todate + "' ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.9in</MarginLeft>" +
            "  <MarginRight>0.9in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        //Daily Bottle And Jar Sales
        public ActionResult DailyBottleSale(string ProductCategory,string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "DailyBottleSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = $@"
                            SELECT CustomerName
	                            ,DailyStatementDate
	                            ,ReceieptNo
	                            ,Swml250Quantity
	                            ,Swml250TotalPrice
	                            ,Sw5mlQuantity
	                            ,Sw5mlTotalPrice
	                            ,Sw1lQuantity
	                            ,Sw1lTotalPrice
	                            ,Sw15lQuintity
	                            ,Sw15lTotalPrice
	                            ,Sw2lQuintity
	                            ,Sw2lTotalPrice
	                            ,Sw5lQuintity
	                            ,Sw5lTotalPrice
                            FROM DailySalesStatements 
                            where DailyStatementDate = '{todate}' 
                            and (Swml250Quantity> 0 or Sw5mlQuantity> 0 Or Sw1lQuantity> 0 or Sw15lQuintity> 0 or Sw2lQuintity> 0 or Sw5lQuintity> 0 );
                            ";


           // string query2 = "select CustomerName,DailyStatementDate,ReceieptNo,Swml250Quantity,Swml250TotalPrice,Sw5mlQuantity,Sw5mlTotalPrice,Sw1lQuantity,Sw1lTotalPrice,Sw15lQuintity,Sw15lTotalPrice,Sw2lQuintity,Sw2lTotalPrice,Sw5lQuintity,Sw5lTotalPrice from DailySalesStatements where DailyStatementDate = '" + todate + "' and (Swml250Quantity>0 or Sw5mlQuantity>0 Or Sw1lQuantity>0 or Sw15lQuintity>0 or Sw2lQuintity>0 or Sw5lQuintity>0 ) ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("DailySaleDataSet", cm);
                lr.DataSources.Add(rd);
            }            
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DailyBottleSales(string ProductCategory, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "DailyBottleSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            var vat=db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select CustomerName,DailyStatementDate,ReceieptNo,(Swml250Quantity*"+vat+ ")/100 as Swml250Quantity,(Swml250TotalPrice/Nullif(Swml250Quantity,0) * (Swml250Quantity*" + vat + "/100)) as Swml250TotalPrice,(Sw5mlQuantity*" + vat + ")/100 as Sw5mlQuantity,(Sw5mlTotalPrice/Nullif(Sw5mlQuantity,0) * (Sw5mlQuantity*" + vat + "/100)) as Sw5mlTotalPrice,(Sw1lQuantity*" + vat + ")/100 as Sw1lQuantity,(Sw1lTotalPrice/Nullif(Sw1lQuantity,0) * (Sw1lQuantity*" + vat + "/100)) as Sw1lTotalPrice,(Sw15lQuintity*" + vat + ")/100 as Sw15lQuintity,(Sw5mlTotalPrice/Nullif(Sw15lQuintity,0) * (Sw15lQuintity*" + vat + "/100)) as Sw15lTotalPrice,(Sw2lQuintity*" + vat + ")/100 as Sw2lQuintity,(Sw5mlTotalPrice/Nullif(Sw2lQuintity,0) * (Sw2lQuintity*" + vat + "/100)) as Sw2lTotalPrice,(Sw5lQuintity*" + vat + ")/100 as Sw5lQuintity,(Sw5lTotalPrice/NullIF(Sw5lQuintity,0)*(Sw5lQuintity*" + vat + "/100)) as Sw5lTotalPrice from DailySalesStatements where DailyStatementDate = '" + todate +"' and (Swml250Quantity>0 or Sw5mlQuantity>0 Or Sw1lQuantity>0 or Sw15lQuintity>0 or Sw2lQuintity>0 or Sw5lQuintity>0 ) ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("DailySaleDataSet", cm);
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult DailyJarSale(string ProductCategory,  string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "DailyJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = $@"
                        SELECT CustomerName
                        ,DailyStatementDate
                        ,ReceieptNo
                        ,Sw20lQuintity
                        ,Sw20lTotalPrice
                        ,Sw20l4Quintity
                        ,Sw20l4TotalPrice
                        FROM DailySalesStatements
                        WHERE DailyStatementDate = '{todate}'
                        AND (
                        Sw20lQuintity > 0
                        OR Sw20l4Quintity > 0
                        );";

           // string query2 = "select CustomerName,DailyStatementDate,ReceieptNo,Sw20lQuintity,Sw20lTotalPrice,Sw20l4Quintity,Sw20l4TotalPrice from DailySalesStatements where DailyStatementDate = '"+ todate + "' and (Sw20lQuintity >0 or Sw20l4Quintity >0)";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("DailySaleDataSet", cm);
                lr.DataSources.Add(rd);
            }
           
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.9in</MarginLeft>" +
            "  <MarginRight>0.9in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DailyJarSales(string ProductCategory, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "DailyJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select CustomerName,DailyStatementDate,ReceieptNo,(Sw20lQuintity*" + vat + ")/100 as Sw20lQuintity,(Sw20lTotalPrice/NULLIF(Sw20lQuintity,0)*(Sw20lQuintity*" + vat + "/100)) as Sw20lTotalPrice,(Sw20l4Quintity*" + vat + ")/100 as Sw20l4Quintity,(Sw20l4TotalPrice/NULLIF(Sw20l4Quintity,0)*(Sw20l4Quintity*" + vat + "/100)) as Sw20l4TotalPrice from DailySalesStatements where DailyStatementDate = '" + todate + "' and (Sw20lQuintity >0 or Sw20l4Quintity >0)";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("DailySaleDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.9in</MarginLeft>" +
            "  <MarginRight>0.9in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult MonthlyBottleAndJarSale(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "MonthlyBottleAndJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select max(StatementDate) StatementDate , datename(m, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) as MonthYear ,sum(Swml250Quantity) Swml250Quantity,sum(Sw5mlQuantity) Sw5mlQuantity, sum(Swml250TotalPrice) Swml250TotalPrice,sum(Sw5mlTotalPrice)Sw5mlTotalPrice, sum(Sw1lQuantity) Sw1lQuantity,sum(Sw1lTotalPrice) Sw1lTotalPrice,sum(Sw15lQuintity) Sw15lQuintity, sum(Sw15lTotalPrice) Sw15lTotalPrice,sum(Sw2lQuintity) Sw2lQuintity,sum(Sw2lTotalPrice) Sw2lTotalPrice, sum(Sw5lQuintity) Sw5lQuintity,sum(Sw5lTotalPrice) Sw5lTotalPrice,sum(Sw20lQuintity) Sw20lQuintity, sum(Sw20lTotalPrice) Sw20lTotalPrice,sum(Sw20l4Quintity) Sw20l4Quintity,sum(Sw20l4TotalPrice) Sw20l4TotalPrice,(select max(StatementDate) from SalesStatements where StatementDate > '" + fromdate + "') as lastValue,(select min(StatementDate) from SalesStatements where StatementDate < '" + todate + "') as firstvalue from SalesStatements where SalesStatements.StatementDate Between '" + fromdate + "' AND '"+ todate + "'group by datename(MM, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar)  order by StatementDate ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //DateTime sdate = Convert.ToDateTime(fromdate);
            //DateTime edate = Convert.ToDateTime(todate);
            //int startmonth = sdate.Month;
            //int startYear = sdate.Year;
            //int endMonth = edate.Month;
            //int endYear = edate.Year;
            //List<SalesStatement> cm = new List<SalesStatement>();

            //// cm = db.SalesStatements.Where(c => c.StatementDate >= sdate && c.StatementDate <= edate).ToList();

            //var result =db.SalesStatements.Where(x => (x.StatementDate.Month >= startmonth || x.StatementDate.Year >= startYear) && (x.StatementDate.Month <= endMonth || x.StatementDate.Year <= endYear))
            // .GroupBy(x => x.StatementDate.Month)
            //   .Select(g => new
            //   {
            //       Swml250Quantity = g.Sum(x => x.Swml250Quantity),
            //       Sw5mlQuantity = g.Sum(x => x.Sw5mlQuantity),
            //       Sw1lQuantity = g.Sum(x => x.Sw1lQuantity),
            //       Sw15lQuintity = g.Sum(x => x.Sw15lQuintity),
            //       Sw2lQuintity = g.Sum(x => x.Sw2lQuintity),
            //       Sw5lQuintity = g.Sum(x => x.Sw5lQuintity),
            //       Sw20l4Quintity = g.Sum(x => x.Sw20l4Quintity),
            //       Sw20lQuintity = g.Sum(x => x.Sw20lQuintity),

            //       Swml250TotalPrice = g.Sum(x => x.Swml250TotalPrice),
            //       Sw5mlTotalPrice = g.Sum(x => x.Sw5mlTotalPrice),
            //       Sw1lTotalPrice = g.Sum(x => x.Sw1lTotalPrice),
            //       Sw15lTotalPrice = g.Sum(x => x.Sw15lTotalPrice),
            //       Sw2lTotalPrice = g.Sum(x => x.Sw2lTotalPrice),
            //       Sw5lTotalPrice = g.Sum(x => x.Sw5lTotalPrice),
            //       Sw20lTotalPrice = g.Sum(x => x.Sw20lTotalPrice),
            //       Sw20l4TotalPrice = g.Sum(x => x.Sw20l4TotalPrice),
            //       StatementDate = g.Key,
            //      // StatementDate = g.Select(x=> x.StatementDate),
            //      // Statementhh =g.StatementDate
            //   }).ToList();

            //ReportDataSource rd = new ReportDataSource("AllStatementDataSet", result);
            //lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult MonthlyBottleAndJarSales(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "MonthlyBottleAndJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select max(StatementDate) StatementDate , datename(m, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) as MonthYear,(sum(Swml250Quantity)*"+ vat +")/100 Swml250Quantity,(sum(Swml250TotalPrice)/NULLIF(sum(Swml250Quantity),0)*(sum(Swml250Quantity)*"+ vat +"/100)) as Swml250TotalPrice,(sum(Sw5mlQuantity)*"+ vat +")/100 Sw5mlQuantity,(sum(Sw5mlTotalPrice)/NULLIF(sum(Sw5mlQuantity),0)*(sum(Sw5mlQuantity)*"+ vat +"/100)) Sw5mlTotalPrice,(sum(Sw1lQuantity)*"+ vat +")/100 Sw1lQuantity,(sum(Sw1lTotalPrice)/NULLIF(sum(Sw1lQuantity),0)*(sum(Sw1lQuantity)*"+ vat +"/100)) Sw1lTotalPrice,(sum(Sw15lQuintity)*"+ vat +")/100 Sw15lQuintity, (sum(Sw15lTotalPrice)/NULLIF(sum(Sw15lQuintity),0)*(sum(Sw15lQuintity)*"+ vat +"/100)) Sw15lTotalPrice,(sum(Sw2lQuintity)*"+ vat +")/100 Sw2lQuintity,(sum(Sw2lTotalPrice)/NULLIF(sum(Sw2lQuintity),0)*(sum(Sw2lQuintity)*"+ vat +"/100)) Sw2lTotalPrice,(sum(Sw5lQuintity)*"+ vat +")/100 Sw5lQuintity,(sum(Sw5lTotalPrice)/NULLIF(sum(Sw5lQuintity),0)*(sum(Sw5lQuintity)*"+ vat +"/100)) Sw5lTotalPrice,(sum(Sw20lQuintity)*"+ vat +")/100 Sw20lQuintity,(sum(Sw20lTotalPrice)/NULLIF(sum(Sw20lQuintity),0)*(sum(Sw20lQuintity)*"+ vat +"/100)) Sw20lTotalPrice,(sum(Sw20l4Quintity)*"+ vat +")/100 Sw20l4Quintity,(sum(Sw20l4TotalPrice)/NULLIF(sum(Sw20l4Quintity),0)*(sum(Sw20l4Quintity)*"+ vat +"/100)) Sw20l4TotalPrice,(select max(StatementDate) from SalesStatements where StatementDate > '" + fromdate + "') as lastValue,(select min(StatementDate) from SalesStatements where StatementDate < '" + todate + "') as firstvalue from SalesStatements where SalesStatements.StatementDate Between '" + fromdate + "' AND '" + todate + "'group by datename(MM, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) order by StatementDate ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //DateTime sdate = Convert.ToDateTime(fromdate);
            //DateTime edate = Convert.ToDateTime(todate);
            //int startmonth = sdate.Month;
            //int startYear = sdate.Year;
            //int endMonth = edate.Month;
            //int endYear = edate.Year;
            //List<SalesStatement> cm = new List<SalesStatement>();

            //// cm = db.SalesStatements.Where(c => c.StatementDate >= sdate && c.StatementDate <= edate).ToList();

            //var result =db.SalesStatements.Where(x => (x.StatementDate.Month >= startmonth || x.StatementDate.Year >= startYear) && (x.StatementDate.Month <= endMonth || x.StatementDate.Year <= endYear))
            // .GroupBy(x => x.StatementDate.Month)
            //   .Select(g => new
            //   {
            //       Swml250Quantity = g.Sum(x => x.Swml250Quantity),
            //       Sw5mlQuantity = g.Sum(x => x.Sw5mlQuantity),
            //       Sw1lQuantity = g.Sum(x => x.Sw1lQuantity),
            //       Sw15lQuintity = g.Sum(x => x.Sw15lQuintity),
            //       Sw2lQuintity = g.Sum(x => x.Sw2lQuintity),
            //       Sw5lQuintity = g.Sum(x => x.Sw5lQuintity),
            //       Sw20l4Quintity = g.Sum(x => x.Sw20l4Quintity),
            //       Sw20lQuintity = g.Sum(x => x.Sw20lQuintity),

            //       Swml250TotalPrice = g.Sum(x => x.Swml250TotalPrice),
            //       Sw5mlTotalPrice = g.Sum(x => x.Sw5mlTotalPrice),
            //       Sw1lTotalPrice = g.Sum(x => x.Sw1lTotalPrice),
            //       Sw15lTotalPrice = g.Sum(x => x.Sw15lTotalPrice),
            //       Sw2lTotalPrice = g.Sum(x => x.Sw2lTotalPrice),
            //       Sw5lTotalPrice = g.Sum(x => x.Sw5lTotalPrice),
            //       Sw20lTotalPrice = g.Sum(x => x.Sw20lTotalPrice),
            //       Sw20l4TotalPrice = g.Sum(x => x.Sw20l4TotalPrice),
            //       StatementDate = g.Key,
            //      // StatementDate = g.Select(x=> x.StatementDate),
            //      // Statementhh =g.StatementDate
            //   }).ToList();

            //ReportDataSource rd = new ReportDataSource("AllStatementDataSet", result);
            //lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult MonthlyBottleSale(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "MonthlyBottleSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select max(StatementDate) StatementDate , datename(m, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) as MonthYear ,sum(Swml250Quantity) Swml250Quantity,sum(Sw5mlQuantity) Sw5mlQuantity, sum(Swml250TotalPrice) Swml250TotalPrice,sum(Sw5mlTotalPrice)Sw5mlTotalPrice, sum(Sw1lQuantity) Sw1lQuantity,sum(Sw1lTotalPrice) Sw1lTotalPrice,sum(Sw15lQuintity) Sw15lQuintity, sum(Sw15lTotalPrice) Sw15lTotalPrice,sum(Sw2lQuintity) Sw2lQuintity,sum(Sw2lTotalPrice) Sw2lTotalPrice, sum(Sw5lQuintity) Sw5lQuintity,sum(Sw5lTotalPrice) Sw5lTotalPrice,(select max(StatementDate) from SalesStatements where StatementDate > '" + fromdate + "') as lastValue,(select min(StatementDate) from SalesStatements where StatementDate < '" + todate + "') as firstvalue from SalesStatements where SalesStatements.StatementDate Between '" + fromdate + "' AND '" + todate + "'group by datename(MM, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) order by StatementDate ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult MonthlyBottleSales(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "MonthlyBottleSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select max(StatementDate) StatementDate , datename(m, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) as MonthYear,(sum(Swml250Quantity)*" + vat + ")/100 Swml250Quantity,(sum(Swml250TotalPrice)/NULLIF(sum(Swml250Quantity),0)*(sum(Swml250Quantity)*" + vat + "/100)) as Swml250TotalPrice,(sum(Sw5mlQuantity)*" + vat + ")/100 Sw5mlQuantity,(sum(Sw5mlTotalPrice)/NULLIF(sum(Sw5mlQuantity),0)*(sum(Sw5mlQuantity)*" + vat + "/100)) Sw5mlTotalPrice,(sum(Sw1lQuantity)*" + vat + ")/100 Sw1lQuantity,(sum(Sw1lTotalPrice)/NULLIF(sum(Sw1lQuantity),0)*(sum(Sw1lQuantity)*" + vat + "/100)) Sw1lTotalPrice,(sum(Sw15lQuintity)*" + vat + ")/100 Sw15lQuintity, (sum(Sw15lTotalPrice)/NULLIF(sum(Sw15lQuintity),0)*(sum(Sw15lQuintity)*" + vat + "/100)) Sw15lTotalPrice,(sum(Sw2lQuintity)*" + vat + ")/100 Sw2lQuintity,(sum(Sw2lTotalPrice)/NULLIF(sum(Sw2lQuintity),0)*(sum(Sw2lQuintity)*" + vat + "/100)) Sw2lTotalPrice,(sum(Sw5lQuintity)*" + vat + ")/100 Sw5lQuintity,(sum(Sw5lTotalPrice)/NULLIF(sum(Sw5lQuintity),0)*(sum(Sw5lQuintity)*" + vat + "/100)) Sw5lTotalPrice,(sum(Sw20lQuintity)*" + vat + ")/100 Sw20lQuintity,(sum(Sw20lTotalPrice)/NULLIF(sum(Sw20lQuintity),0)*(sum(Sw20lQuintity)*" + vat + "/100)) Sw20lTotalPrice,(sum(Sw20l4Quintity)*" + vat + ")/100 Sw20l4Quintity,(sum(Sw20l4TotalPrice)/NULLIF(sum(Sw20l4Quintity),0)*(sum(Sw20l4Quintity)*" + vat + "/100)) Sw20l4TotalPrice,(select max(StatementDate) from SalesStatements where StatementDate > '" + fromdate + "') as lastValue,(select min(StatementDate) from SalesStatements where StatementDate < '" + todate + "') as firstvalue from SalesStatements where SalesStatements.StatementDate Between '" + fromdate + "' AND '" + todate + "'group by datename(MM, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) order by StatementDate ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult MonthlyJarSale(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "MonthlyJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
           
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select max(StatementDate) StatementDate ,datename(m, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) as MonthYear ,sum(Sw20lQuintity) Sw20lQuintity, sum(Sw20lTotalPrice) Sw20lTotalPrice,sum(Sw20l4Quintity) Sw20l4Quintity,sum(Sw20l4TotalPrice) Sw20l4TotalPrice,(select max(StatementDate) from SalesStatements where StatementDate > '" + fromdate + "') as lastValue,(select min(StatementDate) from SalesStatements where StatementDate < '" + todate + "') as firstvalue from SalesStatements where SalesStatements.StatementDate Between '" + fromdate + "' AND '" + todate + "'group by datename(MM, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) order by StatementDate";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.9in</MarginLeft>" +
            "  <MarginRight>0.9in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult MonthlyJarSales(string fromdate, string todate)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "MonthlyJarSale.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "select max(StatementDate) StatementDate , datename(m, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) as MonthYear,(sum(Sw20lQuintity)*" + vat + ")/100 Sw20lQuintity,(sum(Sw20lTotalPrice)/NULLIF(sum(Sw20lQuintity),0)*(sum(Sw20lQuintity)*" + vat + "/100)) Sw20lTotalPrice,(sum(Sw20l4Quintity)*" + vat + ")/100 Sw20l4Quintity,(sum(Sw20l4TotalPrice)/NULLIF(sum(Sw20l4Quintity),0)*(sum(Sw20l4Quintity)*" + vat + "/100)) Sw20l4TotalPrice,(select max(StatementDate) from SalesStatements where StatementDate > '" + fromdate + "') as lastValue,(select min(StatementDate) from SalesStatements where StatementDate < '" + todate + "') as firstvalue from SalesStatements where SalesStatements.StatementDate Between '" + fromdate + "' AND '" + todate + "'group by datename(MM, StatementDate) + ' ' + cast(datepart(yyyy, StatementDate) as varchar) order by StatementDate ";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("AllStatementDataSet", cm);
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.9in</MarginLeft>" +
            "  <MarginRight>0.9in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult DailySaleVatReport(string id, string date)
        {
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,Sales.STAmount, Sales.TSSD,Sales.TSVat, Sales.TransportCost, Sales.Discount, Sales.CashReceive, Sales.DueAmount,Sales.OtherCost,  Sales.NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId INNER JOIN DailySalesStatements dss on dss.ReceieptNo=Sales.InvoiceId Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate = '" + date + "'";
           
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DailyJarSaleVatReport(string id, string date)
        {
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleJarVatReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,Sales.STAmount, Sales.TSSD,Sales.TSVat, Sales.TransportCost, Sales.Discount, Sales.CashReceive, Sales.DueAmount,Sales.OtherCost,  Sales.NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId INNER JOIN DailySalesStatements dss on dss.ReceieptNo=Sales.InvoiceId Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate = '" + date + "' and (Sw20lQuintity >0 or Sw20l4Quintity >0)";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DailyBottleSaleVatReport(string id, string date)
        {
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,Sales.STAmount, Sales.TSSD,Sales.TSVat, Sales.TransportCost, Sales.Discount, Sales.CashReceive, Sales.DueAmount,Sales.OtherCost,  Sales.NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId INNER JOIN DailySalesStatements dss on dss.ReceieptNo=Sales.InvoiceId Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate = '" + date + "' and (Swml250Quantity>0 or Sw5mlQuantity>0 Or Sw1lQuantity>0 or Sw15lQuintity>0 or Sw2lQuintity>0 or Sw5lQuintity>0 )";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult MonthlySaleVatReport(string month, string year)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReportMonthly.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.SaleDate,datename(MONTH,Sales.SaleDate)MonthNames,datename(YEAR,Sales.SaleDate)YearName,Count(InvoiceId) InvoiceId,Sum(Sales.STAmount)STAmount, Sum(Sales.TSSD)TSSD,Sum(Sales.TSVat)TSVat, Sum(Sales.TransportCost)TransportCost, Sum(Sales.Discount)Discount, Sum(Sales.CashReceive)CashReceive, Sum(Sales.DueAmount)DueAmount,Sum(Sales.OtherCost)OtherCost,  Sum(Sales.NetTotal)NetTotal FROM Sales Where Sales.IsDeleted = 'False'AND Month(Sales.SaleDate)= '" + month +"' AND YEAR(Sales.SaleDate)= '"+ year +"' group by  Sales.SaleDate";
           
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("VatDataset", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult YearlySaleVatReport(string year)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReportYearly.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT MONTH(Sales.SaleDate) Month,DateName(MONTH,Sales.SaleDate) AS MonthNames,DATEPART( YEAR,Sales.SaleDate) YearName,Count(InvoiceId)InvoiceId, Sum(Sales.STAmount)STAmount, Sum(Sales.TSSD)TSSD,Sum(Sales.TSVat)TSVat, Sum(Sales.TransportCost)TransportCost, Sum(Sales.Discount)Discount, Sum(Sales.CashReceive)CashReceive, Sum(Sales.DueAmount)DueAmount,Sum(Sales.OtherCost)OtherCost,  Sum(Sales.NetTotal)NetTotal  FROM Sales  Where Sales.IsDeleted='False' AND YEAR(Sales.SaleDate)='" + year+ "' group by  DATEPART( YEAR,Sales.SaleDate),DateName( MONTH, Sales.SaleDate),MONTH(Sales.SaleDate)";
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("VatDataset", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult DailySaleVatReports(string id, string date)
        {
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,(Sales.STAmount*"+ vat +")/100 STAmount, (Sales.TSSD*"+ vat +")/100 TSSD,(Sales.TSVat*"+ vat +")/100 TSVat, (Sales.TransportCost*"+ vat +")/100 TransportCost, (Sales.Discount*"+ vat +")/100 Discount, (Sales.CashReceive*"+ vat +")/100 CashReceive, (Sales.DueAmount*"+ vat +")/100 DueAmount,(Sales.OtherCost*"+ vat +")/100 OtherCost,  (Sales.NetTotal*"+ vat + ")/100 NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  INNER JOIN DailySalesStatements dss on dss.ReceieptNo=Sales.InvoiceId  Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate = '" + date + "'";
            
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DailyJarSaleVatReports(string id, string date)
        {
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleJarVatReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,(Sales.STAmount*" + vat + ")/100 STAmount, (Sales.TSSD*" + vat + ")/100 TSSD,(Sales.TSVat*" + vat + ")/100 TSVat, (Sales.TransportCost*" + vat + ")/100 TransportCost, (Sales.Discount*" + vat + ")/100 Discount, (Sales.CashReceive*" + vat + ")/100 CashReceive, (Sales.DueAmount*" + vat + ")/100 DueAmount,(Sales.OtherCost*" + vat + ")/100 OtherCost,  (Sales.NetTotal*" + vat + ")/100 NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId INNER JOIN DailySalesStatements dss on dss.ReceieptNo=Sales.InvoiceId Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate = '" + date + "' and (Sw20lQuintity >0 or Sw20l4Quintity >0)";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult DailyBottleSaleVatReports(string id, string date)
        {
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.InvoiceId,Sales.SaleDate,(Sales.STAmount*" + vat + ")/100 STAmount, (Sales.TSSD*" + vat + ")/100 TSSD,(Sales.TSVat*" + vat + ")/100 TSVat, (Sales.TransportCost*" + vat + ")/100 TransportCost, (Sales.Discount*" + vat + ")/100 Discount, (Sales.CashReceive*" + vat + ")/100 CashReceive, (Sales.DueAmount*" + vat + ")/100 DueAmount,(Sales.OtherCost*" + vat + ")/100 OtherCost,  (Sales.NetTotal*" + vat + ")/100 NetTotal, Customers.CustomerName FROM Sales INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  INNER JOIN DailySalesStatements dss on dss.ReceieptNo=Sales.InvoiceId  Where Sales.IsDeleted='" + false + "'AND Sales.SaleDate = '" + date + "' and (Swml250Quantity>0 or Sw5mlQuantity>0 Or Sw1lQuantity>0 or Sw15lQuintity>0 or Sw2lQuintity>0 or Sw5lQuintity>0 )";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("SaleReportDataSet", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult MonthlySaleVatReports(string month, string year)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();


            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReportMonthly.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT Sales.SaleDate,datename(MONTH,Sales.SaleDate)MonthNames,datename(YEAR,Sales.SaleDate)YearName,Count(InvoiceId) InvoiceId,(Sum(Sales.STAmount)*"+ vat +")/100 STAmount, (Sum(Sales.TSSD)*"+ vat +")/100 TSSD,(Sum(Sales.TSVat)*"+ vat +")/100 TSVat, (Sum(Sales.TransportCost)*"+ vat +")/100 TransportCost, (Sum(Sales.Discount)*"+ vat +")/100 Discount, (Sum(Sales.CashReceive)*"+ vat +")/100 CashReceive, (Sum(Sales.DueAmount)*"+ vat +")/100 DueAmount,(Sum(Sales.OtherCost)*"+ vat +")/100 OtherCost,  (Sum(Sales.NetTotal)*"+ vat +")/100 NetTotal FROM Sales Where Sales.IsDeleted = 'False'AND Month(Sales.SaleDate)= '" + month + "' AND YEAR(Sales.SaleDate)= '" + year + "' group by  Sales.SaleDate";
//            string query2 = @"
//      SELECT S.saledate, 
//       Datename(month, S.saledate)         MonthNames, 
//       Datename(year, S.saledate)          YearName, 
//       Count(invoiceid)                    InvoiceId, 
//       --(Sum(S.stamount) * 75 ) / 100      STAmount, 
//	   ( Sum(S.tssd) * 75 ) / 100          TSSD, 
//       ( Sum(S.tsvat) * 75 ) / 100         TSVat, 
//       ( Sum(S.transportcost) * 75 ) / 100 TransportCost, 
//       ( Sum(S.discount) * 75 ) / 100      Discount, 
//       ( Sum(S.cashreceive) * 75 ) / 100   CashReceive, 
//       ( Sum(S.dueamount) * 75 ) / 100     DueAmount, 
//       ( Sum(S.othercost) * 75 ) / 100     OtherCost, 
//       ( Sum(S.nettotal) * 75 ) / 100      NetTotal,
//	   ((SELECT SUM(Quantity * (
//		CASE WHEN FacePrice = 7 THEN 6 
//		WHEN FacePrice = 10 THEN 9
//	    WHEN FacePrice = 15 THEN 13 
//		WHEN FacePrice = 18 THEN 16 
//		WHEN FacePrice = 22 THEN 20 
//		WHEN FacePrice = 45 THEN 40 
//		WHEN FacePrice = 40 THEN 30 
//		ELSE FacePrice END)) FROM [dbo].[SaleDetails] WHERE CONVERT(date,AddedDate) = S.SaleDate
//		)*75/100) STAmount
//FROM Sales S  
//WHERE  S.isdeleted = 'False' 
//       AND Month(S.saledate) = '" + month+@"' 
//       AND Year(S.saledate) = '"+year+@"' 
//GROUP  BY S.saledate
//";


            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("VatDataset", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }
        public ActionResult YearlySaleVatReports(string year)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            lr.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            string path = Path.Combine(Server.MapPath("~/Reports"), "SaleVatReportYearly.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View();
            }
            var vat = db.VatEntrys.Select(x => x.VatAmount).FirstOrDefault();

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT MONTH(Sales.SaleDate) Month,DateName(MONTH,Sales.SaleDate) AS MonthNames,DATEPART( YEAR,Sales.SaleDate) YearName,Count(InvoiceId)InvoiceId, (Sum(Sales.STAmount)*" + vat +")/100 STAmount, (Sum(Sales.TSSD)*"+ vat +")/100 TSSD,(Sum(Sales.TSVat)*"+ vat +")/100 TSVat, (Sum(Sales.TransportCost)*"+ vat +")/100 TransportCost, (Sum(Sales.Discount)*"+ vat +")/100 Discount, (Sum(Sales.CashReceive)*"+ vat +")/100 CashReceive, (Sum(Sales.DueAmount)*"+ vat +")/100 DueAmount,(Sum(Sales.OtherCost)*"+ vat +")/100 OtherCost,  (Sum(Sales.NetTotal)*"+ vat +")/100 NetTotal  FROM Sales Where Sales.IsDeleted='False' AND YEAR(Sales.SaleDate)='" + year + "' group by  DATEPART( YEAR,Sales.SaleDate),DateName( MONTH, Sales.SaleDate),MONTH(Sales.SaleDate)";
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("VatDataset", cm);
                lr.DataSources.Add(rd);
            }

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";
            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }

        public ActionResult ServiceAndWrappingChargeReport(string fromdate, string todate,string type)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = type == "Both" ? Path.Combine(Server.MapPath("~/Reports"), "ServiceAndWrappingChargeReportBoth.rdlc")
                                         : Path.Combine(Server.MapPath("~/Reports"), "ServiceAndWrappingChargeReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("");
            }
            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = $"EXEC [dbo].[GetServiceAndWrappingCharge] @DateFrom = '{fromdate}', @DateTo = '{todate}',@Type = {type}";

            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);

                ReportDataSource rd = new ReportDataSource("ServiceNWrappingChargeDataSet", cm);
                lr.DataSources.Add(rd);
            }



            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] renderedByte = null;
            string[] streams = null;
            Warning[] warnings = null;

            renderedByte = lr.Render(
                id,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedByte, mimeType);
        }




        //public ActionResult DailyBottleSales(string ProductCategory, string todate)
        //{
        //    string id = "pdf";
        //    LocalReport lr = new LocalReport();
        //    string path = Path.Combine(Server.MapPath("~/Reports"), "DailyBottleSale.rdlc");
        //    if (System.IO.File.Exists(path))
        //    {
        //        lr.ReportPath = path;
        //    }
        //    else
        //    {
        //        return View("");
        //    }


        //    DataTable cm = new DataTable();
        //    string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
        //    string query2 = "select * from (select sa.InvoiceId , c.CustomerName , sa.NetTotal,sa.SaleDate  , p.ProductShortName,p.ProductCategory, s.Quantity  from SaleDetails s  INNER JOIN Products p on p.ProductId = s.ProductId INNER JOIN Sales sa on sa.SaleId = s.SaleId INNER JOIN Customers c on c.CustomerId = sa.CustomerId  where sa.SaleDate = '" + todate + "'  and p.ProductCategory = 'Bottle') d pivot(  avg(Quantity)  for ProductShortName in ([SW250ML] ,[SW500ML],[SW1000ML],[SW1500ML],[SW2000ML],[SW5000ML])) piv";

        //    using (SqlConnection conn2 = new SqlConnection(connString2))
        //    {
        //        SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
        //        da2.Fill(cm);

        //        ReportDataSource rd = new ReportDataSource("DailySalesDataSet", cm);
        //        lr.DataSources.Add(rd);
        //    }
        //    //lr.DataSources.Add(rd);
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    "  <OutputFormat>" + id + "</OutputFormat>" +
        //    "  <PageWidth>8.5in</PageWidth>" +
        //    "  <PageHeight>11in</PageHeight>" +
        //    "  <MarginTop>0.3in</MarginTop>" +
        //    "  <MarginLeft>0.3in</MarginLeft>" +
        //    "  <MarginRight>0.3in</MarginRight>" +
        //    "  <MarginBottom>0.3in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    byte[] renderedByte = null;
        //    string[] streams = null;
        //    Warning[] warnings = null;

        //    renderedByte = lr.Render(
        //        id,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);
        //    return File(renderedByte, mimeType);
        //}
        //public ActionResult DailyJarSales(string ProductCategory, string fromdate, string todate)
        //{
        //    string id = "pdf";
        //    LocalReport lr = new LocalReport();
        //    string path = Path.Combine(Server.MapPath("~/Reports"), "DailyJarSale.rdlc");
        //    if (System.IO.File.Exists(path))
        //    {
        //        lr.ReportPath = path;
        //    }
        //    else
        //    {
        //        return View("");
        //    }

        //    DataTable cm = new DataTable();
        //    string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
        //    string query2 = "select * from (select sa.InvoiceId , c.CustomerName , sa.NetTotal,sa.SaleDate  , p.ProductShortName, s.Quantity  from SaleDetails s  inner  join Products p on p.ProductId = s.ProductId inner  join Sales sa on sa.SaleId = s.SaleId inner  join Customers c on c.CustomerId = sa.CustomerId  where sa.SaleDate = '" + todate + "'  and ProductCategory = 'Jar') d pivot(  avg(Quantity)  for ProductShortName in ([SW20000ML30],[SW20000ML40])) piv";

        //    using (SqlConnection conn2 = new SqlConnection(connString2))
        //    {
        //        SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
        //        da2.Fill(cm);

        //        ReportDataSource rd = new ReportDataSource("DailyJarSaleDataSet", cm);
        //        lr.DataSources.Add(rd);
        //    }
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    "  <OutputFormat>" + id + "</OutputFormat>" +
        //    "  <PageWidth>8.5in</PageWidth>" +
        //    "  <PageHeight>11in</PageHeight>" +
        //    "  <MarginTop>0.3in</MarginTop>" +
        //    "  <MarginLeft>0.9in</MarginLeft>" +
        //    "  <MarginRight>0.9in</MarginRight>" +
        //    "  <MarginBottom>0.3in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    byte[] renderedByte = null;
        //    string[] streams = null;
        //    Warning[] warnings = null;

        //    renderedByte = lr.Render(
        //        id,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);
        //    return File(renderedByte, mimeType);
        //}
    }
}