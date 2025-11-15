using Invoice.Business;
using Invoice.DataAccess.DataAccessBase;
using Invoice.DataAccess.DataAccessBase.RepositoryDp;
using Invoice.Entites;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddScoped<IRepository<InvoiceStatusLog>, InvoiceStatusLogDataAccessor>();
builder.Services.AddScoped<IMockInvoiceService, MockInvoiceService>();
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
