using Microsoft.EntityFrameworkCore;
using ShopCartApi.Models;

namespace ShopCartApi.DataContext

    //inherits from the DbContextClass provided by Entity framework
{
    public class DataContextEF:DbContext
    {

        private readonly IConfiguration _config;

        public DbSet<User>? Users { get; set;  }
        public DbSet<Post> Posts { get; set;  }

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }


        //Called whenever we create an instance of DataContextEF
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                         optionsBuilder => optionsBuilder.EnableRetryOnFailure()
                    );
            }
           
        }


        // Maps our model to an actual table in the sqlServer
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("ShopCartAppSchema");
            //modelBuilder.Entity<User>()
            //.ToTable("User", "ShopCartAppSchema");
            //.ToTable("TableName", "SchemaName");
            //.HasKey("Primary Key for the Users Table")

            var user = modelBuilder.Entity<User>()
                .ToTable("Users");
            user.HasKey(k => k.UserId);
            user.HasMany(u => u.Posts)
                .WithOne(p => p.User);





            var post = modelBuilder.Entity<Post>()
                 .ToTable("Posts");

            post.HasKey(k => k.Id);

            post.HasOne(u => u.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
                
                

        }


    }
}
