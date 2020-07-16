using Api.Core.Entities.Security;
using Api.Core.Interfaces.CrossCutting.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Infra.ModelsConfiguration
{
    class UserModelConfiguration : IEntityTypeConfiguration<User>
    {
        public IPasswordService _passwordService { get; set; }
        public IConfiguration _configuration{ get; set; }
        public UserModelConfiguration(IPasswordService passwordService, IConfiguration configuration)
        {
            _passwordService = passwordService;
            _configuration = configuration;
        }
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Email)
              .IsRequired();

            builder.Property(x => x.PasswordHash)
              .IsRequired();

            //seed
            builder.HasData(new User
            {
                Id = Guid.Parse("0a725c02-6345-4d65-84b7-ad32b1a535d0"),
                Name = "Talles Valiatti",
                Email = "talles.dsv@gmail.com",
                PasswordHash = _passwordService.CreatePassword("Teste1!", _configuration.GetSection("Settings:PasswordHashKey").Value)
            }); ;
        }
    }
}
