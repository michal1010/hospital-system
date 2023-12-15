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

    public  void GeneratePdf(Dictionary<Doctor, Tuple<TimeSpan, int>> data, string outputPath, DateOnly start, DateOnly end)
    {
        using (var writer = new PdfWriter(outputPath))
        {
            // Create a PdfDocument object
            using (var pdf = new PdfDocument(writer))
            {
                // Create a Document object to add content
                var document = new Document(pdf);

                // Add content to the document from the IEnumerable collection
                document.Add(new Paragraph($"{start.ToString()} - {end.ToString()}"));
                foreach (KeyValuePair<Doctor, Tuple<TimeSpan, int>> kvp in data)
                {
                    int hours = (int)kvp.Value.Item1.TotalHours;
                    int minutes = (int)kvp.Value.Item1.TotalMinutes - hours * 60;
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(kvp.Key.ToString()).Append(": total time = ")
                                 .Append($"{hours} hours {minutes} minutes;")
                                 .Append(" number of appointments = ")
                                 .Append(kvp.Value.Item2).AppendLine();
                    document.Add(new Paragraph(stringBuilder.ToString()));
                }
            }
        }
    }

    public  void GenerateRaport(DateOnly start, DateOnly end, List<Doctor> doctors)
    {
        Dictionary<Doctor, Tuple<TimeSpan, int>> doctorSummary = new Dictionary<Doctor, Tuple<TimeSpan, int>>();
        foreach(Doctor doctor in doctors)
        {
            TimeSpan time = TimeSpan.Zero;
            int nubmerOfAppointments = 0;


            List<Appointment> appointments = doctor.Appointments.Where(a => a.StartTime >= start.ToDateTime(new TimeOnly(0, 0)) && 
                                                                            a.StartTime <= end.ToDateTime(new TimeOnly(0, 0))).ToList();
            foreach(Appointment appointment in appointments)
            {
                time += TimeSpan.FromMinutes(15);
                nubmerOfAppointments++;
            }
            doctorSummary[doctor] = Tuple.Create(time, nubmerOfAppointments);
        }

        GeneratePdf(doctorSummary, "raport.pdf", start, end);
    }

    //private List<Doc>
}
