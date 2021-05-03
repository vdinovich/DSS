namespace DTS.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Community_Risks
    {
        string[] locNames;
        public Community_Risks() => locNames = STREAM.GetLocNames().ToArray();
        public int Id { get; set;}
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public int Location { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public string Type_Of_Risk { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public string Descriptions { get; set; }
        public string Potential_Risk { get; set; }
        public string MOH_Visit { get; set; }
        public string Risk_Legal_Action { get; set; }
        public string Hot_Alert { get; set; }
        public string Status_Update { get; set; }
        public string Resolved { get; set; }
        public override string ToString()
        {
            return $"{Date},{locNames[Location - 1]},{Type_Of_Risk},{Descriptions},{Potential_Risk}," +
                        $"{MOH_Visit},{Risk_Legal_Action},{Hot_Alert},{Status_Update},{Resolved}";
        }
    }
}