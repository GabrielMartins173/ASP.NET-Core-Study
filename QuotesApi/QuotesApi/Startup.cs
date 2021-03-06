using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuotesApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace QuotesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<QuotesDbContext>(option=>option.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog=QuotesDB;"));
            services.AddMvc().AddXmlSerializerFormatters();
            services.AddResponseCaching();

            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-0w6z3uhs.auth0.com/";
                options.Audience = "https://localhost:44353/";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, QuotesDbContext quotesDbContext)
        {
            //quotesDbContext.Database.EnsureCreated();

            //quotesDbContext.Database.Migrate();            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseResponseCaching();

            // 2. Enable authentication middleware
            app.UseAuthentication();

            quotesDbContext.Database.EnsureCreated();
        
         }
    }
}
