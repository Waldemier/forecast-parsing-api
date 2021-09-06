using System;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForecastAPI.Data.Seed
{
    public class SeedUsers: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                    Name = "Admin",
                    Email = "admin@brainence.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123@!"),
                    Role = RoleTypes.Admin
                },
                new User
                {
                    Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                    Name = "Admin2",
                    Email = "admin2@brainence.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123@!"),
                    Role = RoleTypes.Admin
                },
                new User
                {
                    Id = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                    Name = "Mark",
                    Email = "mark@brainence.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Mark123@!"),
                    Role = RoleTypes.SystemUser
                },
                new User
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "Iryna",
                    Email = "iryna@brainence.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Iryna123@!"),
                    Role = RoleTypes.SystemUser
                }
            );
        }
    }
}