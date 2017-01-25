using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClassDiagramGeneratorWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
           services.ConfigureSwaggerGen(options =>
           {
               options.SwaggerDoc("v1", new Info { Title = "My API", Version = "V1" });
               options.IgnoreObsoleteActions();
               options.IgnoreObsoleteProperties();
               options.DescribeAllEnumsAsStrings();
           });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 6000000000;
            });
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc();
            //app.UseSwagger();
            //app.UseSwaggerUi(c=> {c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");});
            

        }
    }
}
