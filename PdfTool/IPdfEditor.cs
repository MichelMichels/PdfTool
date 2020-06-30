using System;
using System.Collections.Generic;
using System.Text;

namespace PdfTool
{
    public interface IPdfEditor
    {
        void Slice(string outputPath, string inputPath, int startIndex, int endIndex, bool isZeroBased = false);
        void Merge(string outputPath, params string[] inputPaths);
    }
}
