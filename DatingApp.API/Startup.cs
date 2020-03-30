using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)   // dependency Injection 
        {
            // adding the services for connecting the DB 
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers().AddNewtonsoftJson(opt =>{
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            // For API corse issue  we need to add this service 
            services.AddCors();
            /// for every http request it creates an instance and use the same instance for the requests with in the scope
            services.AddScoped<IAuthRepository,AuthorityRepository>();
            services.AddScoped<IDatingRepository,DatingRepository>();
            // for authentication 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options => {
                      options.TokenValidationParameters =new TokenValidationParameters 
                      {
                           ValidateIssuerSigningKey =true,
                           IssuerSigningKey =  new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings :Token").Value)),
                           ValidateIssuer = false, // because we are in localhost
                           ValidateAudience =false  // because we are in localhost            
                      };
                 });  
                 // Auto Mappers 
              services.AddAutoMapper(typeof (DatingRepository).Assembly);     
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                /// Handling global exceptions 
                app.UseExceptionHandler(builder =>{
                   builder.Run(async context => {

                        context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if(error != null){
                           context.Response.AddApplicationError(error.Error.Message);
                           await context.Response.WriteAsync(error.Error.Message);
                        }
                   });
                });
            }
            // 
            // app.UseHttpsRedirection();
            /// cors it allows any method with any header and any Origin
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();
         // it maps controller Endpoints 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
