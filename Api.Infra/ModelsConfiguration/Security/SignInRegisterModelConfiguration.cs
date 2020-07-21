using Api.Core.Entities.Security;
using Api.Core.Interfaces.CrossCutting.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Api.Infra.ModelsConfiguration
{
    public class SignInRegisterModelConfiguration : IEntityTypeConfiguration<SignInRegister>
    {
        public IPasswordService _passwordService { get; set; }
        public IConfiguration _configuration{ get; set; }
        public SignInRegisterModelConfiguration(IPasswordService passwordService, IConfiguration configuration)
        {
            _passwordService = passwordService;
            _configuration = configuration;
        }
        public void Configure(EntityTypeBuilder<SignInRegister> builder)
        {
            builder.ToTable(nameof(SignInRegister));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
              .HasDefaultValueSql("GETUTCDATE ()");

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.AuthToken)
                .IsRequired();

            builder.Property(x => x.AuthRefreshToken)
                .IsRequired();

            builder.Property(x => x.SignInType)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.SignInRegisters)
                .HasForeignKey(x => x.UserId);

            builder.Property(x => x.AuthRefreshTokenValidation)
               .IsRequired();
        }
    }
}
