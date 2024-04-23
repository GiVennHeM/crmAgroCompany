namespace API.dbContext 
{

        public partial class SampleDBContext : DbContext
        {

        public SampleDBContext(DbContextOptions
            <SampleDBContext> options)
                : base(options)
            {
            }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Customer>(entity => {
                    entity.HasKey(k => k.Id);
                });
                OnModelCreatingPartial(modelBuilder);
            }
            partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LoggerUser> LoggerUsers { get; set; }
    }

       
}
