//using System;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using System.Linq;
//using System.Threading.Tasks;
//
//using Platform.Server.Data;
//using Platform.Shared;

//namespace Platform.Data
//{
//    public class DataSeeder
//    {
//        private readonly ApplicationDbContext applicationDbContext;


//        public UserManager<ApplicationUser> UserManager { get; }

//        public DataSeeder(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
//        {
//            this.applicationDbContext = applicationDbContext;
//            UserManager = userManager;
//        }


//        public async void SeedRolesAndAdmin()
//        {

//            var ADMIN_ROLE_ID = "685c4c1a-6c66-4bab-bb80-7c520c633c04";
         


//            var admin2Role = new IdentityRole
//            {
//                Name = ROLES.ADMINISTRATOR_NAME,
//                NormalizedName = ROLES.ADMINISTRATOR_NAME.ToUpper(),
//                Id = ADMIN_ROLE_ID,
//                ConcurrencyStamp = ADMIN_ROLE_ID
//            };

//            var users = applicationDbContext.Users.Take(2);

//            foreach (var user in users)
//            {
//                applicationDbContext.UserRoles.Where(f => f.RoleId )
//            }
//            modelBuilder.Entity<IdentityRole>().HasData(admin2Role);

//            var customerRole = new IdentityRole
//            {
//                Name = ROLES.CUSTOMER_NAME,
//                NormalizedName = ROLES.CUSTOMER_NAME.ToUpper(),
//                Id = CUSTOMER_ROLE_ID,
//                ConcurrencyStamp = CUSTOMER_ROLE_ID
//            };

//            modelBuilder.Entity<IdentityRole>().HasData(customerRole);



//            var adminUser = new ApplicationUser
//            {
//                Id = ADMIN_USER_ID,
//                FullName = "Op2B Admin",
//                Email = "admin@op2b.com.br",
//                NormalizedEmail = "ADMIN@OP2B.COM.BR",
//                UserName = "admin@op2b.com.br",
//                NormalizedUserName = "ADMIN@OP2B.COM.BR",
//                EmailConfirmed = true,

//            };

//            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "Op2B@2020");

//            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

//            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
//            {
//                RoleId = ADMIN_ROLE_ID,
//                UserId = ADMIN_USER_ID
//            });



//            //string[] roles = new string[] { ROLES.ADMINISTRATOR_NAME, ROLES.CUSTOMER_NAME };

//            //for (int i = 0; i < roles.Length; i++)
//            //{
//            //    string role = roles[i];


//            //    if (!applicationDbContext.Roles.Any(r => r.Name == role))
//            //    {
//            //        var roleStore = new RoleStore<IdentityRole>(applicationDbContext);

//            //        await roleStore.CreateAsync(new IdentityRole(role) { NormalizedName = role.ToUpper() });
//            //    }
//            //}



//            //if (!applicationDbContext.Users.Any(u => u.UserName == "admin@op2b.com.br"))
//            //{
//            //    var user = new ApplicationUser
//            //    {
//            //        FullName = "Op2B Admin",
//            //        Email = "admin@op2b.com.br",
//            //        NormalizedEmail = "ADMIN@OP2B.COM.BR",
//            //        UserName = "admin@op2b.com.br",
//            //        NormalizedUserName = "ADMIN@OP2B.COM.BR",
//            //        EmailConfirmed = true,
//            //    };

//            //    var password = new PasswordHasher<ApplicationUser>();
//            //    var hashed = password.HashPassword(user, "Op2B@2020");
//            //    //user.PasswordHash = hashed;


//            //    var userStore = new UserStore<ApplicationUser>(applicationDbContext);

//            //    var result = userStore.CreateAsync(user);
//            //    await userStore.SetPasswordHashAsync(user, hashed);
//            //    await AssignRoles(user, roles);

//            //    await applicationDbContext.SaveChangesAsync();
//            //}

//            ////AssignRoles(serviceProvider, user.Email, roles);

//            ////context.SaveChangesAsync();
//        }

//        public async Task<IdentityResult> AssignRoles(ApplicationUser user, string[] roles)
//        {
//            try
//            {
//                var result = await UserManager.AddToRolesAsync(user, roles);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                var xxx = ex;
//                throw;
//            }

//        }
//    }
//}
