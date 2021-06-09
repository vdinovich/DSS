namespace DTS.Models
{
    using System.Data.Entity;

    // codefirst approach - entity framework(DbContext is a part of EntityFramework library)
    public class MyContext : DbContext  
    {
        public MyContext() : base("dssConnectionString") { }

        public virtual DbSet<Critical_Incidents> Critical_Incidents { get; set; }
        public virtual DbSet<Care_Community> Care_Communities { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Labour_Relations> Relations { get; set; }
        public virtual DbSet<Community_Risks> Community_Risks { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Good_News> Good_News { get; set; }
        public virtual DbSet<Sign_in_Main> Sign_in_Mains { get; set; }
        public virtual DbSet<Visits_Agency> Visits_Agencies { get; set; }
        public virtual DbSet<WSIB> WSIBs { get; set; }
        public virtual DbSet<Not_WSIBs> Not_WSIBs { get; set; }
        public virtual DbSet<Visits_Others> Visits_Others { get; set; }
        public virtual DbSet<Outbreaks> Outbreaks { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<CI_Category_Type> CI_Category_Types { get; set; }
        public virtual DbSet<Immunization> Immunizations { get; set; }
        public virtual DbSet<Privacy_Breaches> Privacy_Breaches { get; set; }
        public virtual DbSet<Privacy_Complaints> Privacy_Complaints { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Emergency_Prep> Emergency_Prep { get; set; }
        public virtual DbSet<Search_Word> Search_Words { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
    }
}
