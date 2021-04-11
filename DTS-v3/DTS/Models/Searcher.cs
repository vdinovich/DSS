namespace DTS.Models
{
    public class Searcher
    {
        #region Method witch find model by name:
        public static object FindObjByName(string name)
        {
            switch (name)
            {
                case "Critical_Incidents":
                    return new Critical_Incidents();
                case "Complaint":
                    return new Complaint();
                case "Good_News":
                    return new Good_News();
                case "Emergency_Prep":
                    return new Emergency_Prep();
                case "Community_Risks":
                    return new Community_Risks();
                case "Visits_Others":
                    return new Visits_Others();
                case "Privacy_Breaches":
                    return new Privacy_Breaches();
                case "Privacy_Complaints":
                    return new Privacy_Complaints();
                case "Education":
                    return new Education();
                case "Labour_Relations":
                    return new Labour_Relations();
                case "Immunization":
                    return new Immunization();
                case "Outbreaks":
                    return new Outbreaks();
                case "WSIB":
                    return new WSIB();
                case "Not_WSIBs":
                    return new Not_WSIBs();
                default:
                    return "NoN";
            }
        }
        #endregion
    }
}