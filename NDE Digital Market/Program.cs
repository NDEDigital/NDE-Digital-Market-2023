using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using NDE_Digital_Market.Services.CompanyRegistrationServices;
using NDE_Digital_Market.Services.HK_GetsServices;
using NDE_Digital_Market.Data_Access_Layer;
using Microsoft.AspNetCore.Authentication.Cookies;
using NDE_Digital_Market.Services.AddBanner;
using NDE_Digital_Market.Services.WishListService;
using NDE_Digital_Market.Services.UserService;
using NDE_Digital_Market.Services.UnitService;
using NDE_Digital_Market.Services.TopSellerService;

var builder = WebApplication.CreateBuilder(args);

//CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
            //policy.WithOrigins("http://182.163.119.194:8776")
            .AllowAnyHeader()
            .AllowAnyMethod()
             .AllowCredentials(); // Allow sending credentials (cookies)
        });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//injecting user uservices
builder.Services.AddScoped<IHK_Gets, HK_Gets>();
builder.Services.AddScoped<HK_Gets_DAL>();

builder.Services.AddScoped<ICompanyRegistration, CompanyRegistration>();
builder.Services.AddScoped<CompanyRegistration_DAL>();

builder.Services.AddScoped<IAddBanner, Addbanners>();
builder.Services.AddScoped<AddBanner_DAL>();

builder.Services.AddScoped<IWishList_Service, WishList_Service>();
builder.Services.AddScoped<WishList_DAL>();

builder.Services.AddScoped<IUser_Service, User_Service>();
builder.Services.AddScoped<User_DAL>();

builder.Services.AddScoped<IUnit_Service, Unit_Service>();
builder.Services.AddScoped<Unit_DAL>();

builder.Services.AddScoped<ITopSeller_Service, TopSeller_Service>();
builder.Services.AddScoped<TopSeller_DAL>();
//builder.Services.AddScoped<NDE_Digital_Market.Controllers.UserController>();
//builder.Services.AddScoped<NDE_Digital_Market.Services.CompanyRegistrationServices.ICompanyRegistration, NDE_Digital_Market.Services.CompanyRegistrationServices.CompanyRegistration>();
//builder.Services.AddScoped<NDE_Digital_Market.Data_Access_Layer.CompanyRegistration_DAL>();




builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // cancels out the 5min  delay of library
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Try to get the token from the "accessToken" cookie
                if (context.Request.Cookies.ContainsKey("accessToken"))
                {
                    context.Token = context.Request.Cookies["accessToken"];
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CORS
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
