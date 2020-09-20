﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.LoadingFromFile.DatabaseLoading;
using WalutyBusinessLogic.AutoMapper.Profiles;
using WalutyBusinessLogic.Services;
using WalutyBusinessLogic.DatabaseLoading.Updater;

namespace WalutyMVCWebApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAutoMapper(typeof(UserProfileMap));

            services.AddSingleton<ILoader, Loader>();  
            services.AddTransient<ICurrencyFilesDownloader, CurrencyFilesDownloader>();
            services.AddTransient<ICurrencyFilesUnzipper, CurrencyFilesUnzipper>();
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            services.AddTransient<IUserCurrencyRepository, UserCurrencyRepository>();
            services.AddTransient<IExtremesServices, ExtremesServices>();
            services.AddTransient<IDateRange, DateRange>();
            services.AddTransient<IDateChecker, DateChecker>();
            services.AddTransient<ICurrencyConversionService, CurrencyConversionService>();
            services.AddTransient<ICurrencyNameChecker, CurrencyNameChecker>();
            services.AddTransient<ICurrenciesComparator, CurrenciesComparator>();
            services.AddTransient<ICurrenciesSelectList, CurrenciesSelectList>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IChartService, ChartService>();
            services.AddTransient<IFavoritesService, FavoritesService>();
            services.AddSingleton<ICurrencyFilesUpdater, CurrencyFilesUpdater>();


            services.AddDbContextPool<WalutyDBContext>(opt =>
                opt.UseInMemoryDatabase("Development"));

            //services.AddDbContext<WalutyDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Connection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ApplicationServices.GetService<ILoader>().Init();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.ApplicationServices.GetService<ILoader>().Init();
            app.UseAuthentication();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
