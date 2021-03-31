using System;
using System.IO;
using System.Linq;
using PdfTool;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // pagina’s 10, 11, 16, 17, 19, 20, (21 mag derin blijven), 24, 25
            var tool = new PdfEditor();

            string directory = @"C:\Users\micheldevelop\Desktop\offerte";


            string inputFile = Path.Combine(directory, "back_small.pdf");
            //string outputFile = Path.Combine(directory, "back_small_no_gfx.pdf");

            tool.Slice(Path.Combine(directory, "1.pdf"), inputFile, 1, 1);
            tool.Slice(Path.Combine(directory, "4-7.pdf"), inputFile, 4, 7);
            tool.Slice(Path.Combine(directory, "10.pdf"), inputFile, 10, 10);
            tool.Slice(Path.Combine(directory, "13-15.pdf"), inputFile, 13, 15);
            tool.Slice(Path.Combine(directory, "18-20.pdf"), inputFile, 18, 20);           
        }

    }
}
