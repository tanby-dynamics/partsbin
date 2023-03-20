using Blazored.Modal;
using Blazored.Toast;
using partsbin.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSwaggerGen();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
builder.Services.AddControllers();
builder.Services
    .AddSingleton<IDbFactory>(new DbFactory(builder.Environment.IsProduction()))
    .AddSingleton<IPartService, PartService>()
    .AddSingleton<IPartSearchService, PartSearchService>()
    .AddSingleton<IPartFieldService, PartFieldService>()
    .AddSingleton<ISearchFactory>(new SearchFactory(builder.Environment.IsProduction()))
    .AddSingleton<IRuntimeConfigService, RuntimeConfigService>()
    .AddSingleton<IRuntimeConfigService, RuntimeConfigService>()
    .AddSingleton<IImageService, ImageService>()
    .AddScoped<IFileService, FileService>()
    .AddScoped<IPartUiService, PartUiService>()
    .AddScoped<INavService, NavService>()
    .AddScoped<ISupplierUiService, SupplierUiService>()
    .AddScoped<ISelectStringUiService, SelectStringUiService>()
    .AddScoped<ISupplierService, SupplierService>()
    .AddScoped<IPartDocumentUiService, PartDocumentUiService>()
    .AddScoped<IRuntimeConfigUiService, RuntimeConfigUiService>()
    .AddScoped<IConfirmUiService, ConfirmUiService>();

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
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

