using System.ComponentModel.DataAnnotations;

namespace DTS.Models
{
    public class Not_WSIBs
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required. Please fill it in.")]
        public int Location { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is required. Please fill it in.")]
        public System.DateTime Date_of_Incident { get; set; }
        public string Employee_Initials { get; set; }
        public string Position { get; set; }
        public string Time_of_Incident { get; set; }
        public string Shift { get; set; }
        public string Home_Area { get; set; }
        public string Injury_Related { get; set; }
        public string Type_of_Injury { get; set; }
        public string Details_of_Incident { get; set; }
    }
}