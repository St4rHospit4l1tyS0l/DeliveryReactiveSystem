using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Drs.Infrastructure.Crypto;
using Drs.Infrastructure.Model;
using Drs.Model.Account;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Repository.Account;

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

        public string GetAccountInfo(string sMaInfo, string sConnInfo)
        {
            var eInfo = Cypher.Encrypt(sMaInfo);
            var mConnInfo = Cypher.Decrypt(sConnInfo);

            using (_repository)
            {
                var computerInfo = _repository.GetComputerInfo(eInfo);

                if (computerInfo == null){
                    return CreateComputerInfo(eInfo, sConnInfo);
                }

                if (IsUpdatedComputerInfo(mConnInfo) == false)
                {
                    var response = UpdateComputerInfo(eInfo, sConnInfo);
                    if (string.IsNullOrWhiteSpace(response) == false)
                        return response;
                }

                return IsValidComputerInfo(mConnInfo);

            }
        }

        private string IsValidComputerInfo(string mConnInfo)
        {
            throw new NotImplementedException();
        }

        private string UpdateComputerInfo(string eInfo, string sConnInfo)
        {
            throw new NotImplementedException();
        }

        private bool IsUpdatedComputerInfo(string mConnInfo)
        {
            throw new NotImplementedException();
        }

        private string CreateComputerInfo(string eInfo, string sConnInfo)
        {
            var model = new ConnectionInfoModel
            {
                Hk = Cypher.Decrypt(sConnInfo),
                Hn = Cypher.Decrypt(Cypher.Decrypt(eInfo)),
                St = DateTime.MinValue,
                Et = DateTime.MinValue,
                Iv = false
            };

            _repository.AddComputerInfo(eInfo, Cypher.Encrypt(new JavaScriptSerializer().Serialize(model)));

            return Cypher.Encrypt(new JavaScriptSerializer().Serialize(new ConnectionInfoResponse
            {
                //NxWn = SharedConstants.Client.
            }));


        }
    }
}
