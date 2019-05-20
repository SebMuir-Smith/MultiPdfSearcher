using System;
using System.IO;
using System.Text.RegularExpressions;
using BitMiracle.Docotic.Pdf;

namespace PdfParser2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: Please enter 2 command line arguments, you entered:");
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine("\n {0}", args[i]);
                }

                return;
            }
            
            // Path of pdf folder, relative from where it is being run from
            string path = args[0];

            // Regex to search for 
            string expression = args[1];
            
            Regex regex = new Regex(expression);

            PdfPage page;
            string pageText;
            string filePath;
            string fileName;
            // For each pdf in folder
            for (int docN = 0; docN < Directory.GetFiles(path).Length; docN++)
            {
                filePath = Directory.GetFiles(path)[docN];
                fileName = Path.GetFileName(filePath);
                
                // Skip any hidden files
                if (fileName[0] == '.')
                {
                    continue;
                }
                
                // Open pdf safely
                try
                {
                    using (PdfDocument pdf = new PdfDocument(fileName))
                    {
                        // For each page in pdf
                        for (int pageN = 0; pageN < pdf.PageCount; pageN++)
                        {
                            page = pdf.GetPage(pageN);

                            pageText = page.GetText();

                            // For each match on page
                            foreach (Match match in regex.Matches(pageText))
                            {
                                Console.WriteLine("Word was found on page {0} in document {1}, ({2})",
                                    pageN + 1, docN + 1, fileName);
                            }
                        }
                    }
                }
                catch (BitMiracle.Docotic.Pdf.UnexpectedStructureException)
                {
                    Console.WriteLine("Sorry, document {0} ({1}) was not recognised", docN + 1, fileName);
                }
            }           
        }
    }
}