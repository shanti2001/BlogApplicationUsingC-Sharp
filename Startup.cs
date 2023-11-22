using Microsoft.Extensions.DependencyInjection;
using BlogApplication.Reposotory;
using BlogApplication.Services;
using com.blogApplication.BlogApplication2.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication{
    public class Startup{
         public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<PostService>(); 
            services.AddAuthorization(options =>
            {
                
            });
            services.AddControllersWithViews();
        }
         public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
    }
    
    
}
