namespace DTS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Outbreaks
    {
        public int Id { get; set; }
        public DateTime Date_Declared { get; set; }
        public DateTime Date_Concluded { get; set; }
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
    }
}