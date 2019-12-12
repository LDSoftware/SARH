using iTextSharp.text;
using iTextSharp.text.pdf;
using SARH.Core.PdfCreator.FormatData;
using SARH.Core.PdfCreator.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SARH.Core.PdfCreator
{
    public class PdfManager
    {

        private IConfigPdf _pdfConfig;

        public PdfManager(IConfigPdf pdfConfig)
        {
            _pdfConfig = pdfConfig;
        }


        private Document CreatePDF(string pdfFile) 
        {
            var pdfDoc = new Document(PageSize.A4);
            var pdfFilePath = pdfFile;
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            PdfWriter.GetInstance(pdfDoc, fileStream);
            pdfDoc.AddAuthor("ISOSA SARH");
            pdfDoc.Open();
            return pdfDoc;
        }

        private Font BarCode() 
        {
            BaseFont fontBase = BaseFont.CreateFont(_pdfConfig.FontPathBarCode, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font f = new Font(fontBase);
            f.Size = 38;
            f.Color = BaseColor.Black;
            return f;
        }


        private Font HeaderFont() 
        {
            BaseFont fontBase = BaseFont.CreateFont(_pdfConfig.FontPathPdf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font f = new Font(fontBase);
            f.Size = 18;
            f.Color = BaseColor.White;
            return f;
        }

        private Font TitleFont()
        {
            BaseFont fontBase = BaseFont.CreateFont(_pdfConfig.FontPathPdf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font f = new Font(fontBase);
            f.Size = 11;
            f.Color = BaseColor.White;
            return f;
        }

        private Font DocumentFont()
        {
            BaseFont fontBase = BaseFont.CreateFont(_pdfConfig.FontPathPdf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(fontBase, 9, Font.NORMAL);
        }

        private void CreateHeaderSection(Document document, DocumentInfoPdfData pdfData) 
        {
            var p = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            p.SetWidths(new[] { 50, 50 });


            var cell = new PdfPCell(Image.GetInstance(File.ReadAllBytes(_pdfConfig.ImgPathPdf)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.Black;
            p.AddCell(cell);

            var fontH1 = HeaderFont();

            cell = new PdfPCell(new Phrase($"{pdfData.TitleDocumento} { Environment.NewLine } { pdfData.FormatId } { Environment.NewLine}", fontH1));
            cell.PaddingTop = 20;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BackgroundColor = new BaseColor(34, 31, 32);
            cell.PaddingBottom = 30;
            p.AddCell(cell);

            document.Add(p);
            document.Add(new Phrase(Environment.NewLine));
        }


        private void CreateInfoEmployeeSection(Document document, DocumentInfoPdfData pdfData)
        {

            var font = DocumentFont();
            var fontRev = TitleFont();

            var productsHeader = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cellProducts = new PdfPCell(new Phrase(pdfData.EmployeeInfoTitle, fontRev));
            cellProducts.HorizontalAlignment = Element.ALIGN_CENTER;
            cellProducts.BackgroundColor = BaseColor.Black;
            productsHeader.AddCell(cellProducts);
            document.Add(productsHeader);

            var productsTable = new PdfPTable(4)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            //

            productsTable.SetWidths(new[] { 13, 35, 17, 35 });

            var cellProductItem = new PdfPCell(new Phrase("No. empleado", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);


            cellProductItem = new PdfPCell(new Phrase("Nombre", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase("Puesto", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase("Area", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            var headerData = new PdfPCell(new Phrase(pdfData.EmployeeNumber, font));
            headerData.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(headerData);
            headerData = new PdfPCell(new Phrase(pdfData.EmployeeName, font));
            headerData.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(headerData);
            headerData = new PdfPCell(new Phrase(pdfData.JobTitle, font));
            headerData.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(headerData);
            headerData = new PdfPCell(new Phrase(pdfData.Area, font));
            headerData.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(headerData);

            document.Add(productsTable);
            document.Add(new Phrase(Environment.NewLine));
        }


        private void CreateDetailInfoSection(Document document, DocumentInfoPdfData pdfData) 
        {
            var font = DocumentFont();
            var fontRev = TitleFont();

            var p1 = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var c1 = new PdfPCell(new Phrase(pdfData.EmployeeInfoTitle, fontRev));
            c1.HorizontalAlignment = Element.ALIGN_CENTER;
            c1.BackgroundColor = BaseColor.Black;
            p1.AddCell(c1);
            document.Add(p1);

            pdfData.DocumentDetailInfo.ForEach((data) =>
            {
                var pfc = new PdfPTable(1)
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                    WidthPercentage = 100f
                };
                var cellitm = new PdfPCell(new Phrase(data, font));
                cellitm.HorizontalAlignment = Element.ALIGN_LEFT;
                pfc.AddCell(cellitm);
                document.Add(pfc);

            });
            document.Add(new Phrase(Environment.NewLine));
        }

        private void CreateObservationSection(Document document, DocumentInfoPdfData pdfData)
        {
            var font = DocumentFont();
            var fontRev = TitleFont();
            var p2 = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var c2 = new PdfPCell(new Phrase(pdfData.DocumentObservationsTitle, fontRev));
            c2.HorizontalAlignment = Element.ALIGN_CENTER;
            c2.BackgroundColor = BaseColor.Black;
            p2.AddCell(c2);
            document.Add(p2);


            var pt2 = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cell2 = new PdfPCell(new Phrase(pdfData.Comments, font));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.PaddingBottom = 40;
            pt2.AddCell(cell2);
            document.Add(pt2);

            document.Add(new Phrase(Environment.NewLine));
        }

        private void CreateSignSection(Document document, DocumentInfoPdfData pdfData) 
        {
            var font = DocumentFont();
            var fontRev = TitleFont();
            var fontCode = BarCode();
            var p3 = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var c3 = new PdfPCell(new Phrase(pdfData.SingEmployeeTitle, fontRev));
            c3.HorizontalAlignment = Element.ALIGN_CENTER;
            c3.BackgroundColor = BaseColor.Black;
            p3.AddCell(c3);
            document.Add(p3);

            var pt3 = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cell3 = new PdfPCell(new Phrase("", font));
            cell3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell3.PaddingBottom = 40;
            pt3.AddCell(cell3);
            document.Add(pt3);

            document.Add(new Phrase(Environment.NewLine));
        }

        private void CreateIdCodeBar(Document document, DocumentInfoPdfData pdfData) 
        {
            var fontCode = BarCode();
            var p3 = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var c3 = new PdfPCell(new Phrase(pdfData.IdDocument, fontCode));
            c3.Border = Rectangle.NO_BORDER;
            c3.HorizontalAlignment = Element.ALIGN_CENTER;
            p3.AddCell(c3);
            document.Add(p3);
        }

        public void CreateAssigedHardwareFormat(DocumentInfoPdfData pdfData, string pdfFile) 
        {
            var pdfDoc = new Document(PageSize.A4);
            var pdfFilePath = pdfFile;
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            PdfWriter.GetInstance(pdfDoc, fileStream);
            pdfDoc.AddAuthor("ISOSA SARH");
            pdfDoc.Open();

            CreateHeaderSection(pdfDoc, pdfData);

            CreateInfoEmployeeSection(pdfDoc, pdfData);

            CreateDetailInfoSection(pdfDoc, pdfData);

            CreateObservationSection(pdfDoc, pdfData);

            CreateSignSection(pdfDoc, pdfData);

            CreateIdCodeBar(pdfDoc, pdfData);

            pdfDoc.Close();
            fileStream.Dispose();
        }





        
    }
}
