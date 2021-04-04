namespace DTS.Models
{
    using DTS.Controllers;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class STREAM
    {
       // string fname = Path.Combine(Server.MapPath("~/Uploaded_Files/"));
        public string WriteTo_CSV(List<Critical_Incidents> doc)
        {
            var msg = string.Empty;
            var size = doc.Count;
            string path = @"C:\Users\ldinovich-Admin\Documents\2. DSS - WOR Project\DSS (WOR Compliants) - Copy\DSS (WOR Compliants)\DTS-v3\mycsv.csv";
            var locNames = new List<string>();
            var list = new MyContext().Care_Communities.ToList();
            foreach (var it in list)
                locNames.Add(it.Name);
            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine($"Id,Date,CI_Form_Number,CI_Category_Type,Location,Brief_Description,MOH_Notified,Police_Notified,POAS_Notified,Care_Plan_Updated," +
                        $"Quality_Improvement_Actions,MOHLTC_Follow_Up," +
                        $"CIS_Initiated,Follow_Up_Amendments,Risk_Locked,File_Complete");
                for (int i = 0; i < size; i++)
                {
                    tw.WriteLine($"{doc[i].id},{doc[i].Date},{doc[i].CI_Form_Number},{doc[i].CI_Category_Type},{locNames[doc[i].Location-1]},{doc[i].Brief_Description},{doc[i].MOH_Notified}," +
                        $"{doc[i].Police_Notified},{doc[i].POAS_Notified},{doc[i].Care_Plan_Updated}," +
                        $"{doc[i].Quality_Improvement_Actions},{doc[i].MOHLTC_Follow_Up}," +
                        $"{doc[i].CIS_Initiated},{doc[i].Follow_Up_Amendments},{doc[i].Risk_Locked},{doc[i].File_Complete}");
                }
    
                msg = "All found record have writed successfuly!";
            }
            return msg;
        }
    }
}