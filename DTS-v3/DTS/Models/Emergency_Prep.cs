using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTS.Models
{
    public class Emergency_Prep
    {
        string[] locNames;
        public Emergency_Prep() => locNames = STREAM.GetLocNames().ToArray();
        public int Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage ="This is a required field. Please fill it in.")]
        public int Location { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public override string ToString() => $"{Name},{locNames[Location - 1]},{Jan},{Feb},{Mar},{Apr}," +
                $"{Mar},{Jun},{Jul},{Aug},{Sep},{Oct}," +
                $"{Nov},{Dec}";
    }
}