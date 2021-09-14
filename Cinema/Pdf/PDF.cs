
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cinema.Model;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Cinema.Pdf
{
 public class PDF
    {
        public static void CreatePDF(Movie movie,  string filename, string totalPrice ,List<Seat>seats)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            StringBuilder sb = new StringBuilder();
            foreach (var seat in seats)
            {
                sb.AppendLine(".............................................................................................");
                sb.AppendLine($"Seat:  {seat.Numbers}");                
            }
            string s = sb.ToString();
            document.Add(new Paragraph($@"
                                                                    {        movie.MovieName.ToUpper()   }
                                                 {movie.Location}
                                                           {movie.Date} | {movie.Time}
                             {s}
                             Total Price: {totalPrice}"));
           
            document.Close();
            writer.Close();
            fs.Close();
        }

    }
}
