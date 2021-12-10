using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class SalesPrintReportController : Controller
    {
        // GET: SalesPrintReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Invoice(string reportType, long saleId)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "SalePrintReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT  Sales.AddedById as UserName,Sales.AddedById,Customers.CustomerName, Customers.AddressLine1,Customers.Phone, Products.ProductName, SaleDetails.Quantity, SaleDetails.TotalPrice, SaleDetails.FacePrice, Sales.InvoiceId, Sales.STAmount, Sales.TSSD, Sales.TSVat, Sales.TransportCost,Sales.OtherCost, Sales.Discount,Sales.DiscountAmount, Sales.CashReceive, Sales.DueAmount, Sales.SaleDate, Sales.NetTotal, SaleDetails.QtyCase FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId where Sales.SaleId=" + saleId;

            //Fill the VFWPostManagers Set with the data
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                ReportDataSource rd = new ReportDataSource("InvoiceDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>3in</PageWidth>" +
            "  <PageHeight>7in</PageHeight>" +
            "  <MarginTop>0.20in</MarginTop>" +
            "  <MarginLeft>0.0in</MarginLeft>" +
            "  <MarginRight>0.0in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
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
        public ActionResult Getpass(string reportType, long saleId)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "GetPassReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT  Sales.AddedById as UserName,Sales.AddedById,SaleDetails.SaleDetailId, SaleDetails.SaleId, SaleDetails.ProductId, SaleDetails.QtyCase, SaleDetails.Quantity, Customers.CustomerName, Customers.AddressLine1, Products.ProductName, Sales.InvoiceId,  Sales.AddedById,Sales.SaleDate FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  where Sales.SaleId=" + saleId;

            //Fill the VFWPostManagers Set with the data
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                ReportDataSource rd = new ReportDataSource("GetPassDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>3in</PageWidth>" +
            "  <PageHeight>5.5in</PageHeight>" +
            "  <MarginTop>0.20in</MarginTop>" +
            "  <MarginLeft>0.0in</MarginLeft>" +
            "  <MarginRight>0.0in</MarginRight>" +
            "  <MarginBottom>0.20in</MarginBottom>" +
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
        public ActionResult Chalan(string reportType, long saleId)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "ChalanReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT  Sales.AddedById as UserName,Sales.AddedById,SaleDetails.SaleDetailId, SaleDetails.SaleId, SaleDetails.ProductId, SaleDetails.QtyCase, SaleDetails.Quantity, Customers.CustomerName, Customers.AddressLine1, Products.ProductName, Sales.InvoiceId, Sales.SaleDate FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId where Sales.SaleId=" + saleId;

            //Fill the VFWPostManagers Set with the data
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                ReportDataSource rd = new ReportDataSource("ChalanDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>3in</PageWidth>" +
            "  <PageHeight>5.5in</PageHeight>" +
            "  <MarginTop>0.20in</MarginTop>" +
            "  <MarginLeft>0.0in</MarginLeft>" +
            "  <MarginRight>0.0in</MarginRight>" +
            "  <MarginBottom>0.20in</MarginBottom>" +
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
        [HttpPost]
        public ActionResult InvoiceSearch(string invoice_id)
        {
            string reportType = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "SalePrintReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT  Sales.AddedById as UserName,Sales.AddedById,Customers.CustomerName, Customers.AddressLine1,Customers.Phone, Products.ProductName, SaleDetails.Quantity, SaleDetails.TotalPrice, SaleDetails.FacePrice, Sales.InvoiceId, Sales.STAmount, Sales.TSSD, Sales.TSVat, Sales.TransportCost,Sales.OtherCost, Sales.Discount,Sales.DiscountAmount, Sales.CashReceive, Sales.DueAmount, Sales.SaleDate, Sales.NetTotal, SaleDetails.QtyCase FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  where Sales.IsDeleted='" + false+"' AND Sales.InvoiceId=" + invoice_id;

            //Fill the VFWPostManagers Set with the data
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                ReportDataSource rd = new ReportDataSource("InvoiceDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>3in</PageWidth>" +
            "  <PageHeight>7in</PageHeight>" +
            "  <MarginTop>0.20in</MarginTop>" +
            "  <MarginLeft>0.0in</MarginLeft>" +
            "  <MarginRight>0.0in</MarginRight>" +
            "  <MarginBottom>0.10in</MarginBottom>" +
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
        public ActionResult GetpassSearch(string invoice_id)
        {
            string reportType = "pdf";
           
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "GetPassReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
           // string query2 = "SELECT AspNetUsers.UserName,Sales.AddedById,SaleDetails.SaleDetailId, SaleDetails.SaleId, SaleDetails.ProductId, SaleDetails.QtyCase, SaleDetails.Quantity, Customers.CustomerName, Customers.AddressLine1, Products.ProductName, Sales.InvoiceId,  Sales.AddedById,Sales.SaleDate FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId Inner JOIN AspNetUsers ON AspNetUsers.Id=Sales.AddedById where Sales.IsDeleted='" + false + "' AND Sales.InvoiceId="+ invoice_id;
            string query2 = "SELECT  Sales.AddedById as UserName,Sales.AddedById,SaleDetails.SaleDetailId, SaleDetails.SaleId, SaleDetails.ProductId, SaleDetails.QtyCase, SaleDetails.Quantity, Customers.CustomerName, Customers.AddressLine1, Products.ProductName, Sales.InvoiceId,  Sales.AddedById,Sales.SaleDate FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId where Sales.IsDeleted='" + false + "' AND Sales.InvoiceId=" + invoice_id;

            //Fill the VFWPostManagers Set with the data
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                ReportDataSource rd = new ReportDataSource("GetPassDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>3in</PageWidth>" +
            "  <PageHeight>5.5in</PageHeight>" +
            "  <MarginTop>0.20in</MarginTop>" +
            "  <MarginLeft>0.0in</MarginLeft>" +
            "  <MarginRight>0.0in</MarginRight>" +
            "  <MarginBottom>0.20in</MarginBottom>" +
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
        public ActionResult ChalanSearch(string invoice_id)
        {
            string reportType = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "ChalanReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable cm = new DataTable();
            string connString2 = ConfigurationManager.ConnectionStrings["ShantiPOS"].ConnectionString;
            string query2 = "SELECT  Sales.AddedById as UserName,Sales.AddedById,SaleDetails.SaleDetailId, SaleDetails.SaleId, SaleDetails.ProductId, SaleDetails.QtyCase, SaleDetails.Quantity, Customers.CustomerName, Customers.AddressLine1, Products.ProductName, Sales.InvoiceId, Sales.SaleDate FROM SaleDetails INNER JOIN Products ON SaleDetails.ProductId = Products.ProductId INNER JOIN Sales ON SaleDetails.SaleId = Sales.SaleId INNER JOIN Customers ON Sales.CustomerId = Customers.CustomerId  where Sales.IsDeleted='" + false + "' AND Sales.InvoiceId=" + invoice_id;

            //Fill the VFWPostManagers Set with the data
            using (SqlConnection conn2 = new SqlConnection(connString2))
            {
                SqlDataAdapter da2 = new SqlDataAdapter(query2, conn2);
                da2.Fill(cm);
                ReportDataSource rd = new ReportDataSource("ChalanDataSet", cm);
                lr.DataSources.Add(rd);
            }
            //string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>3in</PageWidth>" +
            "  <PageHeight>5.5in</PageHeight>" +
            "  <MarginTop>0.20in</MarginTop>" +
            "  <MarginLeft>0.0in</MarginLeft>" +
            "  <MarginRight>0.0in</MarginRight>" +
            "  <MarginBottom>0.20in</MarginBottom>" +
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
    }
}