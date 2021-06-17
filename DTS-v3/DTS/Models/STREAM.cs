namespace DTS.Models
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    public class STREAM
    {
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
                    tw.WriteLine($"{doc[i].id},{doc[i].Date},{doc[i].CI_Form_Number},{doc[i].CI_Category_Type},{locNames[doc[i].Location - 1]},{doc[i].Brief_Description},{doc[i].MOH_Notified}," +
                        $"{doc[i].Police_Notified},{doc[i].POAS_Notified},{doc[i].Care_Plan_Updated}," +
                        $"{doc[i].Quality_Improvement_Actions},{doc[i].MOHLTC_Follow_Up}," +
                        $"{doc[i].CIS_Initiated},{doc[i].Follow_Up_Amendments},{doc[i].Risk_Locked},{doc[i].File_Complete}");
                }

                msg = "All found records within the specified data range were written into a file successfuly!";
            }
            return msg;
        }

        public string WriteTo2_CSV(List<Good_News> doc)
        {
            var msg = string.Empty;
            var size = doc.Count;
            string path = @"C:\Users\ldinovich-Admin\Documents\2. DSS - WOR Project\DSS (WOR Compliants) - Copy\DSS (WOR Compliants)\DTS-v3\good_news.csv";
            var locNames = new List<string>();
            var list = new MyContext().Care_Communities.ToList();
            foreach (var it in list)
                locNames.Add(it.Name.Split(new char[] { ' ' }).Last());
            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine($"Id,Location,DateNews,Category,Department,SourceCompliment,ReceivedFrom,Description_Complim,Respect,Passion," +
                        $"Teamwork,Responsibility,Growth,Compliment,Spot_Awards,Awards_Details,NameAwards,Awards_Received,Community_Inititives");
                for (int i = 0; i < size; i++)
                {
                    tw.WriteLine($"{doc[i].Id},{10099887766},{doc[i].DateNews},{doc[i].Category},{doc[i].Department},{doc[i].SourceCompliment}," +
                        $"{doc[i].ReceivedFrom},{doc[i].Description_Complim},{doc[i].Respect},{doc[i].Passion},{doc[i].Teamwork},{doc[i].Responsibility}," +
                        $"{doc[i].Growth},{doc[i].Compliment},{doc[i].Spot_Awards},{doc[i].Awards_Details},{doc[i].NameAwards},{doc[i].Awards_Received}," +
                        $"{doc[i].Community_Inititives}");
                }

                msg = "All found records within the specified data range were written into a file successfuly!";
            }
            return msg;
        }

        #region Test method:
        public void WriteToCSV(List<Good_News> doc)
        {
            var locNames = new List<string>();
            var list = new MyContext().Care_Communities.ToList();
            foreach (var it in list)
                locNames.Add(it.Name);

            string path = @"C:\Users\ldinovich-Admin\Documents\2. DSS - WOR Project\DSS (WOR Compliants) - Copy\DSS (WOR Compliants)\DTS-v3\new_csv.csv";

            System.Data.DataSet _result = new System.Data.DataSet();
            _result.Tables.Add("GoodNews");
            _result.Tables["GoodNews"].Columns.Add("Id");
            _result.Tables["GoodNews"].Columns.Add("Location");
            _result.Tables["GoodNews"].Columns.Add("DateNews");
            _result.Tables["GoodNews"].Columns.Add("Category");
            _result.Tables["GoodNews"].Columns.Add("Department");
            _result.Tables["GoodNews"].Columns.Add("SourceCompliment");
            _result.Tables["GoodNews"].Columns.Add("ReceivedFrom");
            _result.Tables["GoodNews"].Columns.Add("Description_Complim");
            _result.Tables["GoodNews"].Columns.Add("Respect");
            _result.Tables["GoodNews"].Columns.Add("Passion");
            _result.Tables["GoodNews"].Columns.Add("Teamwork");
            _result.Tables["GoodNews"].Columns.Add("Responsibility");
            _result.Tables["GoodNews"].Columns.Add("Growth");
            _result.Tables["GoodNews"].Columns.Add("Compliment");
            _result.Tables["GoodNews"].Columns.Add("Spot_Awards");
            _result.Tables["GoodNews"].Columns.Add("Awards_Details");
            _result.Tables["GoodNews"].Columns.Add("NameAwards");
            _result.Tables["GoodNews"].Columns.Add("Awards_Received");
            _result.Tables["GoodNews"].Columns.Add("Community_Inititives");

            System.Data.DataRow newRow = _result.Tables["GoodNews"].NewRow();
            newRow["Id"] = "1";
            newRow["Location"] = locNames[doc[0].Location - 1];

            _result.Tables["GoodNews"].Rows.Add(newRow);

            string fileName = "exportData";
            System.Data.DataTable data = _result.Tables[0];

            HttpContext context = HttpContext.Current;

            context.Response.Clear();
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".csv");


            // Write data
            TextWriter tw = new StreamWriter(path);
            foreach (System.Data.DataRow row in data.Rows)
            {
                foreach (System.Data.DataColumn col in data.Columns)
                {
                    tw.Write(row[col.ColumnName].ToString() + ",");
                    context.Response.Write(row[col.ColumnName].ToString() + ",");
                }
                tw.WriteLine(System.Environment.NewLine);
                context.Response.Write(System.Environment.NewLine);
            }
            tw.Close();
            context.Response.End();
        }
        #endregion

        #region Methods get Names of Location, CI_Category_Type:
        public static IEnumerable<string> GetLocNames()
        {
            var locNames = new List<string>();
            var list = new MyContext().Care_Communities.ToList();
            foreach (var it in list)
                locNames.Add(it.Name.Replace('\r', '\0').Replace('\n', '\0'));
            return locNames;
        }

        public static IEnumerable<string> GetCINames()
        {
            var ciNames = new List<string>();
            var list = new MyContext().CI_Category_Types.ToList();
            foreach (var it in list)
                ciNames.Add(it.Name.Replace('\r', '\0').Replace('\n', '\0'));
            return ciNames;
        }

        public static string GetLocNameById(int id)
        {
            if (id == 0) throw new System.ArgumentNullException();
            else
                return new MyContext().Care_Communities.Find(id).Name;
        }
        #endregion
    }
}