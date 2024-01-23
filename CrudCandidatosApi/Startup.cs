using CrudCandidatos.Application.Interfaces;
using CrudCandidatos.Application.Services;
using CrudCandidatos.Infrastructure.Data;
using CrudCandidatos.Infrastructure.Interfaces;
using CrudCandidatos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using static OpenQA.Selenium.VirtualAuth.VirtualAuthenticatorOptions;
using static System.Net.WebRequestMethods;

namespace CrudCandidatosApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost4200",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });


            // Injeção de dependência dos serviços e repositórios
            services.AddScoped<ICandidatoService, CandidatoService>();
            services.AddScoped<ICandidatoRepository, CandidatoRepository>();

            
            services.AddControllers();

            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            
            services.AddHostedService<DatabaseInitializer>();

            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Candidatos", Version = "v1" });
            });
        }


       

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Candidatos v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseCors("AllowLocalhost4200");

            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    
                    var context = services.GetRequiredService<AppDbContext>();

                    
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}