using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfTool
{
    public class PdfEditor : IPdfEditor
    {
        protected static bool IsInitialized = false;

        public PdfEditor()
        {
            Initialize();
        }

        public void Slice(string outputPath, string inputPath, string indexes)
        {
            if(Regex.IsMatch(indexes, @"\d*-\d*"))
            {
                var splitIndexes = indexes.Split('-').Select(x => Convert.ToInt32(x)).ToArray();
                var startIndex = splitIndexes[0];
                var endIndex = splitIndexes[1];

                Slice(outputPath, inputPath, startIndex, endIndex);
            }
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
        public void MergeNPagesOnOne(string outputPath, params string[] filePaths)
        {
            double totalWidth = 0, totalHeight = 0;

            using var output = new PdfDocument(outputPath);

            var outputPage = output.AddPage();

            foreach (string path in filePaths)
            {
                using var input = OpenAsImportPdfDocument(path);

                var firstPage = input.Pages[0];
                totalWidth += firstPage.Width.Millimeter;

                if(firstPage.Height.Millimeter > totalHeight)
                {
                    totalHeight = firstPage.Height.Millimeter;
                }

                outputPage.Width = XUnit.FromMillimeter(totalWidth);
                outputPage.Height = XUnit.FromMillimeter(totalHeight);

                var g = XGraphics.FromPdfPage(outputPage);
                g.DrawImage(XImage.FromFile(path), totalWidth - firstPage.Width.Millimeter, 0);
                g.Dispose();
            }           

            output.Close();
        }
        public void MergeDirectory(string outputPath, string directoryPath)
        {
            var filePaths = Directory.GetFiles(directoryPath, "*.pdf");

            Merge(outputPath, filePaths);
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
