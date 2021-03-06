namespace DTS.Models
{
    using System.Data.SqlClient;
    using System.Configuration;

    public class ADO_NET_CRUD
    {
        static SqlConnection conn = null;
        static SqlCommand cmd = null;
        static SqlDataReader dreader = null;

        public static string Insert_Incident(Critical_Incidents inc)
        {
            string msg = string.Empty;
            const string query = "" +
                "insert into Critical_Incidents" +
                "(Date, CI_Form_Number, CI_Category_Type, Location, Brief_Description, MOH_Notified, Police_Notified, POAS_Notified," +
                "Care_Plan_Updated, Quality_Improvement_Actions, MOHLTC_Follow_Up, CIS_Initiated, Follow_Up_Amendments, Risk_Locked," +
                "File_Complete) values" +
                "(@Date, @CI_Form_Number, @CI_Category_Type, @Location, @Brief_Description, @MOH_Notified, @Police_Notified, @POAS_Notified," +
                "@Care_Plan_Updated, @Quality_Improvement_Actions, @MOHLTC_Follow_Up, @CIS_Initiated, @Follow_Up_Amendments, @Risk_Locked," +
                "@File_Complete)";
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dssConnectionString"].ConnectionString))
            {
                conn.Open();
                using(cmd =new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", inc.Date);
                    cmd.Parameters.AddWithValue("@CI_Form_Number", inc.CI_Form_Number);
                    cmd.Parameters.AddWithValue("@CI_Category_Type", inc.CI_Category_Type);
                    cmd.Parameters.AddWithValue("@Location", inc.Location);
                    cmd.Parameters.AddWithValue("@Brief_Description", inc.Brief_Description);
                    cmd.Parameters.AddWithValue("@MOH_Notified", inc.MOH_Notified);
                    cmd.Parameters.AddWithValue("@Police_Notified", inc.Police_Notified);
                    cmd.Parameters.AddWithValue("@POAS_Notified", inc.POAS_Notified);
                    cmd.Parameters.AddWithValue("@Care_Plan_Updated", inc.Care_Plan_Updated);
                    cmd.Parameters.AddWithValue("@Quality_Improvement_Actions", inc.Quality_Improvement_Actions);
                    cmd.Parameters.AddWithValue("@MOHLTC_Follow_Up", inc.MOHLTC_Follow_Up);
                    cmd.Parameters.AddWithValue("@CIS_Initiated", inc.CIS_Initiated); 
                    cmd.Parameters.AddWithValue("@Follow_Up_Amendments", inc.Follow_Up_Amendments); 
                    cmd.Parameters.AddWithValue("@Risk_Locked", inc.Risk_Locked);
                    cmd.Parameters.AddWithValue("@File_Complete", inc.File_Complete);

                    int result = cmd.ExecuteNonQuery();
                    if (result == 1)
                        msg = "This record was written successfuly!";
                    else msg = "Something went wrong... Follow the stack trace";
                }
            }

            return msg;
        }
    }
}