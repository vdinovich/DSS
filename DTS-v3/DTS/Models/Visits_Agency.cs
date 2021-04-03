using System.ComponentModel.DataAnnotations;

namespace DTS.Models
{
    using System;

    public class Visits_Agency
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public DateTime Date_of_Visit { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public int Location { get; set; }
        public string Agency { get; set; }
        public string Findings_number { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public string Findings_Details { get; set; }
        public string Corrective_Actions { get; set; }
        public string Report_Posted { get; set; }
    }
}