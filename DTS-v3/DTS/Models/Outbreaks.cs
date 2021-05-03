namespace DTS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Outbreaks
    {
        string[] locNames;
        public Outbreaks() => locNames = STREAM.GetLocNames().ToArray();
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date_Declared { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date_Concluded { get; set; }
        public string Type_of_Outbreak { get; set; }
        public int Total_Days_Closed { get; set; }
        [Required(ErrorMessage = "This field is required! Please try to fill it in!")]
        public int Location { get; set; }
        public int Total_Residents_Affected { get; set; }
        public int Total_Staff_Affected { get; set; }
        public string Strain_Identified { get; set; }
        public int Deaths_Due { get; set; }
        public string CI_Report_Submitted { get; set; }
        public string Notify_MOL { get; set; }
        public int Credit_for_Lost_Days { get; set; }
        public string Tracking_Sheet_Completed { get; set; }
        public string Docs_Submitted_Finance { get; set; }
        public override string ToString()
        {
            return $"{Date_Declared},{Date_Concluded},{Type_of_Outbreak},{Total_Days_Closed},{locNames[Location - 1]},{Total_Residents_Affected},{Total_Residents_Affected}," +
                $"{Strain_Identified},{Deaths_Due}," +
                $"{CI_Report_Submitted},{Notify_MOL},{Credit_for_Lost_Days},{Tracking_Sheet_Completed},{Docs_Submitted_Finance}";
        }
    }
}