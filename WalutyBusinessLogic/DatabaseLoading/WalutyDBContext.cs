﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public class WalutyDBContext : IdentityDbContext<User> // DbContext 
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyInfo> CurrencyInfos { get; set; }

        public WalutyDBContext(DbContextOptions<WalutyDBContext> options) : base(options)
        {
        }
    }
}
