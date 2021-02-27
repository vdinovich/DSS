namespace DTS.Models
{
    using System.Data.Entity;

    public partial class MyContext : DbContext  
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
    }
}
