namespace DTS.Models
{
    using System;

    public class Visits_Others
    {
        public int Id { get; set; }
        public DateTime Date_of_Visit { get; set; }
        public string Agency { get; set; }
        public int Number_of_Findings { get; set; }
        public string Details_of_Findings { get; set; }
        public string Corrective_Actions { get; set; }
        public string Report_Posted { get; set; }
        public string LHIN_Letter_Received { get; set; }
        public string PH_Letter_Received { get; set; }
    }
}