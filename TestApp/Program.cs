using Data.Core.Infrastructure;
using Data.Core.IRepository;
using Data.Core.Repository;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var configConnString = builder.Configuration.GetConnectionString("TestConnection");
builder.Services.AddDbContext<TestAppDbContext>(options => options.UseSqlServer(configConnString));

//Scoped serviciu poate fi folosit de n-ori - din mai multe parti de cate ori vreau 
builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();


// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
