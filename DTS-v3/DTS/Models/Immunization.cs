using System.ComponentModel.DataAnnotations;

namespace DTS.Models
{
    public class Immunization
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in!")]
        public int Location { get; set; }
        public string Numb_Res_Comm { get; set; }
        public string Numb_Res_Immun { get; set; }
        public string Numb_Res_Not_Immun { get; set; }
        public string Per_Res_Immun { get; set; }
        public string Per_Res_Not_Immun { get; set; }
    }
}