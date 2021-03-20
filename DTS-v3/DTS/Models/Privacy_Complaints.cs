using System.ComponentModel.DataAnnotations;

namespace DTS.Models
{
    public class Privacy_Complaints
    {
        public int id { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public int Location { get; set; }
        public string Status { get; set; }
        public System.DateTime Date_Complain_Received { get; set; }
        public string Complain_Filed_By { get; set; }
        public string Type_of_Complaint { get; set; }
        public string Is_Complaint_Resolved { get; set; }
        public string Description_Outcome { get; set; }
    }
}