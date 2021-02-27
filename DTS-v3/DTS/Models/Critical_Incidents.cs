using System.ComponentModel.DataAnnotations;

namespace DTS.Models
{
    public partial class Critical_Incidents
    {
        public int id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public System.DateTime? Date { get; set; }

        public string CI_Form_Number { get; set; }
        public string CI_Category_Type { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        public int? Location { get; set; }
        public string Brief_Description { get; set; }
        public string MOH_Notified { get; set; }
        public string Police_Notified { get; set; }
        public string POAS_Notified { get; set; }
        public string Care_Plan_Updated { get; set; }
        public string Quality_Improvement_Actions { get; set; }
        public string MOHLTC_Follow_Up { get; set; }
        public string CIS_Initiated { get; set; }
        public string Follow_Up_Amendments { get; set; }
        public string Risk_Locked { get; set; }
        public string File_Complete { get; set; }
    }
}
