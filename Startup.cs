using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxLearn.Main.Data.Repositories;
using FoxLearn.Main.IData.Interfaces;
using FoxLearn.Models.Security;
using FoxLearn.SharedKernel.Factories;
using FoxLearn.SqlServer.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using VueCliMiddleware;

namespace FoxLearn
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
            //add swagger
            services.AddOpenAPI();

            services.AddControllers();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp"; // ClientApp/dist
            });

            services.AddDbContext<FoxLearnDbContext>
              (options =>
              {
                  options.UseSqlServer
                          (Configuration.GetConnectionString("DefaultConnection"));

              });

            services.AddIdentity<FLUser, FLRole>(identity =>
            {
                identity.Password.RequiredLength = 4;
                identity.Password.RequireNonAlphanumeric = false;
                identity.Password.RequireLowercase = false;
                identity.Password.RequireUppercase = false;
                identity.Password.RequireDigit = false;
            })
              .AddEntityFrameworkStores<FoxLearnDbContext>().AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var key = Encoding.UTF8.GetBytes(Configuration["JwtKey"]);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwt =>
                    {
                        jwt.RequireHttpsMetadata = false;
                        jwt.SaveToken = false;
                        jwt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ClockSkew = TimeSpan.Zero
                        };
                    });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddScoped<ISubjectRepository, SubjectRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //use swagger
            app.ConfigureOpenAPI();

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseSpaStaticFiles();


            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapToVueCliProxy(
                   "{*path}",
                   new SpaOptions { SourcePath = "ClientApp" },
                   npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                   regex: "Compiled successfully",
                   forceKill: true,
                    wsl: false // Set to true if you are using WSL on windows. For other operating systems it will be ignored
                   );
            });

        }
    }
}
