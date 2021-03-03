using System.ComponentModel.DataAnnotations;

namespace DTS.Models
{
    using System;

    public class Visits_Others
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public DateTime Date_of_Visit { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public int Location { get; set; }
        public string Agency { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public int Number_of_Findings { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public string Details_of_Findings { get; set; }
        public string Corrective_Actions { get; set; }
        public string Report_Posted { get; set; }
        public string LHIN_Letter_Received { get; set; }
        public string PH_Letter_Received { get; set; }
    }
}