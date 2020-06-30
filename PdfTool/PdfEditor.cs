using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Text;

namespace PdfTool
{
    public class PdfEditor : IPdfEditor
    {
        protected static bool IsInitialized = false;

        public PdfEditor()
        {
            Initialize();
        }

        public void Slice(string outputPath, string inputPath, int startIndex, int endIndex, bool isZeroBased = false)
        {
            using var input = OpenAsImportPdfDocument(inputPath);
            
            var output = new PdfDocument();

            int realStartIndex = isZeroBased ? startIndex : startIndex - 1;
            int realEndIndex = isZeroBased ? endIndex : endIndex - 1;

            if(realEndIndex > input.PageCount)
            {
                throw new ArgumentException("endIndex exceeds page count");
            }

            for (int i = realStartIndex; i <= realEndIndex; i++)
            {
                output.AddPage(input.Pages[i]);
            }

            output.Save(outputPath);            
        }
        public void Merge(string outputPath, params string[] filePaths)
        {
            using var output = new PdfDocument(outputPath);            
            foreach (string path in filePaths)
            {
                using var input = OpenAsImportPdfDocument(path);                
                for (int i = 0; i < input.PageCount; i++)
                {
                    output.AddPage(input.Pages[i]);
                }                
            }
            output.Close();    
        }

        protected static void Initialize()
        {
            if (!IsInitialized)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }
        }

        private PdfDocument OpenAsImportPdfDocument(string path)
        {
            return PdfReader.Open(path, PdfDocumentOpenMode.Import);
        }        
    }
}
