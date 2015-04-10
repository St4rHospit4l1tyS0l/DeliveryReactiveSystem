using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Drs.Infrastructure.Crypto;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Extensions.Json;
using Drs.Infrastructure.Hinfo;
using Drs.Infrastructure.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Account;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Repository.Account;
using Drs.Repository.Entities;
using Drs.Repository.Log;
using Drs.Service.LicProxySvc;

namespace Drs.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService()
            :this(new AccountRepository())
        {
        }
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

                ConnectionInfoModel decCompInfo;
                if (IsUpdatedComputerInfo(mConnInfo, computerInfo, out decCompInfo) == false)
                {
                    var response = UpdateComputerInfo(eInfo, sConnInfo);
                    if (string.IsNullOrWhiteSpace(response) == false)
                        return response;
                }

                return IsValidComputerInfo(decCompInfo);
            }
        }

        private string IsValidComputerInfo(ConnectionInfoModel decCompInfo)
        {
            var now = DateTime.Now;

            if (decCompInfo.Code != AccountConstants.CODE_VALID)
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[decCompInfo.Code]);

            if (now < decCompInfo.St.AddDays(-2))
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[AccountConstants.CODE_NOT_ACTIVE_ST]);


            if (now > decCompInfo.Et.AddDays(10))
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[AccountConstants.CODE_NOT_ACTIVE_ET]);
        
        
            if(decCompInfo.Iv == false)
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[AccountConstants.CODE_NOT_ACTIVE]);

        
            return  BuildResponse(SharedConstants.Client.STATUS_SCREEN_LOGIN, AccountConstants.LstCodes[AccountConstants.CODE_VALID]);
        }

        private string UpdateComputerInfo(string eInfo, string sConnInfo)
        {
            return String.Empty;
        }

        private bool IsUpdatedComputerInfo(string mConnInfo, string computerInfo, out ConnectionInfoModel decCompInfo)
        {
            var connInfo = mConnInfo;
            decCompInfo = new JavaScriptSerializer().Deserialize<ConnectionInfoModel>(Cypher.Decrypt(computerInfo));

            if (connInfo == decCompInfo.Hk)
                return true;
            
            return false;
        }

        private string CreateComputerInfo(string eInfo, string sConnInfo)
        {
            var model = new ConnectionInfoModel
            {
                Hk = Cypher.Decrypt(sConnInfo),             //HostId
                Hn = Cypher.Decrypt(Cypher.Decrypt(eInfo)),  //HostName
                St = DateTime.MinValue,             
                Et = DateTime.MinValue,         
                Iv = false,                      //Is valid license
                Code = AccountConstants.CODE_NEW
            };

            _repository.AddComputerInfo(eInfo, Cypher.Encrypt(new JavaScriptSerializer().Serialize(model)));
            return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[model.Code]);
        }

        private static string BuildResponse(int wnd, string msg)
        {
            return Cypher.Encrypt(new JavaScriptSerializer().Serialize(new ConnectionInfoResponse
            {
                NxWn = wnd,
                Msg = msg
            }));
        }

        public void ValidateMainAccount()
        {
            var eInfo = Cypher.Encrypt(Environment.MachineName);
            var mConnInfo = ManagementExt.GetKey();

            using (_repository)
            {
                var serverInfo = _repository.GetServerInfo(Cypher.Encrypt(eInfo));

                if (serverInfo == null)
                {
                    CreateServerInfo(eInfo, mConnInfo);
                    return;
                }

                ConnectionInfoModel decServInfo;
                if (IsUpdatedServerInfo(mConnInfo, serverInfo, out decServInfo) == false)
                {
                    UpdateServerInfo(eInfo, mConnInfo, decServInfo, serverInfo);
                }
            }
        }


        private void UpdateServerInfo(string eInfo, string mConnInfo, ConnectionInfoModel decServInfo, ServerInfo serverInfo)
        {
            decServInfo.Hk = mConnInfo;
            serverInfo.ServerCode = Cypher.Encrypt(new JavaScriptSerializer().Serialize(decServInfo));
            _repository.SaveChanges();

            if (serverInfo.CallCenterInfoId.HasValue == false)
                return;

            return;
        }

        private bool IsUpdatedServerInfo(string mConnInfo, ServerInfo serverInfo, out ConnectionInfoModel decCompInfo)
        {
            var connInfo = mConnInfo;
            decCompInfo = new JavaScriptSerializer().Deserialize<ConnectionInfoModel>(Cypher.Decrypt(serverInfo.ServerCode));

            if (connInfo == decCompInfo.Hk)
                return true;

            return false;
        }

        private void CreateServerInfo(string eInfo, string mConnInfo)
        {
            var model = new ConnectionInfoModel
            {
                Hk = mConnInfo,             //HostId
                Hn = Cypher.Decrypt(eInfo),  //HostName
                St = DateTime.MinValue,
                Et = DateTime.MinValue,
                Iv = false,                      //Is valid license
                Code = AccountConstants.CODE_NEW
            };

            _repository.AddServerInfo(Cypher.Encrypt(eInfo), Cypher.Encrypt(new JavaScriptSerializer().Serialize(model)));
        
        }

        public DeviceInfoModel GetLstDevices()
        {
            var deviceInfo = new DeviceInfoModel();
            using (_repository)
            {
                var jsSer = new JavaScriptSerializer();
                GetDevices(_repository.GetLstServers(), jsSer, deviceInfo.LstServers);
                GetDevices(_repository.GetLstClients(), jsSer, deviceInfo.LstClients);
            }
            return deviceInfo;
        }

        public bool DoSelectServer(int id, bool enable)
        {
            using (_repository)
            {
                var serverInfo = _repository.GetServerInfo(id);
                if (serverInfo == null)
                    return false;

                var callCenterId = VerifyHasCallCenter();

                if (enable)
                    serverInfo.CallCenterInfoId = callCenterId;
                else
                    serverInfo.CallCenterInfoId = null;

                _repository.SaveChanges();
                return true; 
            }
        }

        public bool DoSelectClient(int id, bool enable)
        {
            using (_repository)
            {
                var clientInfo = _repository.GetClientInfo(id);
                if (clientInfo == null)
                    return false;

                var callCenterId = VerifyHasCallCenter();

                if (enable)
                    clientInfo.CallCenterInfoId = callCenterId;
                else
                    clientInfo.CallCenterInfoId = null;

                _repository.SaveChanges();
                return true;
            }
        }

        public async Task<ResponseMessageModel> AskForLicense()
        {
            using (_repository)
            {
                var deviceConn = new DeviceConnModel
                {
                    LstClients = _repository.GetLstClientsCodes(),
                    LstServers = _repository.GetLstServersCodes(),
                    ActivationCode = _repository.GetActivationCode()
                };

                if (deviceConn.LstClients.Count == 0 || deviceConn.LstServers.Count == 0){
                    return new ResponseMessageModel{
                        HasError = true,
                        Title = "Error licencia",
                        Message = ""
                    };
                }

                var deviceConnTx = deviceConn.SerializeAndEncrypt();
                var responseConn = String.Empty;

                using (var proxy = new LicProxySvcClient())
                {
                    responseConn = await proxy.RequestActivationAsync(deviceConnTx);
                }

                try
                {

                }
                catch (Exception ex)
                {
                    SharedLogger.LogError(ex, responseConn);
                    return new ResponseMessageModel
                    {
                        HasError = true,
                        Message = "Respuesta no válida"
                    };
                }

                return new ResponseMessageModel{
                    HasError = false
                };
            }
        }

        public string GetActivationCodeToShow()
        {
            var actCode = _repository.GetActivationCode();

            if (string.IsNullOrWhiteSpace(actCode))
                return String.Empty;

            try
            {
                var actCodeModel = actCode.DeserializeAndDecrypt<ActivationCodeModel>();
                return actCodeModel.ActivationCode.SubstringMax(10) + "***********************************";
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        private int VerifyHasCallCenter()
        {
            var callCenterId = _repository.GetCallCenterId();

            if (callCenterId > 0)
                return callCenterId;

            return _repository.AddCallCenterId();

        }

        private static void GetDevices(IEnumerable<ConnectionFullModel> lstDevices, JavaScriptSerializer jsSer, List<ConnectionFullModel> lstInfo)
        {
            foreach (var client in lstDevices)
            {
                ConnectionInfoModel model;

                try
                {
                    model = jsSer.Deserialize<ConnectionInfoModel>(Cypher.Decrypt(client.Code));
                    client.DeviceName = model.Hn;
                    client.StartDateTx = model.St.Date == DateTime.MinValue.Date ? "ND" : model.St.ToString(SharedConstants.DATE_FORMAT);
                    client.EndDateTx = model.Et.Date == DateTime.MinValue.Date ? "ND" : model.Et.ToString(SharedConstants.DATE_FORMAT);
                    client.IsValid = model.Iv;
                    client.CodeId = model.Code;
                    client.Code = AccountConstants.LstBadges[model.Code];
                }
                catch
                {
                    continue;
                }
                lstInfo.Add(client);
            }
        }

        public void AddActivationCode(string actCode)
        {
            using (_repository)
            {
                var code = new ActivationCodeModel{
                    ActivationCode = actCode,
                    ActivationCodeId = Guid.NewGuid().ToString()
                }.SerializeAndEncrypt();


                if (_repository.GetCallCenterId() <= 0)
                {
                    _repository.AddActivationCode(code);
                }
                else
                {
                    _repository.UpdateActivationCode(code);
                }
            }
        }
    }
}
