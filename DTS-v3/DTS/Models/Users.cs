namespace DTS.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Position { get; set; }
        public string Care_Community { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Enter the issued date.")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public System.DateTime Date { get; set; }
        public string Week { get; set; }
        public string User_Name { get; set; }
    }
}