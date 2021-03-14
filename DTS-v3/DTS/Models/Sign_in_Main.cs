namespace DTS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Sign_in_Main
    {
        public int Id { get; set; }
        public string Care_Community_Centre { get; set; }
        public string User_Name { get; set; }
        public string Position { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Current_Date { get; set; }
        public int Week { get; set; }
        public DateTime Date_Entred { get; set; }
    }
}