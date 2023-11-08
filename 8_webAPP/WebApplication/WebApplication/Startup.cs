using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication.Middlewares;
using WebApplication.Models;
using WebApplication.Repositories.Implementations;
using WebApplication.Repositories.Interfaces;

namespace WebApplication
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
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,  // dla cwiczen, normalnie dawac true 
                    ClockSkew = TimeSpan.Zero,  // margines błędu
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Secret"]))
                };
            });
            
            services.AddDbContext<HospitalContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MssqlDbConnString"));
            });
            services.AddScoped<IDoctorDbRepository, DoctorDbRepository>();
            services.AddScoped<IPrescriptionDbRepository, PrescriptionDbRepository>();
            
            services.AddScoped<IAccountDbRepository, AccountDbRepository>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApplication", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication v1"));
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();
            // najpierw uwierzytelnienie, potem autoryzacja 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}