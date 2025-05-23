

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(); // ? Add session support

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<OJPDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("con")));


var app = builder.Build();

app.UseSession(); // ? Use session

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