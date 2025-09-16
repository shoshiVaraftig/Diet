using DietWeb.Core.Repositories;
using DietWeb.Core.Services;
using DietWeb.Data;
using DietWeb.Data.Repositories;
using DietWeb.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer; // הוסף את זה
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // הוסף את זה
using OpenAI;
using System.Text; // הוסף את זה

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// *** הוסף את זה: הגדרת מדיניות CORS ***
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()//WithOrigins("http://localhost:3000", "https://localhost:5173", "http://localhost:5173") // *** שנה לכתובת של הקליינט שלך! ***
                         .AllowAnyHeader()
                         .AllowAnyMethod());
});



builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPersonalTrainerService, PersonalTrainerService>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IAuthService, AuthService>(); // *** הוספה חדשה זו ***

//builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

// *** הגדרת JWT Authentication ***
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var openAIApiKey = builder.Configuration["OpenAI:ApiKey"];
builder.Services.AddSingleton<OpenAIClient>(sp => new OpenAIClient(openAIApiKey));
builder.Services.AddScoped<IFoodService, FoodService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// *** ודאי שזה הסדר המדויק שלך ***
app.UseRouting(); // <-- חייב לבוא כאן!
app.UseCors("AllowAnyOrigin"); // <-- חייב לבוא כאן!
app.UseAuthentication(); // *** חייב לבוא לפני UseAuthorization ***

app.UseAuthorization();
app.MapControllers();
// ******************************

app.Run();




