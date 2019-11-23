namespace DTS.Models
{
    using System.Data.Entity;

    public partial class MyContext : DbContext
    {
        public MyContext() : base("name=MyContext") { }

        public virtual DbSet<Critical_Incidents> Critical_Incidents { get; set; }
        public virtual DbSet<Care_Community> Care_Communities { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Labour_Relations> Relations { get; set; }
        public virtual DbSet<Community_Risks> Community_Risks { get; set; }
    }
}
