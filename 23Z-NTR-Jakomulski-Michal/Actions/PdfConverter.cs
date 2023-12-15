using System.Collections.Generic;
using System.IO;
using System.Text;
using ClinicApp.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class PdfConverter
{
    public  void GeneratePdf(List<Appointment> data, string outputPath)
    {
        using (var writer = new PdfWriter(outputPath))
        {
            // Create a PdfDocument object
            using (var pdf = new PdfDocument(writer))
            {
                // Create a Document object to add content
                var document = new Document(pdf);

                // Add content to the document from the IEnumerable collection
                foreach (var item in data)
                {
                    document.Add(new Paragraph(item.ToString()));
                }
            }
        }
    }

    public  void GeneratePdf(Dictionary<Doctor, Tuple<TimeOnly, int>> data, string outputPath)
    {
        using (var writer = new PdfWriter(outputPath))
        {
            // Create a PdfDocument object
            using (var pdf = new PdfDocument(writer))
            {
                // Create a Document object to add content
                var document = new Document(pdf);

                // Add content to the document from the IEnumerable collection
                foreach (KeyValuePair<Doctor, Tuple<TimeOnly, int>> kvp in data)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(kvp.Key.ToString()).Append(": total time = ")
                                 .Append(kvp.Value.Item1.ToString(@"hh\:mm")).Append(" number of appointments = ")
                                 .Append(kvp.Value.Item2).AppendLine();
                    document.Add(new Paragraph(stringBuilder.ToString()));
                }
            }
        }
    }

    public  void GenerateRaport(DateTime start, DateTime end, List<Doctor> doctors)
    {
        Dictionary<Doctor, Tuple<TimeOnly, int>> doctorSummary = new Dictionary<Doctor, Tuple<TimeOnly, int>>();
        foreach(Doctor doctor in doctors)
        {
            TimeOnly time = new TimeOnly(0);
            int nubmerOfAppointments = 0;


            List<Appointment> appointments = doctor.Appointments.Where(a => a.StartTime >= start&& 
                                                                            a.StartTime <= end).ToList();
            foreach(Appointment appointment in appointments)
            {
                time.AddMinutes(15);
                nubmerOfAppointments++;
            }
            doctorSummary[doctor] = Tuple.Create(time, nubmerOfAppointments);
        }

        GeneratePdf(doctorSummary, "raport.pdf");
    }

    //private List<Doc>
}
