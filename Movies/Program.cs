using BLL.Interfaces;
using BLL.Repositories;
using DAL.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Movies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //automapper
            builder.Services.AddAutoMapper(typeof(Program));


            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped< IGenreRepository, GenreRepository>();

            builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
            {
                ProgressBar=true,
                PositionClass=ToastPositions.TopCenter,
                PreventDuplicates=true,
                CloseButton=true,
                
            });
        

            var app = builder.Build();
            //builder.Services.AddScoped<IMovieRepository,MovieRepository>();



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}