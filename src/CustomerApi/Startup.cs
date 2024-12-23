using System;
using CustomerApi.Dto;
using CustomerApi.Interfaces;
using CustomerEntities;
using Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CustomerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<AccountOwnerClientMappingResolver>();
            ConfigureBusinessServices(services);
            services.AddDbContext<CustomerDbContext>(options =>
            {
                var connstring = Configuration.GetConnectionString("CustomerDB");
                options.UseMySql(connstring, ServerVersion.AutoDetect(connstring));
            }, ServiceLifetime.Scoped);

            services.AddValidatorsFromAssemblyContaining<CreateClient>();
            services.AddValidatorsFromAssemblyContaining<UpdateClient>();
            services.AddValidatorsFromAssemblyContaining<CreateAccountOwner>();
            services.AddValidatorsFromAssemblyContaining<UpdateAccountOwner>();
            services.AddValidatorsFromAssemblyContaining<CreateAccount>();
            services.AddValidatorsFromAssemblyContaining<UpdateAccount>();
            services.AddFluentValidationAutoValidation();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomersApi", Version = "v1" });
            });
            services.AddHttpResolvers();

        }

        private static void ConfigureBusinessServices(IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountOwnerService, AccountOwnerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerApi v1"));
            }
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
