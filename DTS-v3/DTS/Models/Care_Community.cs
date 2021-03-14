namespace DTS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Care_Community
    {
        public int Id { get; private set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public string Name { get; set; }
    }
}