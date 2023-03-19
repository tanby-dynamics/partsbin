using Blazored.Modal;
using Blazored.Toast;
using partsbin.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
builder.Services
    .AddSingleton<IDbFactory>(new DbFactory(builder.Environment.IsProduction()))
    .AddSingleton<IPartService, PartService>()
    .AddSingleton<IPartSearchService, PartSearchService>()
    .AddSingleton<IPartFieldService, PartFieldService>()
    .AddSingleton<ISearchFactory>(new SearchFactory(builder.Environment.IsProduction()))
    .AddScoped<IPartUiService, PartUiService>()
    .AddScoped<INavService, NavService>()
    .AddScoped<ISupplierUiService, SupplierUiService>()
    .AddScoped<ISelectStringUiService, SelectStringUiService>()
    .AddScoped<ISupplierService, SupplierService>()
    .AddScoped<IPartDocumentUiService, PartDocumentUiService>()
    .AddSingleton<IRuntimeConfigService, RuntimeConfigService>()
    .AddSingleton<IRuntimeConfigService, RuntimeConfigService>()
    .AddScoped<IRuntimeConfigUiService, RuntimeConfigUiService>()
    .AddSingleton<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

