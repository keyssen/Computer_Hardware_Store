using HardwareShopBusinessLogic.BusinessLogics;
using HardwareShopBusinessLogic.BusinessLogics.Storekeeper;
using HardwareShopBusinessLogic.MailWorker;
using HardwareShopBusinessLogic.OfficePackage;
using HardwareShopBusinessLogic.OfficePackage.Implements;
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BuisnessLogicsContracts;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.StoragesContracts;
using HardwareShopDatabaseImplement.Implements;
using HardwareShopDatabaseImplement.Implements.Storekeeper;
using HardwareShopDatabaseImplement.Implements.Worker;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddLog4Net("log4net.config");

// Add services to the container.
builder.Services.AddTransient<IUserStorage, UserStorage>();
builder.Services.AddTransient<IBuildStorage, BuildStorage>();
builder.Services.AddTransient<ICommentStorage, CommentStorage>();
builder.Services.AddTransient<IComponentStorage, ComponentStorage>();
builder.Services.AddTransient<IGoodStorage, GoodStorage>();
builder.Services.AddTransient<IOrderStorage, OrderStorage>();
builder.Services.AddTransient<IPurchaseStorage, PurchaseStorage>();

builder.Services.AddTransient<IUserLogic, UserLogic>();
builder.Services.AddTransient<IBuildLogic, BuildLogic>();
builder.Services.AddTransient<ICommentLogic, CommentLogic>();
builder.Services.AddTransient<IComponentLogic, ComponentLogic>();
builder.Services.AddTransient<IGoodLogic, GoodLogic>();
builder.Services.AddTransient<IOrderLogic, OrderLogic>();
builder.Services.AddTransient<IPurchaseLogic, PurchaseLogic>();
builder.Services.AddTransient<IReportStorekeeperLogic, ReportStorekeeperLogic>();
builder.Services.AddTransient<IWorkerReportLogic, WorkerReportLogic>();
builder.Services.AddTransient<AbstractSaveToWord, SaveToWord>();
builder.Services.AddTransient<AbstractSaveToExcel, SaveToExcel>();

builder.Services.AddTransient<AbstractSaveToPdf, SaveToPdf>();
builder.Services.AddSingleton<AbstractMailWorker, MailKitWorker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HardwareShopRestApi", Version = "v1" });
});

var app = builder.Build();

var mailSender = app.Services.GetService<AbstractMailWorker>();
mailSender?.MailConfig(new MailConfigBindingModel
{
    MailLogin = builder.Configuration?.GetSection("MailLogin")?.Value?.ToString() ?? string.Empty,
    MailPassword = builder.Configuration?.GetSection("MailPassword")?.Value?.ToString() ?? string.Empty,
    SmtpClientHost = builder.Configuration?.GetSection("SmtpClientHost")?.Value?.ToString() ?? string.Empty,
    SmtpClientPort = Convert.ToInt32(builder.Configuration?.GetSection("SmtpClientPort")?.Value?.ToString()),
    PopHost = builder.Configuration?.GetSection("PopHost")?.Value?.ToString() ?? string.Empty,
    PopPort = Convert.ToInt32(builder.Configuration?.GetSection("PopPort")?.Value?.ToString())
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
