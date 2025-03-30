using ParkyWeb.Repository;
using ParkyWeb.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpClient();
builder.Services.AddCors();

builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMvc().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();

builder.Services.AddSession(o =>
{

    o.IdleTimeout = TimeSpan.FromMinutes(60);
    o.Cookie.HttpOnly= true;
    o.Cookie.IsEssential= true;


});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");




app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(x => x
.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()

);

app.UseSession();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.Run();
