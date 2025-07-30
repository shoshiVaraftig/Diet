using DietWeb.Core.Repositories;
using DietWeb.Core.Services;
using DietWeb.Data;
using DietWeb.Data.Repositories;
using DietWeb.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer; // ���� �� ��
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // ���� �� ��
using System.Text; // ���� �� ��

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// *** ���� �� ��: ����� ������� CORS ***
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()//WithOrigins("http://localhost:3000", "https://localhost:5173", "http://localhost:5173") // *** ��� ������ �� ������� ���! ***
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

builder.Services.AddScoped<IAuthService, AuthService>(); // *** ����� ���� �� ***

//builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

// *** ����� JWT Authentication ***
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

builder.Services.AddAuthorization(); // *** ����� ���� ��, ���� ������ �- [Authorize] ***


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// *** ���� ��� ���� ������ ��� ***
app.UseRouting(); // <-- ���� ���� ���!
app.UseCors("AllowAnyOrigin"); // <-- ���� ���� ���!
app.UseAuthentication(); // *** ���� ���� ���� UseAuthorization ***

app.UseAuthorization();
app.MapControllers();
// ******************************

app.Run();




