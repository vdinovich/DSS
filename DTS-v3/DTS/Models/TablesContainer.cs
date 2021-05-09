namespace DTS.Models
{
    using System.Collections.Generic;
    /// <summary>
    /// List all tables in Database:
    /// </summary>
    public class TablesContainer
    {
        public static int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0, c6 = 0, c7 = 0, c8 = 0, c9 = 0, c10 = 0, c11 = 0, c12 = 0, c13 = 0, c14 = 0, c15 = 0, COUNT;
        public static List<int> count_arr = new List<int>();
        public static List<Critical_Incidents> list1;
        public static List<Complaint> list2;
        public static List<Good_News> list3;
        public static List<Emergency_Prep> list4;
        public static List<Community_Risks> list5;
        public static List<Visits_Others> list6;
        public static List<Privacy_Breaches> list7;
        public static List<Privacy_Complaints> list8;
        public static List<Education> list9;
        public static List<Labour_Relations> list10;
        public static List<Immunization> list11;
        public static List<Outbreaks> list12;
        public static List<WSIB> list13;
        public static List<Not_WSIBs> list14;
    }
}