using Blazored.Modal;
using Blazored.Toast;
using partsbin.Services;
using partsbin.UiServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<IDbFactory>(new DbFactory(builder.Environment.IsProduction()));
builder.Services.AddSingleton<IPartService, PartService>();
builder.Services.AddSingleton<IPartSearchService, PartSearchService>();
builder.Services.AddSingleton<IPartFieldService, PartFieldService>();
builder.Services.AddSingleton<ISearchFactory>(new SearchFactory(builder.Environment.IsProduction()));
builder.Services.AddScoped<IPartUiService, PartUiService>();

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

