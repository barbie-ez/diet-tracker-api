﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using WeightLossTrackeData.Repositories.Impl;
using WeightLossTracker.Api.Helpers;
using WeightLossTracker.DataStore;
using WeightLossTracker.DataStore.Repositories.Impl;
using WeightLossTrackerData.DataContext;
using WeightLossTrackerData.Entities;
using WeightLossTrackerData.Repositories.Interface;

namespace WeightLossTracker.Api
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("WebApplication")));

            services.AddIdentity<UserProfileModel, UserRoleModel>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
                config.User.RequireUniqueEmail = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 5;
                config.Password.RequireDigit = false;
                config.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<MemberRepository>() 
            .AddDefaultTokenProviders();

            services.AddCors();

            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlSerializerInputFormatter(setupAction));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
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
                    ValidateAudience = false
                };
            });

            services.AddTransient<IFoodRepository, FoodRepository>();
            services.AddTransient<IDietTrackerRepository, DietTrackerRepository>();
            services.AddTransient<IMealCategoryRepository, MealCategoryRepository>();
            services.AddTransient<IWeightHistoryRepository, WeightHistoryRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(impl=>
            {
                var actionContext = impl.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddSwaggerGen(setupAction =>
            {
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                setupAction.SwaggerDoc("DietTrackerOpenApiSpecification", 
                    new Info() {
                        Title= "Diet tracker API",
                        Version="1",
                        Description="Through this api you can keep track of the food you eat",
                        Contact= new Contact
                        {
                            Email="ezomo.barbara@gmail.com",
                            Name="Barbara Ezomo",
                            Url= "https://www.linkedin.com/in/barbara-ezomo-5997a495/"
                        },
                        License=new License
                        {
                            Name="MIT License",
                            Url= "https://opensource.org/licenses/MIT"
                        }
                    });

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory,xmlComments);

                setupAction.IncludeXmlComments(xmlCommentsPath);

                setupAction.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });

                setupAction.AddSecurityRequirement(security);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Information) ;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler(appBuilder =>
                                            appBuilder.Run(async context =>
                                            {
                                                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                                                if (exceptionHandlerFeature != null)
                                                {
                                                    var logger = loggerFactory.CreateLogger("Global Exception Logger");
                                                    logger.LogError(500,exceptionHandlerFeature.Error,exceptionHandlerFeature.Error.Message);
                                             
                                                }
                                                context.Response.StatusCode = 500;
                                                await context.Response.WriteAsync("An unexpected server side error occured");
                                            }));
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction=> {
                setupAction.SwaggerEndpoint
                    ("/swagger/DietTrackerOpenApiSpecification/swagger.json","Diet Tracker API");
                setupAction.RoutePrefix="";

                
            });
            app.UseMvc();
        }
    }
}
