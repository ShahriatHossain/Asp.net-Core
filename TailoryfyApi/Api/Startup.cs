using Api.Persistence;
using Api.Persistence.Repositories;
using Api.Service;
using Core.Models;
using Core.Repositories;
using Framework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Api
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
            var connectionString = Configuration["connectionStrings:tailoryfyDBDefaultConnection"];

            services.AddDbContext<TailoryfyDbContext>(x =>
            {
                x.UseSqlServer(connectionString);
                x.UseLoggerFactory(_loggerFactory);
            });

            services.AddDbContext<SecurityDbContext>(x =>
            {
                x.UseSqlServer(connectionString);
                x.UseLoggerFactory(_loggerFactory);
            });

            services.AddIdentity<AppUser, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 5;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<SecurityDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                      .AddJwtBearer(options =>
                      {
                          options.RequireHttpsMetadata = false;
                          options.SaveToken = true;

                          options.TokenValidationParameters =
                               new TokenValidationParameters
                               {
                                   ValidIssuer = Configuration["Tokens:Issuer"],
                                   ValidAudience = Configuration["Tokens:Audience"],
                                   ValidateIssuerSigningKey = true,
                                   ValidateLifetime = true,
                                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                               };
                      })
                      .AddCookie(options =>
                      {
                          options.Events = new CookieAuthenticationEvents
                          {
                              OnRedirectToLogin = OnRedirectToLogin,
                              OnRedirectToAccessDenied = OnRedirectToAccessDenied
                          };
                      });

            // authorization
            services.AddAuthorization(options =>
            {
                // require user to have cookie auth or jwt bearer token
                options.AddPolicy("Authenticated",
                    policy => policy
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser());
            });

            // Add global authorization policy
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new AuthorizeFilter("Authenticated"));
            });

            services.AddSingleton(Configuration);

            // Add framework services.
            services.AddCors();
            services.AddMvc();

            services.AddSingleton<IEmailClient, EmailClient>();
            services.AddScoped<IAccessControlService, AccessControlService>();
            services.AddScoped<IItemRepository, ItemRepository>();

        }

        private static Task OnRedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> ctx)
        {
            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            {
                ctx.Response.StatusCode = 403;
            }

            return Task.CompletedTask;
        }

        private static Task OnRedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                // return 401 if not "logged in" from an API Call
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            }

            // Redirect users to login page
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }

        ILoggerFactory _loggerFactory;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            TailoryfyDbContext tailoryfyDbContext)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            _loggerFactory = loggerFactory;
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog(env.ContentRootPath + "\\nlog.config");

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
