var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddSession(options=>{
    options.IdleTimeout = TimeSpan.FromSeconds(1800);
});
builder.Services.AddSingleton<IDatabaseService,DatabaseService>();
builder.Services.AddSingleton<IAdminDataBaseServices,AdminDataBaseService>();
builder.Services.AddSingleton<ILibraryDataBaseService,LibraryDataBaseService>();
builder.Services.AddSingleton<ITransactionDatabaseService,TransactionDatabaseService>();
builder.Services.AddSingleton<IAuthDatabaseService,AuthDatabaseService>();
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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
