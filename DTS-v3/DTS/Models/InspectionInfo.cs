namespace DTS.Models
{
    using System;
    using System.Collections.Generic;

    public class InspectionInfo
    {
        public int Id { get; set; }
        public double InspectNumber { get; set; }
        public DateTime ReportDate { get; set; }
        public string TypeOfInspection { get; set; }
        public DateTime LastDateOfInpect { get; set; }
        public List<string> Home { get; set; }
     }
}