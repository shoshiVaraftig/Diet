using DietWeb.Core.Models;
using Microsoft.EntityFrameworkCore;
using System; 

namespace DietWeb.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Food> Foods { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ConversationMessage> Messages { get; set; }
        public object FavoriteRecipes { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=_db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // הגדרת קשר 1 ל־רבים בין User ל־DietaryPreference
            modelBuilder.Entity<User>()
                .HasMany(u => u.DietaryPreferences)
                .WithOne(dp => dp.User)
                .HasForeignKey(dp => dp.UserId)
                .OnDelete(DeleteBehavior.Cascade); // אפשר גם Restrict אם לא רוצים מחיקה אוטומטית
        }


        

    }
}