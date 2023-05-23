using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Customers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Customers.WebApi.Util;

namespace Customers.WebApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      CurrentEnvironment = env;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment CurrentEnvironment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(
          "DevCorsPolicy",
          builder => builder.WithOrigins(
            "http://localhost:4200"//angular
          )
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());

        options.AddPolicy(
          "ProdCorsPolicy",
          builder => builder.WithOrigins(
            "https://urlProduction.com")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
      });

      services.AddSingleton(new ConnectionString(
        Configuration["ConnectionStrings:Customers"]));

      services.AddDbContext<SqlDbContext>(
        opt =>
        opt.UseSqlServer(Configuration["ConnectionStrings:Customers"],
        opciones =>
        opciones.MigrationsAssembly("Customers.Entities")));

      services.AddControllers();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customers-WebApi", Version = "v1" });
      });

      services.AddSingleton<Messages>();
      services.AddHandlers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseCors("DevCorsPolicy");
        app.UseRouting();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customers-WebApi-v1"));
      }
      else if (env.IsProduction())
      {
        app.UseCors("ProdCorsPolicy");
        app.UseRouting();
        app.UseAuthorization();
      }
      else
      {
        throw new ArgumentNullException("Environment");
      }

      app.UseMiddleware<ExceptionHandler>();

      app.UseHttpsRedirection();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }

    public class SqlDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<SqlDbContext>
    {
      public SqlDbContext CreateDbContext(string[] args)
      {
        var optionsBuilder = new DbContextOptionsBuilder<SqlDbContext>();
        return new SqlDbContext(optionsBuilder.Options);
      }
    }

  }
}

