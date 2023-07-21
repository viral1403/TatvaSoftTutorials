using BasicCRUD.DataAccess;
using BasicCRUD.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using BasicCRUD.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add application DB Context for EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
     , x => x.MigrationsAssembly("BasicCRUD.DataAccess") // change assesmbly ref as per project migrations
    ));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// add razor page dependencies & runtime compliation
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// build app from here
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
