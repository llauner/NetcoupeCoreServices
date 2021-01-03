using AutoMapper;
using IgcRestApi.Common.Helper;
using IgcRestApi.DataConversion;
using IgcRestApi.Dto;
using IgcRestApi.Extensions;
using IgcRestApi.Services;
using IgcRestApi.Services.Authentication;
using IgcRestApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;

namespace IgcRestApi
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://tracemap.volavoile.net",
                                            "https://tracemap.volavoile.net",
                                            "http://heatmap.volavoile.net",
                                            "https://heatmap.volavoile.net",
                                            "http://localhost:51363",
                                            "https://localhost:44355");
                    });
            });

            services.AddControllers()
                .AddNewtonsoftJson(options => JsonHelper.GetJsonSerializerSettings());

            var token = Configuration.GetSection("tokenManagement").Get<TokenManagementDto>();
            services.AddSingleton(token);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = token.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidAudience = token.Audience,
                    ValidateAudience = false
                };
            });

            // --- Register the Swagger generator, defining 1 or more Swagger documents ---
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IGC REST API",
                    Description = "REST API to interact with Netcoupe IGC files",
                    Contact = new OpenApiContact
                    {
                        Name = "Ludovic Launer"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // --- Add security requirements ---
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

            });

            // --- Automapper
            services.AddAutoMapper(typeof(Startup).Assembly);  // Registering and Initializing AutoMapper

            // ----- Register dependencies -----
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddTransient<IDataConverter, AutoMapperDataConverter>();
            services.AddTransient<IFirestoreService, FirestoreService>();
            services.AddTransient<IFtpService, FtpService>();
            services.AddTransient<IStorageService, StorageService>();
            services.AddTransient<IIgcReaderService, IgcReaderService>();
            services.AddTransient<IAggregatorService, AggregatorService>();
            services.AddTransient<INetcoupeService, NetcoupeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISecretService, SecretService>();
            services.AddSingleton<IApiKeyService, ApiKeyService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler();    // Use Extensions/ExceptionMiddleWareExtensions.cs

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IGC API V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
