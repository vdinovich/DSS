namespace DTS.Models
{
    using System;

    public class Visits_Agency
    {
        public int Id { get; set; }
        public DateTime Date_of_Visit { get; set; }
        public string Agency { get; set; }
        public int Findings_number { get; set; }
        public string Findings_Details { get; set; }
        public string Corrective_Actions { get; set; }
        public string Report_Posted { get; set; }
    }
}