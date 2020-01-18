namespace DTS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Care_Community
    {
        public int Id { get; private set; }
        [Required]
        public string Name { get; set; }
    }
}