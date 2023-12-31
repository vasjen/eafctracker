using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Eafctracker.Data;
using Eafctracker.Services;
using Eafctracker.Services.Interfaces;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    ConfigurationManager config = builder.Configuration;
    NpgsqlConnectionStringBuilder conStrBuilder = new NpgsqlConnectionStringBuilder(
        config.GetConnectionString("DefaultConnection"));
    conStrBuilder.Password = config.GetValue<string>("db_password");
    string connection = conStrBuilder.ConnectionString;
    options.UseNpgsql(connection);
});
builder.Services.AddHttpClient("proxy",options =>
{
    string? apikey = builder.Configuration.GetValue<string>("Proxy:API");
    options.BaseAddress = new Uri($"https://proxy-seller.io/personal/api/v1/{apikey}/proxy/list/ipv4");
});
builder.Services.AddScoped<IHttpClientService,HttpClientService>();
builder.Services.AddScoped<IWebService,WebService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endp =>
    endp.MapControllers());
app.Run();