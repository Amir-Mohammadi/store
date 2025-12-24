using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Autofac;
using Core.Data;
using Core.Extensions;
using Core.Transaction;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers and swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(apiVersion: "v1");

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.AddAutofacDependencyServices();

    // TransactionManager depends on DbContext
    containerBuilder.RegisterType<TransactionManager>()
                    .As<ITransactionManager>()
                    .InstancePerLifetimeScope();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API v1");
    c.DocExpansion(DocExpansion.None);
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseTransactionsPerRequest();
app.UseStaticFiles();

app.MapGet("/", context =>
{
    context.Response.Redirect("/status.html");
    return Task.CompletedTask;
});

app.MapControllers();
app.Run();
