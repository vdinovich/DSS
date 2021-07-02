namespace DTS.Models
{
    public class ComplaintsSummaryAll
    {
        public int Id { get; set; }
        public int DateReceived { get; set; }
        public int Location { get; set; }
        public int WritenOrVerbal { get; set; }
        public int Receive_Directly { get; set; }
        public int FromResident { get; set; }
        public int ResidentName { get; set; }
        public int Department { get; set; }
        public int BriefDescription { get; set; }
        public int IsAdministration { get; set; }
        public int CareServices { get; set; }
        public int PalliativeCare { get; set; }
        public int Dietary { get; set; }
        public int Housekeeping { get; set; }
        public int Laundry { get; set; }
        public int Maintenance { get; set; }
        public int Programs { get; set; }
        public int Physician { get; set; }
        public int Beautician { get; set; }
        public int FootCare { get; set; }
        public int DentalCare { get; set; }
        public int Physio { get; set; }
        public int Other { get; set; }
        public int MOHLTCNotified { get; set; }
        public int CopyToVP { get; set; }
        public int ResponseSent { get; set; }
        public int ActionToken { get; set; }
        public int Resolved { get; set; }
        public int MinistryVisit { get; set; }
        public int LocationName { get; set; }
    }
}