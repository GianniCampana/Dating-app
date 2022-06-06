using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get;}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });
            //questa stringa mi permette dinon avere la sicurezza corse e ricevere tutte le chiamate api nel frontend che non hanno la stesa origine del backend
            services.AddCors(x => {
                x.AddPolicy(
                    name: "CORS_POLICY",
                    builder =>{
                        builder.WithOrigins("http://localhost:4200".Trim('/', '\\'))
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                    }
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }
        
            app.UseHttpsRedirection();

            app.UseRouting();
            
            //questa stringa mi permette dinon avere la sicurezza corse e ricevere tutte le chiamate api nel frontend che non hanno la stesa origine del backend
            //app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
            //app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyMethod());
            app.UseCors("CORS_POLICY"); 
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CORS_POLICY");
            });
        }
    }
}
