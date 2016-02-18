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
using Drs.Model.Franchise;
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
            : this(new AccountRepository())
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

                if (computerInfo == null)
                {
                    return CreateComputerInfo(eInfo, sConnInfo);
                }

                GetDevice decCompInfo;
                if (IsUpdatedComputerInfo(mConnInfo, computerInfo, out decCompInfo) == false)
                {
                    var response = UpdateComputerInfo(eInfo, mConnInfo, decCompInfo);
                    if (string.IsNullOrWhiteSpace(response) == false)
                        return response;
                }

                return IsValidDeviceInfo(decCompInfo);
            }
        }

        private string IsValidDeviceInfo(GetDevice decCompInfo)
        {
            var now = DateTime.Now;

            if (decCompInfo.Code != AccountConstants.CODE_VALID)
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[decCompInfo.Code]);

            if (now < decCompInfo.St.AddDays(-2))
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[AccountConstants.CODE_NOT_ACTIVE_ST]);

            if (now > decCompInfo.Et.AddDays(10))
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[AccountConstants.CODE_NOT_ACTIVE_ET]);

            if (decCompInfo.Iv == false)
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[AccountConstants.CODE_NOT_ACTIVE]);

            return BuildResponse(SharedConstants.Client.STATUS_SCREEN_LOGIN, AccountConstants.LstCodes[AccountConstants.CODE_VALID]);
        }

        private string UpdateComputerInfo(string eInfo, string mConnInfo, GetDevice decCompInfo)
        {
            decCompInfo.Code = AccountConstants.CODE_NOT_ACTIVE_BY_UPDATE;
            decCompInfo.Hk = mConnInfo;
            _repository.UpdateComputerInfo(eInfo, decCompInfo.SerializeAndEncrypt());
            return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[decCompInfo.Code]);
        }

        private bool IsUpdatedComputerInfo(string mConnInfo, string computerInfo, out GetDevice decCompInfo)
        {
            var connInfo = mConnInfo;
            decCompInfo = new JavaScriptSerializer().Deserialize<GetDevice>(Cypher.Decrypt(computerInfo));

            return connInfo == decCompInfo.Hk;
        }

        private string CreateComputerInfo(string eInfo, string sConnInfo)
        {
            var model = new GetDevice
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

        public GetDevice ValidateMainAccount()
        {
            var eInfo = Cypher.Encrypt(Environment.MachineName);
            var mConnInfo = ManagementExt.GetKey();

            using (_repository)
            {
                var infoServer = _repository.GetInfoServer(Cypher.Encrypt(eInfo));

                if (infoServer == null)
                {
                    CreateInfoServer(eInfo, mConnInfo);
                    return null;
                }

                GetDevice decServInfo;
                if (IsUpdatedInfoServer(mConnInfo, infoServer, out decServInfo) == false)
                {
                    UpdateInfoServer(mConnInfo, decServInfo, infoServer);
                    return null;
                }

                return decServInfo;
            }
        }

        public string IsValidInfoServer()
        {
            var decSerInfo = ValidateMainAccount();

            if (decSerInfo == null)
                return BuildResponse(SharedConstants.Client.STATUS_SCREEN_MESSAGE, AccountConstants.LstCodes[AccountConstants.CODE_NOT_ACTIVE]);

            return IsValidDeviceInfo(decSerInfo);
        }

        private void UpdateInfoServer(string mConnInfo, GetDevice decServInfo, InfoServer infoServer)
        {
            decServInfo.Hk = mConnInfo;
            infoServer.Code = Cypher.Encrypt(new JavaScriptSerializer().Serialize(decServInfo));
            _repository.SaveChanges();

            //if (infoServer.InfoCallCenterId.HasValue == false)
            //    return;
        }

        private bool IsUpdatedInfoServer(string mConnInfo, InfoServer infoServer, out GetDevice decCompInfo)
        {
            var connInfo = mConnInfo;
            decCompInfo = new JavaScriptSerializer().Deserialize<GetDevice>(Cypher.Decrypt(infoServer.Code));

            if (connInfo == decCompInfo.Hk)
                return true;

            return false;
        }

        private void CreateInfoServer(string eInfo, string mConnInfo)
        {
            var model = new GetDevice
            {
                Hk = mConnInfo,             //HostId
                Hn = Cypher.Decrypt(eInfo),  //HostName
                St = DateTime.MinValue,
                Et = DateTime.MinValue,
                Iv = false,                      //Is valid license
                Code = AccountConstants.CODE_NEW
            };

            _repository.AddInfoServer(Cypher.Encrypt(eInfo), Cypher.Encrypt(new JavaScriptSerializer().Serialize(model)));

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

        public DeviceInfoModel GetLstClients()
        {
            var deviceInfo = new DeviceInfoModel();
            using (_repository)
            {
                var jsSer = new JavaScriptSerializer();
                GetDevices(_repository.GetLstClients(), jsSer, deviceInfo.LstClients);
            }
            return deviceInfo;
        }

        public ConnectionFullModel GetTerminalInfo(int id, JavaScriptSerializer jSer)
        {
            var model = _repository.GetClient(id);
            if (model == null)
                return null;

            GetDevice(model, jSer);
            return model;
        }

        public bool DoSelectServer(int id, bool enable)
        {
            using (_repository)
            {
                var infoServer = _repository.GetInfoServer(id);
                if (infoServer == null)
                    return false;

                var callCenterId = VerifyHasCallCenter();

                if (enable)
                    infoServer.InfoCallCenterId = callCenterId;
                else
                    infoServer.InfoCallCenterId = null;

                _repository.SaveChanges();
                return true;
            }
        }

        public bool DoSelectClient(int id, bool enable)
        {
            using (_repository)
            {
                var infoClient = _repository.GetInfoClientTerminal(id);
                if (infoClient == null)
                    return false;

                var callCenterId = VerifyHasCallCenter();

                if (enable)
                    infoClient.InfoCallCenterId = callCenterId;
                else
                    infoClient.InfoCallCenterId = null;

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

                if (deviceConn.LstClients.Count == 0 || deviceConn.LstServers.Count == 0)
                {
                    return new ResponseMessageModel
                    {
                        HasError = true,
                        Title = "Error licencia",
                        Message = "Al menos debes tener una terminal y un servidor para activar la licencia"
                    };
                }

                var deviceConnTx = deviceConn.SerializeAndEncrypt();
                string responseConn;

                using (var proxy = new LicProxySvcClient())
                {
                    responseConn = await proxy.RequestActivationAsync(deviceConnTx);
                }

                try
                {
                    return ProcessActivationResponse(responseConn);
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
            }
        }

        private ResponseMessageModel ProcessActivationResponse(string responseConn)
        {
            var response = responseConn.DeserializeAndDecrypt<ResponseConnection>();

            if (response.IsSuccess == false)
            {
                return new ResponseMessageModel
                {
                    HasError = true,
                    Message = response.Message
                };
            }

            var devices = response.Data.DeserializeAndDecrypt<DeviceConnModel>();

            foreach (var device in devices.LstClients)
            {
                UpdateClientDevice(device);
            }

            foreach (var device in devices.LstServers)
            {
                UpdateServerDevice(device);
            }

            return new ResponseMessageModel
            {
                HasError = false
            };

        }

        private void UpdateServerDevice(string device)
        {
            try
            {
                var connInfo = device.DeserializeAndDecrypt<GetDevice>();
                var deviceModel = _repository.GetServerByHost(Cypher.Encrypt(Cypher.Encrypt(connInfo.Hn)));

                if (deviceModel == null)
                    return;

                deviceModel.Code = device;
                _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, device);
            }
        }

        private void UpdateClientDevice(string device)
        {
            try
            {
                var connInfo = device.DeserializeAndDecrypt<GetDevice>();
                var deviceModel = _repository.GetClientTerminalByHost(Cypher.Encrypt(Cypher.Encrypt(connInfo.Hn)));

                if (deviceModel == null)
                    return;

                deviceModel.Code = device;
                _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, device);
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

        public List<TerminaFranchiseModel> GetLstTerminalFranchise(int id)
        {
            return _repository.GetLstTerminalFranchise(id);
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
                try
                {
                    GetDevice(client, jsSer);
                }
                catch
                {
                    continue;
                }
                lstInfo.Add(client);
            }
        }

        private static void GetDevice(ConnectionFullModel client, JavaScriptSerializer jsSer)
        {
            var model = jsSer.Deserialize<GetDevice>(Cypher.Decrypt(client.Code));
            client.DeviceName = model.Hn;
            client.StartDateTx = model.St.Date == DateTime.MinValue.Date ? "ND" : model.St.ToString(SharedConstants.DATE_FORMAT);
            client.EndDateTx = model.Et.Date == DateTime.MinValue.Date ? "ND" : model.Et.ToString(SharedConstants.DATE_FORMAT);
            client.IsValid = model.Iv;
            client.CodeId = model.Code;
            client.Code = AccountConstants.LstBadges[model.Code];
        }

        public void AddActivationCode(string actCode)
        {
            using (_repository)
            {
                var code = new ActivationCodeModel
                {
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

        public int UpsertTerminalFranchise(TerminaFranchiseModel model)
        {
            using (_repository)
            {
                return _repository.UpsertTerminalFranchise(model);
            }
        }
    }
}
