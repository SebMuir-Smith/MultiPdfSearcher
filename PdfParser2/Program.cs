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
            
            // Path of pdf folder, relative from ~/
            string path = args[0];

            // Regex to search for 
            string expression = args[1];
            
            Regex regex = new Regex(expression);
            
            // For each pdf in folder
            for (int docN = 0; docN < Directory.GetFiles(path).Length; docN++)
            {
                // Open pdf
                using (PdfDocument pdf = new PdfDocument(Directory.GetFiles(path)[docN]))
                {
                    // For each page in pdf
                    for (int pageN = 0; pageN < pdf.PageCount; pageN++)
                    {
                        PdfPage page = pdf.GetPage(pageN);

                        string pageText = page.GetText();

                        // For each match on page
                        foreach (Match match in regex.Matches(pageText))
                        {
                            Console.WriteLine("Word was found on page {0} in document {1}, ({2})", 
                                pageN +1, docN + 1, Directory.GetFiles(path)[docN]);
                        }
                    }
                }
            }           
        }
    }
}