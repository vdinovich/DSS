namespace DTS.Models
{
    public class Position
    {
        public int Id { get; private set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
    }
}