namespace DTS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class WSIB
    {
        string[] locNames;
        public WSIB() => locNames = STREAM.GetLocNames().ToArray();
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required. Please fill it in. ")]
        public int Location { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date_Accident { get; set; }
        public string Employee_Initials { get; set; }
        public string Accident_Cause { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date_Duties { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date_Regular { get; set; }
        public int Lost_Days { get; set; }
        public int Modified_Days_Not_Shadowed { get; set; }
        public int Modified_Days_Shadowed { get; set; }
        public string Form_7 { get; set; }
        public override string ToString()
        {
            return $"{locNames[Location - 1]},{Date_Accident},{Employee_Initials},{Accident_Cause},{Date_Duties},{Date_Regular},{Lost_Days},{Modified_Days_Not_Shadowed}," +
                $"{Modified_Days_Shadowed},{Form_7}";
        }
    }
}