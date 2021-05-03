using System.ComponentModel.DataAnnotations;

namespace DTS.Models
{
    using System;
    using System.Linq;

    public class Visits_Others
    {
        string[] locNames;
        public Visits_Others() => locNames = STREAM.GetLocNames().ToArray();
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public DateTime Date_of_Visit { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public int Location { get; set; }
        public string Agency { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public string Number_of_Findings { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public string Details_of_Findings { get; set; }
        public string Corrective_Actions { get; set; }
        public string Report_Posted { get; set; }
        public string LHIN_Letter_Received { get; set; }
        public string PH_Letter_Received { get; set; }
        public override string ToString()
        {
            return $"{Date_of_Visit},{locNames[Location - 1]},{Agency},{Number_of_Findings},{Details_of_Findings},{Corrective_Actions},{Report_Posted},{LHIN_Letter_Received},{PH_Letter_Received}";
        }
    }
}