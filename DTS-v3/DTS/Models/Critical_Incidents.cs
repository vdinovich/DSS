namespace DTS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Critical_Incidents
    {
        public int id { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string CI_Form_Number { get; set; }

        public string CI_Category_Type { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        public string Brief_Description { get; set; }

        [StringLength(3)]
        public string MOH_Notified { get; set; }

        [StringLength(3)]
        public string Police_Notified { get; set; }

        [StringLength(3)]
        public string POAS_Notified { get; set; }

        [StringLength(3)]
        public string Care_Plan_Updated { get; set; }

        public string Quality_Improvement_Actions { get; set; }

        public string MOHLTC_Follow_Up { get; set; }

        [StringLength(50)]
        public string CIS_Initiated { get; set; }

        [StringLength(50)]
        public string Follow_Up_Amendments { get; set; }

        [StringLength(50)]
        public string Risk_Locked { get; set; }

        [StringLength(50)]
        public string File_Complete { get; set; }
    }
}
