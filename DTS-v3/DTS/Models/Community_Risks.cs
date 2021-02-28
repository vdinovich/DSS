namespace DTS.Models
{
    public class Community_Risks
    {
        public int Id { get; set;}
        public System.DateTime Date { get; set; }
        public int Location { get; set; }
        public string Type_Of_Risk { get; set; }
        public string Descriptions { get; set; }
        public string Potential_Risk { get; set; }
        public string MOH_Visit { get; set; }
        public string Risk_Legal_Action { get; set; }
        public string Hot_Alert { get; set; }
        public string Status_Update { get; set; }
        public string Resolved { get; set; }
    }
}