using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> sortedInputFiles = Directory.GetFiles("input", "*.*", SearchOption.AllDirectories).OrderBy(f => f).ToList();

            MergePdf(sortedInputFiles, "output.pdf");
        }

        public static void MergePdf(List<String> InFiles, String OutFile)
        {

            using (FileStream stream = new FileStream(OutFile, FileMode.Create))
            using (Document doc = new Document())
            using (PdfCopy pdf = new PdfCopy(doc, stream))
            {
                doc.Open();

                PdfReader reader = null;
                PdfImportedPage page = null;

                //fixed typo
                InFiles.ForEach(file =>
                {
                    reader = new PdfReader(file);

                    for (int i = 0; i < reader.NumberOfPages; i++)
                    {
                        page = pdf.GetImportedPage(reader, i + 1);
                        pdf.AddPage(page);
                    }

                    pdf.FreeReader(reader);
                    reader.Close();
                    File.Delete(file);
                });
            }
        }
    }
}
