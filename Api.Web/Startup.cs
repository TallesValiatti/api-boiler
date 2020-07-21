using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Core.Interfaces.CrossCutting.Services;
using Api.Core.Interfaces.Infra.Repositories;
using Api.Core.Tasks.Commands.Security;
using Api.CrossCutting.Services;
using Api.Infra;
using Api.Web.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Api.Infra.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.Core.Interfaces.Infra.Repositories.Security;
using Api.Core.Entities.Security;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using FluentValidation.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Api.Web.Middlewares;
using Api.CrossCutting.Services.Microservices;
using Api.Core.Services.Microservices;

namespace Api.Web
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "AllowAllOrigins";
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json",false,true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json" ,true)
                            .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region DI
            //->DI

            //Core
            //CrossCutting
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IAuthTokenService, AuthTokenService>();
            services.AddSingleton<IFailureMicroservice, FailureMicroservice>();

            //Infra
            services.AddScoped<AppDbContext>();
            services.AddScoped(typeof(IGenerericRepository<>), typeof(Api.Infra.Repositories.GenericRepository<>));
            services.AddScoped<IUserRepository<User>, Api.Infra.Repositories.Security.UserRepository>();
            services.AddScoped<ISignInRegisterRepository<SignInRegister>, Api.Infra.Repositories.Security.SignInRegisterRepository>();
            #endregion

            //connection String
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            //Config the database
            services.AddDbContext<AppDbContext>(opt => 
            {
                opt.UseSqlServer(connectionString, dbOpt => 
                {
                    dbOpt.MigrationsAssembly("Api.Web");
                    dbOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            //reponse compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<Configuration.BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });

            //swagger setup
            services.AddSwaggerSetup();

            //mediaR setup
            services.AddMediatR(typeof(CreateLoginCommand).GetTypeInfo().Assembly);

            //Validation on DTOs
            //remove null from json response
            services.AddControllers()
                .AddFluentValidation(opt => 
                {
                    opt.RegisterValidatorsFromAssembly(typeof(CreateLoginCommand).GetTypeInfo().Assembly);
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });

            //Auth 
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Settings:AuthToken:Key").Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
            };
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //        builder => builder.AllowAnyOrigin());
            //});

            //Configure global exception middleware 
            services.AddGlobalExceptionHandlerMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerSetup();
            }

            app.UseResponseCompression();

            app.UseCors(option => option
                           .AllowAnyOrigin()
                           .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors(MyAllowSpecificOrigins);
           
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseGlobalExceptionHandlerMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
