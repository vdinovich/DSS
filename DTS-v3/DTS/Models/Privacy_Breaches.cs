using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTS.Models
{
    public class Privacy_Breaches
    {
        string[] locNames;
        public Privacy_Breaches() => locNames = STREAM.GetLocNames().ToArray();
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]   
        [DataType(DataType.Text)]
        public int Location { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public System.DateTime Date_Breach_Occurred { get; set; }
        public string Description_Outcome { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime Date_Breach_Reported { get; set; }
        public string Date_Breach_Reported_By { get; set; }
        public string Type_of_Breach { get; set; }
        public string Type_of_PHI_Involved { get; set; }
        public int Number_of_Individuals_Affected { get; set; }
        public string Risk_Level { get; set; }
        public override string ToString()
        {
            return $"{locNames[Location - 1]},{Status},{Date_Breach_Occurred},{Description_Outcome},{Date_Breach_Reported},{Date_Breach_Reported_By},{Type_of_Breach},{Type_of_PHI_Involved},{Number_of_Individuals_Affected}" +
                $",{Risk_Level}";
        }
    }
}