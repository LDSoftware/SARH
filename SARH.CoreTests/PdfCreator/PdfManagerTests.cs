using Microsoft.VisualStudio.TestTools.UnitTesting;
using SARH.Core.PdfCreator;
using SARH.Core.PdfCreator.FormatData;
using SARH.Core.PdfCreator.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SARH.Core.PdfCreator.Tests
{
    [TestClass()]
    public class PdfManagerTests
    {
        [TestMethod()]
        public void CreateHardwareFormatTest()
        {

            //Arrange

            IConfigPdf pdfconfig = new ConfigPdf() 
            {
                FontPathPdf = @"C:\tmp\ISOSA-SARH\ISOSA\SARH\SARH.Core\PdfCreator\Font\TitilliumWeb-Regular.ttf",
                ImgPathPdf = @"C:\tmp\ISOSA-SARH\ISOSA\SARH\SARH.Core\PdfCreator\Images\F002.png",
                FontPathBarCode = @"C:\tmp\ISOSA-SARH\ISOSA\SARH\SARH.Core\PdfCreator\Font\BarcodeFont.ttf"
            };

            PdfManager pdfmanager = new PdfManager(pdfconfig);

            var _tt = "\t\t\t";
            var _ii = "Carateristicas \t Modelo 2012 Marca HP No Serie 123456789 ";
            var _rr = "\tFecha de entrega " + DateTime.Now.ToShortDateString();

            List<string> _list = new List<string>()
            {
                $"Celular {_tt} {_ii} {_rr}"
            };

            DocumentInfoPdfData data = new DocumentInfoPdfData()
            {
                EmployeeNumber = "00002",
                EmployeeName = "ROMAN GUTIERREZ GUTIERREZ",
                JobTitle = "AUXILIAR RH",
                Area = "R. HUMANOS",
                DocumentDetailInfo = _list,
                Comments = "Ninguno",
                TitleDocumento = "Equipo en Resguardo",
                FormatId = "F002",
                EmployeeInfoTitle = "DATOS PERSONALES",
                DetailDocument = "TECNOLOGíA DE LA INFORMACIÓN",
                DocumentObservationsTitle = "OBSERVACIONES",
                SingEmployeeTitle = "NOMBRE Y FIRMA DE RECIBIDO",
                IdDocument = $"0002F002{DateTime.Now.ToShortDateString()}-001"
            };


            //Act
            pdfmanager.CreateAssigedHardwareFormat(data, "Result.pdf");


            //Assert
            Assert.AreNotEqual(null, pdfmanager);
        }
    }
}