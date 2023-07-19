
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Platform.Shared;
using System.Security.Claims;
using Platform.Shared.Models;

namespace Platform.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
            var ADMIN_ROLE_ID = "685c4c1a-6c66-4bab-bb80-7c520c633c04";
            var CUSTOMER_ROLE_ID = "b0099fd9-8a2f-4a70-9c2e-996f851c9f43";

     
            var adminRole = new IdentityRole
            {
                Name = ROLES.ADMINISTRATOR_NAME,
                NormalizedName = ROLES.ADMINISTRATOR_NAME.ToUpper(),
                Id = ADMIN_ROLE_ID,
                ConcurrencyStamp = ADMIN_ROLE_ID
            };
            modelBuilder.Entity<IdentityRole>().HasData(adminRole);

            var customerRole = new IdentityRole
            {
                Name = ROLES.CUSTOMER_NAME,
                NormalizedName = ROLES.CUSTOMER_NAME.ToUpper(),
                Id = CUSTOMER_ROLE_ID,
                ConcurrencyStamp = CUSTOMER_ROLE_ID
            };

            modelBuilder.Entity<IdentityRole>().HasData(customerRole);



            var adminUser = new ApplicationUser
            {
                Id = ROLES.ADMIN_USER_ID,
                FullName = "Op2B Admin",
                Email = "admin@op2b.com.br",
                NormalizedEmail = "ADMIN@OP2B.COM.BR",
                UserName = "admin@op2b.com.br",
                NormalizedUserName = "ADMIN@OP2B.COM.BR",
                EmailConfirmed = true,
             
            };

            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "Op2B@2020");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ROLES.ADMIN_USER_ID
            });

            var adminClaim = new IdentityUserClaim<string> { Id = 1, UserId = ROLES.ADMIN_USER_ID, ClaimType = ClaimsConstants.PERMISSION_CLAIM_IS_ADMIN, ClaimValue = "true" };

            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(adminClaim);

            modelBuilder.Entity<UserVMScheduling>().HasKey(p => new { p.UserId, p.ActionTypeId });

            //modelBuilder.Entity<DayOfWeeks>().HasData(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
            //         .Select(e => new DayOfWeeks()
            //         {
            //              DayOfWeek = e,
            //              Name = e.ToString()
            //         })
            // );




        }

        /// <summary>
        /// Relacao de empresas
        /// </summary>
        public DbSet<CompanyRow> Companies { get; set; }

        /// <summary>
        /// Maquinas virtuais existentes
        /// </summary>
        public DbSet<VirtualMachineRow> VirtualMachines { get; set; }

        public DbSet<CompanyUserRow> CompanyUsers { get; set; }

        public DbSet<UserVMScheduling> UserVMSchedulings { get; set; }

        //public DbSet<DayOfWeeks> DayOfWeeks { get; set; }


    }
}
