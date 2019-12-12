using System;
using System.Collections.Generic;
using System.Text;

namespace SARH.Core.PdfCreator.Interface
{
    public interface IConfigPdf
    {
        string ImgPathPdf { get; set; }
        string FontPathPdf { get; set; }
        string FontPathBarCode { get; set; }
    }
}
