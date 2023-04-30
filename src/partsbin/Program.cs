using Blazored.Modal;
using Blazored.Toast;
using partsbin.BusinessLogic.Services;
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
    .AddSingleton<IPartSearchService, PartSearchService>()
    .AddSingleton<IEquipmentSearchService, EquipmentSearchService>()
    .AddSingleton<IPartFieldService, PartFieldService>()
    .AddSingleton<IEquipmentFieldService, EquipmentFieldService>()
    .AddSingleton<ISearchFactory>(new SearchFactory(builder.Environment.IsProduction()))
    .AddSingleton<IRuntimeConfigService, RuntimeConfigService>()
    .AddSingleton<IImageService, ImageService>()
    .AddScoped<IFileService, FileService>()
    .AddScoped<IPartService, PartService>()
    .AddScoped<IEquipmentService, EquipmentService>()
    .AddScoped<IPartUiService, PartUiService>()
    .AddScoped<IEquipmentUiService, EquipmentUiService>()
    .AddScoped<ILocationUiService, LocationUiService>()
    .AddScoped<INavUiService, NavUiService>()
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

// This is meant to reindex everything on app init, however it's breaking the index at the moment, so I've left it commented out
// var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
// using (var scope = serviceScopeFactory.CreateScope())
// {
//     // Reindex parts
//     var partSearchService = scope.ServiceProvider.GetRequiredService<IPartSearchService>();
//     var partService = scope.ServiceProvider.GetRequiredService<IPartService>();
//     partSearchService.ClearIndex();
//     var parts = (await partService.GetAllParts()).ToArray();
//     foreach (var part in parts) partSearchService.IndexPart(part);
//     Console.WriteLine($"Reindexed {parts.Length} parts");

//     // Reindex equipment
//     var equipmentSearchService = scope.ServiceProvider.GetRequiredService<IEquipmentSearchService>();
//     var equipmentService = scope.ServiceProvider.GetRequiredService<IEquipmentService>();
//     equipmentSearchService.ClearIndex();
//     var equipment = (await equipmentService.GetAllEquipment()).ToArray();
//     foreach (var e in equipment) equipmentSearchService.IndexEquipment(e);
//     Console.WriteLine($"Reindexed {equipment.Length} equipment");
// }

app.Run();

