using iTextSharp.text;
using iTextSharp.text.pdf;
using SARH.Core.PdfCreator.FormatData;
using SARH.Core.PdfCreator.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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

        private Font IncidenciaFont() 
        {
            BaseFont fontBase = BaseFont.CreateFont(_pdfConfig.FontPathPdf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font f = new Font(fontBase);
            f.Size = 18;
            f.Color = BaseColor.Black;
            return f;
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
            cell.BackgroundColor = new BaseColor(34, 31, 32);
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

        private void CreateInfoDataEmployeeSection(Document document, DocumentInfoPdfData pdfData)
        {
            var font = DocumentFont();
            var fontRev = TitleFont();

            var productsTable = new PdfPTable(3)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 45, 35, 20 });

            var p1 = new Paragraph($"   ID Empleado: {pdfData.EmployeInfo.EmployeeId}" +
               $"{Environment.NewLine}   Nombre: {pdfData.EmployeInfo.EmployeeName.ToUpper()}" +
               $"{Environment.NewLine}   RFC: {pdfData.EmployeInfo.Rfc}" +
               $"{Environment.NewLine}   CURP: {pdfData.EmployeInfo.Curp}" +
               $"{Environment.NewLine}   NSS: {pdfData.EmployeInfo.NSS}" +
               $"{Environment.NewLine}" +
               $"{Environment.NewLine}   Fecha de Nacimiento: {pdfData.EmployeInfo.BrithDate}", font);
            p1.SetLeading(4, 18);

            var cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = new BaseColor(231, 231, 232);
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Fecha de Alta: {pdfData.EmployeInfo.HireDate}" +
                $"{Environment.NewLine}   Fecha de Baja: {pdfData.EmployeInfo.FireDate}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = new BaseColor(231, 231, 232);
            productsTable.AddCell(cellProductItem);


            Byte[] bytes = Convert.FromBase64String(Regex.Replace(pdfData.EmployeInfo.PhotoPath, @"^data:image\/[a-zA-Z]+;base64,", string.Empty));

            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(bytes);
            image1.SetAbsolutePosition(140, 70);
            image1.ScalePercent(45f);

            cellProductItem = new PdfPCell(image1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);


            document.Add(productsTable);
        }

        private void CreatePersonalDataEmployeeSection(Document document, DocumentInfoPdfData pdfData)
        {
            var font = DocumentFont();
            var fontRev = TitleFont();


            document.Add(new Paragraph($"{Environment.NewLine}"));

            var productsHeader = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cellProducts = new PdfPCell(new Phrase("DATOS PERSONALES", fontRev));
            cellProducts.HorizontalAlignment = Element.ALIGN_CENTER;
            cellProducts.BackgroundColor = BaseColor.Black;
            productsHeader.AddCell(cellProducts);
            document.Add(productsHeader);

            var productsTable = new PdfPTable(3)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            //

            productsTable.SetWidths(new[] { 50, 20, 30 });

            var cellProductItem = new PdfPCell(new Phrase($"   Telefóno{Environment.NewLine}   {pdfData.EmployeInfo.PersonalPhone}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);


            cellProductItem = new PdfPCell(new Phrase($"   Celular{Environment.NewLine}   {pdfData.EmployeInfo.CellNumber}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Email{Environment.NewLine}   {pdfData.EmployeInfo.PersonalEmail}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);

            var productsTable2 = new PdfPTable(4)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            productsTable2.SetWidths(new[] { 50, 20, 15, 15 });


            var cellProductItem2 = new PdfPCell(new Phrase($"   Dirección{Environment.NewLine}   {pdfData.EmployeInfo.Address}", font));
            cellProductItem2.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable2.AddCell(cellProductItem2);


            cellProductItem2 = new PdfPCell(new Phrase($"   Ciudad{Environment.NewLine}   {pdfData.EmployeInfo.City}", font));
            cellProductItem2.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable2.AddCell(cellProductItem2);

            cellProductItem2 = new PdfPCell(new Phrase($"   Estado{Environment.NewLine}   {pdfData.EmployeInfo.State}", font));
            cellProductItem2.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable2.AddCell(cellProductItem2);

            cellProductItem2 = new PdfPCell(new Phrase($"   C.P.{Environment.NewLine}   {pdfData.EmployeInfo.Cp}", font));
            cellProductItem2.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable2.AddCell(cellProductItem2);

            document.Add(productsTable2);
        }


        private void CreateContactDataEmployeeSection(Document document, DocumentInfoPdfData pdfData)
        {
            var font = DocumentFont();
            var fontRev = TitleFont();


            document.Add(new Paragraph($"{Environment.NewLine}"));

            var productsHeader = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cellProducts = new PdfPCell(new Phrase("CONTACTO EMERGENCIA", fontRev));
            cellProducts.HorizontalAlignment = Element.ALIGN_CENTER;
            cellProducts.BackgroundColor = BaseColor.Black;
            productsHeader.AddCell(cellProducts);
            document.Add(productsHeader);

            var productsTable = new PdfPTable(3)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            //

            productsTable.SetWidths(new[] { 50, 20, 30 });

            var cellProductItem = new PdfPCell(new Phrase($"   Nombre Contacto{Environment.NewLine}   {pdfData.EmployeInfo.ContactName}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);


            cellProductItem = new PdfPCell(new Phrase($"   Relación{Environment.NewLine}   {pdfData.EmployeInfo.ContactRelation}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Teléfono{Environment.NewLine}   {pdfData.EmployeInfo.ContactPhone}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);
        }

        private void CreateJobDataEmployeeSection(Document document, DocumentInfoPdfData pdfData)
        {
            var font = DocumentFont();
            var fontRev = TitleFont();


            document.Add(new Paragraph($"{Environment.NewLine}"));

            var productsHeader = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cellProducts = new PdfPCell(new Phrase("PUESTO", fontRev));
            cellProducts.HorizontalAlignment = Element.ALIGN_CENTER;
            cellProducts.BackgroundColor = BaseColor.Black;
            productsHeader.AddCell(cellProducts);
            document.Add(productsHeader);

            var productsTable = new PdfPTable(6)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            //

            string stwork = string.Empty;
            string enwork = string.Empty;
            string stmeal = string.Empty;
            string enmeal = string.Empty;

            if (!string.IsNullOrEmpty(pdfData.EmployeInfo.StarWorkDay)) 
            {
                var job1 = pdfData.EmployeInfo.StarWorkDay.Split('-');
                stwork = job1[0];
                enwork = job1[1];
            }
            if (!string.IsNullOrEmpty(pdfData.EmployeInfo.StartMealDay))
            {
                var job2 = pdfData.EmployeInfo.StartMealDay.Split('-');
                stmeal = job2[0];
                enmeal = job2[1];

            }

            productsTable.SetWidths(new[] { 26, 18, 14, 14, 14, 14 });

            var cellProductItem = new PdfPCell(new Phrase($"   Puesto{Environment.NewLine}{pdfData.EmployeInfo.JobTitle}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);


            cellProductItem = new PdfPCell(new Phrase($"   Salario{Environment.NewLine}{pdfData.EmployeInfo.Salary}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Horario Inicio{Environment.NewLine}{stwork}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Horario Salida{Environment.NewLine}{enwork}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Inicio Comida{Environment.NewLine}{stmeal}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Fin Comida{Environment.NewLine}{enmeal}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);


            document.Add(productsTable);

            var productsTable2 = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            productsTable2.SetWidths(new[] { 44, 56 });


            var cellProductItem2 = new PdfPCell(new Phrase($"   Equipo en resguardo{Environment.NewLine}   {pdfData.EmployeInfo.AssignedEquipment}", font));
            cellProductItem2.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable2.AddCell(cellProductItem2);


            cellProductItem2 = new PdfPCell(new Phrase($"   Otros{Environment.NewLine}", font));
            cellProductItem2.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable2.AddCell(cellProductItem2);


            document.Add(productsTable2);
        }

        private void CreateHolidayEmployeeSection(Document document, DocumentInfoPdfData pdfData)
        {
            var font = DocumentFont();
            var fontRev = TitleFont();
            var fontInc = IncidenciaFont();


            document.Add(new Paragraph($"{Environment.NewLine}"));

            var productsHeader = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cellProducts = new PdfPCell(new Phrase("VACACIONES/PERMISOS/FALTAS", fontRev));
            cellProducts.HorizontalAlignment = Element.ALIGN_CENTER;
            cellProducts.BackgroundColor = BaseColor.Black;
            productsHeader.AddCell(cellProducts);
            document.Add(productsHeader);

            var productsTable = new PdfPTable(4)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 40, 20, 20, 20 });


            var p1 = new Paragraph($"   Vacaciones" +
               $"{Environment.NewLine}   Días correspondientes por año: 0" +
               $"{Environment.NewLine}   Días no gozados año anterior: 0" +
               $"{Environment.NewLine}   Total días por gozar: { pdfData.EmployeeVacations.TotalDias }" +
               $"{Environment.NewLine}   Días Gozados: {pdfData.EmployeeVacations.DiasTomados}" +
               $"{Environment.NewLine}   Días por gozar: {pdfData.EmployeeVacations.DiasDisponibles}" +
               $"{Environment.NewLine}", font);



            var cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);


            cellProductItem = new PdfPCell(new Phrase($"   Permisos{Environment.NewLine}{Environment.NewLine}00", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Retardos{Environment.NewLine}{Environment.NewLine}00", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"   Faltas{Environment.NewLine}{Environment.NewLine}00", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);
        }


        private void CreateDocumentEmployeeSection(Document document, DocumentInfoPdfData pdfData)
        {
            var font = DocumentFont();
            var fontRev = TitleFont();

            document.Add(new Paragraph($"{Environment.NewLine}"));

            var productsHeader = new PdfPTable(1)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            var cellProducts = new PdfPCell(new Phrase("Expendiente", fontRev));
            cellProducts.HorizontalAlignment = Element.ALIGN_CENTER;
            cellProducts.BackgroundColor = BaseColor.Black;
            productsHeader.AddCell(cellProducts);
            document.Add(productsHeader);

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 50, 50 });

            var cellProductItem = new PdfPCell(new Phrase($"   Documento               Última Actialización{Environment.NewLine}{pdfData.EmployeInfo.Documents}", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            cellProductItem = new PdfPCell(new Phrase($"", font));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);

            document.Add(new Paragraph($"{Environment.NewLine}"));
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

        private void CreateProfileJobTitleSectionA(Document document, DocumentInfoPdfData pdfData, string sectionName) 
        {
            var font = DocumentFont();
            var fontRev = HeaderFont();

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 30, 70 });

            var cellProductItem = new PdfPCell(new Phrase(sectionName, fontRev));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = BaseColor.LightGray;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);


            var p1 = new Paragraph($"   Nombre del puesto: {pdfData.EmployeProfile.NombrePuesto}" +
               $"{Environment.NewLine}   Área: {pdfData.EmployeProfile.Area}" +
               $"{Environment.NewLine}   Reporta a: " +
               $"{Environment.NewLine}   Supervisa a: " +
               $"{Environment.NewLine}   Tipo de funciones: " +
               $"{Environment.NewLine}" +
               $"{Environment.NewLine}   _____________________________________________________________________________", font);
            p1.SetLeading(4, 18);

            cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);


            document.Add(productsTable);
            document.Add(new Paragraph($"{Environment.NewLine}"));
        }

        private void CreateProfileJobTitleSectionB(Document document, DocumentInfoPdfData pdfData, string sectionName)
        {
            var font = DocumentFont();
            var fontRev = HeaderFont();

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 30, 70 });

            var cellProductItem = new PdfPCell(new Phrase(sectionName, fontRev));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = BaseColor.LightGray;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);


            var p1 = new Paragraph($"   Sexo: {pdfData.EmployeProfile.Sexo}" +
               $"{Environment.NewLine}   Estado civil: {pdfData.EmployeProfile.EstadoCivil}" +
               $"{Environment.NewLine}   Edad: {pdfData.EmployeProfile.Edad}" +
               $"{Environment.NewLine}   Escolaridad: {pdfData.EmployeProfile.Escolaridad}" +
               $"{Environment.NewLine}   Especialidad: {pdfData.EmployeProfile.Especialidad}" +
               $"{Environment.NewLine}   Habilidades Técnicas y Administrativas: " +
               $"{Environment.NewLine}   Experiencia Laboral: {pdfData.EmployeProfile.Experiencia}" +
               $"{Environment.NewLine}   _____________________________________________________________________________", font);
            p1.SetLeading(4, 18);

            cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);


            document.Add(productsTable);
            document.Add(new Paragraph($"{Environment.NewLine}"));
        }

        private void CreateProfileJobTitleSectionC(Document document, DocumentInfoPdfData pdfData, string sectionName)
        {
            var font = DocumentFont();
            var fontRev = HeaderFont();

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 30, 70 });

            var cellProductItem = new PdfPCell(new Phrase(sectionName, fontRev));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = BaseColor.LightGray;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);


            var p1 = new Paragraph($"{pdfData.EmployeProfile.RequerimientosOCondiciones}" +
               $"{Environment.NewLine}" +
               $"Horario de trabajo: {pdfData.EmployeProfile.Horario}" +
               $"{Environment.NewLine}   _____________________________________________________________________________", font);
            p1.SetLeading(4, 18);

            cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);


            document.Add(productsTable);
            document.Add(new Paragraph($"{Environment.NewLine}"));
        }

        private void CreateProfileJobTitleSectionD(Document document, DocumentInfoPdfData pdfData, string sectionName)
        {
            var font = DocumentFont();
            var fontRev = HeaderFont();

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 30, 70 });

            var cellProductItem = new PdfPCell(new Phrase(sectionName, fontRev));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = BaseColor.LightGray;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            var p1 = new Paragraph($"Internta{Environment.NewLine}" +
            $"{pdfData.EmployeProfile.ComunicacionInterna}" +
            $"{Environment.NewLine}" +
            $"Externa{Environment.NewLine}" +
            $"{pdfData.EmployeProfile.ComunicacionExterna}" +
            $"{Environment.NewLine}   _____________________________________________________________________________", font);
            p1.SetLeading(4, 18);

            cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);
            document.Add(new Paragraph($"{Environment.NewLine}"));
        }

        private void CreateProfileJobTitleSectionE(Document document, DocumentInfoPdfData pdfData, string sectionName)
        {
            var font = DocumentFont();
            var fontRev = HeaderFont();

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 30, 70 });

            var cellProductItem = new PdfPCell(new Phrase(sectionName, fontRev));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = BaseColor.LightGray;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            var p1 = new Paragraph($"{pdfData.EmployeProfile.ObjetivoGeneralPuesto}" +
            $"{Environment.NewLine}   _____________________________________________________________________________", font);
            p1.SetLeading(4, 18);

            cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);
            document.Add(new Paragraph($"{Environment.NewLine}"));
        }

        private void CreateProfileJobTitleSectionF(Document document, DocumentInfoPdfData pdfData, string sectionName)
        {
            var font = DocumentFont();
            var fontRev = HeaderFont();

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 30, 70 });

            var cellProductItem = new PdfPCell(new Phrase(sectionName, fontRev));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = BaseColor.LightGray;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            var p1 = new Paragraph($"{pdfData.EmployeProfile.ResponsabilidadPuesto}" +
            $"{Environment.NewLine}   _____________________________________________________________________________", font);
            p1.SetLeading(4, 18);

            cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);
            document.Add(new Paragraph($"{Environment.NewLine}"));
        }

        private void CreateProfileJobTitleSectionG(Document document, DocumentInfoPdfData pdfData, string sectionName)
        {
            var font = DocumentFont();
            var fontRev = HeaderFont();

            var productsTable = new PdfPTable(2)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            productsTable.SetWidths(new[] { 30, 70 });

            var cellProductItem = new PdfPCell(new Phrase(sectionName, fontRev));
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BackgroundColor = BaseColor.LightGray;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            var p1 = new Paragraph($"{pdfData.EmployeProfile.FuncionesActividades}" +
            $"{Environment.NewLine}   _____________________________________________________________________________", font);
            p1.SetLeading(4, 18);

            cellProductItem = new PdfPCell(p1);
            cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
            cellProductItem.BorderColor = BaseColor.White;
            productsTable.AddCell(cellProductItem);

            document.Add(productsTable);
            document.Add(new Paragraph($"{Environment.NewLine}"));
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

        public void CreateEmployeeProfile(DocumentInfoPdfData pdfData, string pdfFile) 
        {
            var pdfDoc = new Document(PageSize.A4);
            var pdfFilePath = pdfFile;
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            PdfWriter.GetInstance(pdfDoc, fileStream);
            pdfDoc.AddAuthor("ISOSA SARH");
            pdfDoc.Open();

            CreateHeaderSection(pdfDoc, pdfData);
            CreateInfoDataEmployeeSection(pdfDoc, pdfData);
            CreatePersonalDataEmployeeSection(pdfDoc, pdfData);
            CreateContactDataEmployeeSection(pdfDoc, pdfData);
            CreateJobDataEmployeeSection(pdfDoc, pdfData);
            CreateHolidayEmployeeSection(pdfDoc, pdfData);
            CreateDocumentEmployeeSection(pdfDoc, pdfData);
            CreateObservationSection(pdfDoc, pdfData);

            pdfDoc.Close();
            fileStream.Dispose();
        }

        public void CreateEmployeeProfileJobTitle(DocumentInfoPdfData pdfData, string pdfFile)
        {
            var pdfDoc = new Document(PageSize.A4);
            var pdfFilePath = pdfFile;
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            PdfWriter.GetInstance(pdfDoc, fileStream);
            pdfDoc.AddAuthor("ISOSA SARH");
            pdfDoc.Open();

            CreateHeaderSection(pdfDoc, pdfData);
            CreateProfileJobTitleSectionA(pdfDoc, pdfData, "Identifiación del puesto");
            CreateProfileJobTitleSectionB(pdfDoc, pdfData, "Perfil");
            CreateProfileJobTitleSectionC(pdfDoc, pdfData, "Requerimientos o condiciones especificas");
            CreateProfileJobTitleSectionD(pdfDoc, pdfData, "Comunicación");
            CreateProfileJobTitleSectionE(pdfDoc, pdfData, "Objetivo General del puesto");
            CreateProfileJobTitleSectionF(pdfDoc, pdfData, "Responsabilidad del puesto");
            CreateProfileJobTitleSectionG(pdfDoc, pdfData, "Funciones o actividades");

            pdfDoc.Close();
            fileStream.Dispose();
        }

        public void CreateIncidenciasReport(DocumentInfoPdfData pdfData, string pdfFile) 
        {
            var pdfDoc = new Document(PageSize.A4);
            var pdfFilePath = pdfFile;
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            PdfWriter.GetInstance(pdfDoc, fileStream);
            pdfDoc.AddAuthor("ISOSA SARH");
            pdfDoc.Open();

            CreateHeaderSection(pdfDoc, pdfData);




            pdfDoc.Close();
            fileStream.Dispose();
        }



    }
}
