using System;
using Drs.Infrastructure.Crypto;
using Drs.Model.Account;
using Drs.Model.Shared;
using Drs.Repository.Account;
using Drs.Repository.Entities;
using Drs.Resources.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementCallCenter.Account
{

    public class LoginSvc : ILoginSvc
    {
        //private readonly IAccountService _serviceAccount;

        //public LoginSvc(IAccountService service)
        //{
        //    _serviceAccount = service;
        //}

        public ResponseMessage Login(LoginModel login)
        {
            try
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {
                    var asyncUser = userManager.FindAsync(Cypher.Decrypt(login.Username), Cypher.Decrypt(login.Password));
                    asyncUser.Wait();
                    var result = asyncUser.Result;

                    if (result == null)
                        return new ResponseMessage { IsSuccess = false, Message = ResAccount.ERROR_USERNAME_PASSWORD_INCORRECT };

                    using (var accountRepository = new AccountRepository())
                    {
                        if (!accountRepository.IsValidUser(result.Id))
                        {
                            return new ResponseMessage { IsSuccess = false, Message = ResAccount.ERROR_USERNAME_PASSWORD_INCORRECT };
                        }
                    }

                    return new ResponseMessage { IsSuccess = true };

                }
            
                //Thread.Sleep(4000);
                //return _serviceAccount.Login(login);
            }
            catch (Exception ex)
            {
                return new ResponseMessage { IsSuccess = false, Message = ex.Message};
            }

        }
    }
}
