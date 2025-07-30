using DietWeb.Core.Models;
using Microsoft.EntityFrameworkCore;
using System; // הוסף ייבוא עבור NotImplementedException אם לא קיים

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


        // ----------------------------------------------------
        // התיקון כאן: הסר את הפונקציה הזו וסמוך על היישום המובנה של DbContext.
        // או, אם אתה חייב override, קרא ליישום הבסיס.
        // ----------------------------------------------------

        // אם אתה משתמש ב-Entity Framework Core, ה-DbContext כבר מספק
        // יישום ל-SaveChangesAsync() באופן מובנה.
        // אין צורך ליישם אותו בעצמך, אלא אם כן אתה רוצה להוסיף לוגיקה מותאמת אישית
        // לפני או אחרי שמירה.

        // אם כן היית רוצה להוסיף לוגיקה מותאמת אישית, היית צריך להשתמש ב-override:
        // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     // כאן תוכל להוסיף לוגיקה מותאמת אישית לפני הקריאה ל-base
        //     var result = await base.SaveChangesAsync(cancellationToken);
        //     // כאן תוכל להוסיף לוגיקה מותאמת אישית אחרי הקרירה ל-base
        //     return result;
        // }

        // מכיוון שאתה רק זורק NotImplementedException, זה אומר שאין לך לוגיקה מותאמת אישית
        // שאתה רוצה ליישם כרגע.
        // לכן, הדרך הפשוטה והנכונה היא פשוט להסיר את כל הפונקציה הזו:
        /*
        internal async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
        */
        // אל תשכח להסיר גם את ה-using System; אם זה הייבוא היחיד שאתה משתמש בו עבורו.
    }
}