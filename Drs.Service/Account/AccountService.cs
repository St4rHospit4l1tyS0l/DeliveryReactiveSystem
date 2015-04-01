using System;
using System.Collections.Generic;
using Drs.Infrastructure.Crypto;
using Drs.Model.Account;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Repository.Account;
using Drs.Resources.Account;

namespace Drs.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public ResponseMessage Login(LoginModel login)
        {
            throw new NotImplementedException();

            //using (_repository)
            //{
            //    var userSalt = _repository.GetUserSalt(Cypher.Decrypt(login.Username));

            //    if (userSalt == null)
            //        return new ResponseMessage{IsSuccess = false, Message = ResAccount.ERROR_USERNAME_PASSWORD_INCORRECT};

            //    if(_repository.IsUserIdAndPasswordCorrect(userSalt.UserId, Cypher.Decrypt(login.Password).Hash(userSalt.Salt)) == false)
            //        return new ResponseMessage { IsSuccess = false, Message = ResAccount.ERROR_USERNAME_PASSWORD_INCORRECT };

            //    return new ResponseMessage{IsSuccess = true};
            //}
        }

        public IEnumerable<ButtonItemModel> GetMenuByUser(string username)
        {
            using (_repository)
            {
                var profileId = _repository.GetRoleIdByUsername(username);
                return _repository.GetMenuByRole(profileId);
            }
        }
    }
}
