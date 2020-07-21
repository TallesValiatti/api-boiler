
using Api.Core.Entities.Security;
using Api.Core.Interfaces.CrossCutting.Services;
using Api.Infra.ModelsConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Infra
{
    public class AppDbContext : DbContext
    {
        public IPasswordService _passwordService { get; set; }
        public IConfiguration _configuration { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> opt, IConfiguration configuration, IPasswordService passwordService) : base(opt)
        {
            _passwordService = passwordService;
            _configuration = configuration;
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserModelConfiguration(_passwordService,_configuration));
            modelBuilder.ApplyConfiguration(new SignInRegisterModelConfiguration(_passwordService,_configuration));
        }
    }
}
