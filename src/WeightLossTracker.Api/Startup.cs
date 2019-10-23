using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using WeightLossTracker.Api.Helpers;
using WeightLossTracker.DataStore;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Repositories.Impl;
using WeightLossTracker.DataStore.Repositories.Interface;

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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); 

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

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddSwaggerGen(setupAction =>
            {
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
                    Type = "apiKey"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
