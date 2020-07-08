using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Text;
using System.Globalization;
using PdfSharp.Pdf.AcroForms;

namespace pdf2
{
    class Program
    {
       static string savingPdfPath = "C:\\GeneratedPdf\\";
       static int maxMb=1, minMb=1,fileNumber=1;
       
        //generowanie losowanych znaków z dopuszczalnego zakresu w kodzie Ascii
        static string RandomTextGenerator(int dl)//do ukończenie
        {
            Random rand = new Random();
            string text = "";

            for (int i = 0; i < dl; i++)
                text += (char)((rand.Next() % 74) + 48);

            return text;
        }
        //generowanie nazwy faktury wg. zadananego formatu
        static string FileNameGenerator(int i)//do ukończenia
        {
            string type = "FV";
            int dl = Convert.ToString(i).Length;
            string number = "";
            if (dl > 6) Console.WriteLine("Za dużo plików");
            else
            {
                for (; dl < 6; dl++) number += '0';
                number += i;
            }
           
            int year = 20;//
            string month = "0" + 7;//1-12
            int period = 3;//1,2,3,4
            int clientNumber = 123456;//6cyfrowy
            string invoiceName = type + '-' + number + '-' + year + '-' + month + '-' + period + '-' + clientNumber;
            return invoiceName;
        }
        //metoda generująca pliki od 1 do 10 M
        static void NewDirectory()
        {
            string dir = @"C:\GeneratedPdf";
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        static void PdfGenerator(int i)
        {
            Random rand = new Random();
            int letterNumber = 100;
            int rowNumber = 100;
            int Mb = 75;
            int pageNumber = (rand.Next()%(maxMb-minMb) + minMb) * Mb;
            Console.WriteLine(pageNumber );
            string pdfname = FileNameGenerator(i) + ".pdf";
  
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument pdfdocument = new PdfDocument();
            pdfdocument.Info.Title = "randomtitle";
            PdfPage[] pdfpage = new PdfPage[pageNumber];
            for (int x = 0; x < pageNumber; x++)
            {
                pdfpage[x] = pdfdocument.AddPage();
                XGraphics graph = XGraphics.FromPdfPage(pdfpage[x]);
                XFont font = new XFont("Arial", 12, XFontStyle.Bold);
                for (int j = 0; j < rowNumber; j++)
                {
                    graph.DrawString(RandomTextGenerator(letterNumber), font, XBrushes.Black,
                       new XRect(0, j * 10, pdfpage[x].Width, pdfpage[x].Height),
                       XStringFormats.TopCenter);

                }

            }

            string pdffilename = savingPdfPath + pdfname;
            pdfdocument.Save(pdffilename);
            Console.WriteLine(pdfname + " wygenerowany prawidłowo");
           

        }

        static void LoopPdfGenerator()
        {
            for(int i = 0; i < fileNumber; i++)
            {
                PdfGenerator(i);
            }
        }
        //fragment odpowiada za sprawdzanie poprawności wpisanej maksmalnej wielkości plików;
        static void SetMaxMinSize()
        {
            bool b = false;
            do
            {
                Console.WriteLine("Podaj minimalną wielkość pliku: ");
                string min = Console.ReadLine();
                try
                {
                    minMb = Int32.Parse(min);
                }
                catch (FormatException)
                {
                    b = true;
                    Console.WriteLine($"Nie mozliwe do konwersji: ");
                }
            } while (b);
            b = false;
            do
            {
                Console.WriteLine("Podaj maksymalną wielkość pliku: ");
                string max = Console.ReadLine();
                try
                {
                    maxMb = Int32.Parse(max);
                }
                catch (FormatException)
                {
                    b = true;
                    Console.WriteLine($"Nie mozliwe do konwersji: ");
                }
            } while (b);
            
        }

        //fragment odpowiada za sprawdzanie poprawności wpisanej liczby
        static void SetFileNumber()
        {
            string fileNum;
     
            bool b = false;
            do
            {
                Console.WriteLine("Podaj liczbę plików do wygenerowania: ");
                fileNum = Console.ReadLine();
                try
                {
                    fileNumber = Int32.Parse(fileNum);
                }
                catch (FormatException)
                {
                    b = true;
                    Console.WriteLine($"Nie mozliwe do konwersji:");
                }
            } while (b);
        }
        
        static void Main(string[] args)
        {
            
            //fragment odpowiada za sprawdzanie poprawności wpisanej liczby
            
            SetFileNumber();
            SetMaxMinSize();
            NewDirectory();
            LoopPdfGenerator();

        }
    }
}
