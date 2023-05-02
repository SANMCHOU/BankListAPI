using BankListAPI.VsCode.Configuration;
using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Introduce connectionString configuration & Dbcontext Service
var connectionString = builder.Configuration.GetConnectionString("BankListDbConnectionString");
builder.Services.AddDbContext<BankListDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//Introduce Indentity core
builder.Services.AddIdentityCore<ApiUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BankListDbContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

//Introduce Automapper Service so that it can be used globally.
builder.Services.AddAutoMapper(typeof(MapperConfig));

//Add Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountryRepository>();
builder.Services.AddScoped<IBanksRepository, BanksRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//invoke instance of buidler crx and lc- logger configuration

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
