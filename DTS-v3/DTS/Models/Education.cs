using System.Linq;

namespace DTS.Models
{
    public class Education
    {
        string[] locNames;
        public Education() => locNames = STREAM.GetLocNames().ToArray();
        public int Id{ get; set; }
        public string Session_Name { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "This field is empty. Please fill it in.")]
        public int Location { get; set; }
        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }
        public int Total_Numb_Educ { get; set; }
        public int Total_Numb_Eligible { get; set; }
        public int Approx_Per_Educated { get; set; }
        public override string ToString() => $"{Session_Name},{locNames[Location - 1]},{Jan},{Feb},{Mar},{Apr}," +
             $"{Mar},{Jun},{Jul},{Aug},{Sep},{Oct},{Nov},{Dec},{Total_Numb_Educ},{Total_Numb_Eligible},{Approx_Per_Educated}";
    }
}