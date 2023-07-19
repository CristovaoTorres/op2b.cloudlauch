using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Server.Data;
using Platform.Shared;
using Platform.Shared.Models;

namespace Platform.Server.Services
{

    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext applicationDbContext;

        public AccountService(ILogger<AccountService> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ResponseBase<string>> AddUserAsync(UserModel target)
        {
            var response = new ResponseBase<string>();
            if (target == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Null", ErrorMessage = "Nenhum usuario informado para adicionar" });
                return response;
            }

            if (string.IsNullOrWhiteSpace(target.Password) || target.Password.Length < 8)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Password", ErrorMessage = "Senha deve ser informada e ter pelo menos 8 caracteres" });
                return response;
            }

            if (!target.Password.Equals(target.PasswordRepeat, StringComparison.OrdinalIgnoreCase))
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Password", ErrorMessage = "Senhas devem ser iguais" });
                return response;
            }


            if (!target.Roles.Any())
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Roles", ErrorMessage = "Papel do usuario deve ser informado." });
                return response;
            }


            if (!string.IsNullOrWhiteSpace(target.Id))
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Id", ErrorMessage = "Id deve ser vazio" });
                return response;
            }


            var user = await userManager.FindByIdAsync(target.Id);


            if (string.IsNullOrEmpty(target.UserName))
            {
                target.UserName = target.Email;
            }

            user = new ApplicationUser
            {
                FullName = target.FullName,
                Email = target.Email,
                NormalizedEmail = target.Email.ToUpper(),
                UserName = target.UserName,
                NormalizedUserName = target.UserName.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };


            try
            {
                IdentityResult result;
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var hashed = passwordHasher.HashPassword(user, "Op2B@2020");
                result = await userManager.CreateAsync(user);
                var userStore = new UserStore<ApplicationUser>(applicationDbContext);
                await userStore.SetPasswordHashAsync(user, hashed);
                user.PasswordHash = passwordHasher.HashPassword(user, target.Password);


                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        response.ValidationResults.Add(new ValidationFailure { ErrorCode = error.Code, ErrorMessage = error.Description });
                    }
                    return response;
                }
                else
                {
                    await applicationDbContext.SaveChangesAsync();
                    response.Item = user.Id;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add("Exception", ex.ToString());
            }


            response = await ReasignRolesAsync(user, target.Roles);
            response.Item = user?.Id ?? "";
            return response;
        }


        /// <summary>
        /// Adiciona as roles ao usuario informado.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="finalRoles">Relacao de roles que o usuario deve ter.</param>
        /// <param name="removePreviousRoles">True para remover todas as roles que o usuario tem, antes de inserir as novas roles</param>
        /// <returns></returns>
        private async Task<ResponseBase<string>> ReasignRolesAsync(ApplicationUser user, IList<string> finalRoles, bool removePreviousRoles = true)
        {
            var response = new ResponseBase<string>();
            var allRoles = (await userManager.GetRolesAsync(user)).ToList();

            
            IdentityResult result = null;

            if (removePreviousRoles)
            {
                //remove todas as roles que o usuario tem atualmente.
                result = await userManager.RemoveFromRolesAsync(user, allRoles);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        response.ValidationResults.Add(new ValidationFailure { ErrorCode = error.Code, ErrorMessage = error.Description });
                    }
                    return response;
                }
            }

         

            response.Success = true;
            //Insere as roles que o usuario deve ter
            result = await userManager.AddToRolesAsync(user, finalRoles);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.ValidationResults.Add(new ValidationFailure { ErrorCode = error.Code, ErrorMessage = error.Description });
                }
                return response;
            }

            response.Success = true;
            return response;
        }

        /// <summary>
        /// Atualiza os dados do usuario informado.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public async Task<ResponseBase> UpdateAsync(UserModel target)
        {
            var response = new ResponseBase();

            if (target == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Null", ErrorMessage = "Nenhum usuario informado para adicionar" });
                return response;
            }

            var changePassword = false;

            if (string.IsNullOrWhiteSpace(target.Password) && string.IsNullOrWhiteSpace(target.PasswordRepeat))
            {
                changePassword = false;
            }
            else if (!string.IsNullOrWhiteSpace(target.Password) || !string.IsNullOrWhiteSpace(target.PasswordRepeat))
            {
                changePassword = true;
            }


            //var changePassword =  !(string.IsNullOrEmpty(target.Password) && string.IsNullOrEmpty(target.PasswordRepeat)) || (target.Password != target.PasswordRepeat);

            if (changePassword)
            {
                if (string.IsNullOrWhiteSpace(target.Password) || target.Password.Length < 8)
                {
                    response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Password", ErrorMessage = "Senha deve ser informada e ter pelo menos 8 caracteres" });
                    return response;
                }

                if (!target.Password?.Equals(target.PasswordRepeat, StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Password", ErrorMessage = "Senhas devem ser iguais" });
                    return response;
                }
            }


            var user = await userManager.FindByIdAsync(target.Id);

            user.Email = target.Email;
            user.FullName = target.FullName;
            user.UserName = target.UserName;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.ValidationResults.Add(new ValidationFailure { ErrorCode = error.Code, ErrorMessage = error.Description });
                }
                return response;
            }

            if (changePassword)
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

                var passworduser = await userManager.ResetPasswordAsync(user, resetToken, target.Password);

                if (!passworduser.Succeeded)
                {
                    var firstError = passworduser.Errors.FirstOrDefault();
                    if (firstError != null)
                    {
                        response.ValidationResults.Add(new ValidationFailure { ErrorCode = firstError.Code, ErrorMessage = firstError.Description });
                    }
                    else
                    {
                        response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Password", ErrorMessage = "Erro na senha" });
                    }
                    return response;
                }
            }

            


            response = await ReasignRolesAsync(user, target.Roles);

            return response;
        }

          

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            var list = new List<UserModel>();
            foreach (var x in userManager.Users.ToList())
            {
                var retUser = new UserModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                };
                try
                {
                    retUser.Roles = await userManager.GetRolesAsync(x);
                    list.Add(retUser);

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return list;
        }



        public async Task<ResponseBase> DeleteAsync(string Id)
        {
            var response = new ResponseBase();
            if (Id == null)
            {
                response.Errors.Add("Null", "Id nulo ou nao informado");
                return response;
            }


            if (Id.Equals(ROLES.ADMIN_USER_ID))
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Forbidden", ErrorMessage = "Exclusao o primeiro-administrador nao e' permitida." });
                return response;
            }

            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "NotFound", ErrorMessage = "Usuario nao encontrado com o Id informado" });
                return response;
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.ValidationResults.Add(new ValidationFailure { ErrorCode = error.Code, ErrorMessage = error.Description });
                }
                return response;
            }

            response.Success = true;
            return response;
        }
    }
}
