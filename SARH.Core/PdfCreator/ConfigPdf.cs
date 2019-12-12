using SARH.Core.PdfCreator.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SARH.Core.PdfCreator
{
    public class ConfigPdf : IConfigPdf
    {
        public string ImgPathPdf { get; set; }
        public string FontPathPdf { get; set; }
        public string FontPathBarCode { get; set; }
    }
}
