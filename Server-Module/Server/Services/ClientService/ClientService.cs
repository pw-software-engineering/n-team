using Server.Database;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels;
using Server.Database.DataAccess;
using AutoMapper;
using Server.Services.Result;
using System.Net;
using System.Text.RegularExpressions;
using Server.Authentication.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;

namespace Server.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly IClientDataAccess _dataAccess;
        private readonly IMapper _mapper;
        private readonly IDatabaseTransaction _transaction;
        public ClientService(IClientDataAccess dataAccess, IMapper mapper, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
            _transaction = transaction;
        }

        public IServiceResult UpdateClientInfo(int clientID, EditClientInfo editClientInfo)
        {
            bool usernameEmpty = string.IsNullOrWhiteSpace(editClientInfo.Username);
            Regex usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{5,60}$");
            bool emailEmpty = string.IsNullOrWhiteSpace(editClientInfo.Email);
            Regex emailRegex = new Regex(@"^[a-zA-Z]([a-zA-Z0-9]|[\.\-_]){0,100}\@[a-z]{1,40}\.[a-z]{1,40}$");

            if (usernameEmpty && emailEmpty)
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Username and e-mail are null"));
            else if (!usernameEmpty && !usernameRegex.IsMatch(editClientInfo.Username))
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Invalid (or too short/long) username"));
            else if (!emailEmpty && !emailRegex.IsMatch(editClientInfo.Email))
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Invalid (or too short/long) e-mail"));

            _transaction.BeginTransaction();
            _dataAccess.UpdateClientInfo(clientID, editClientInfo);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult GetClientInfo(int clientID)
        {
            ClientInfoView clientInfo = _dataAccess.GetClientInfo(clientID);
            if(clientInfo == null)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest, 
                    new Error($"User with ID equal to {clientID} does not exist"));
            }
            return new ServiceResult(HttpStatusCode.OK, clientInfo);
        }

        public IServiceResult Login(ClientCredentials clientCredentials)
        {
            if(clientCredentials == null)
            {
                throw new ArgumentNullException("clientCredentials");
            }
            if(string.IsNullOrEmpty(clientCredentials.Login) || string.IsNullOrEmpty(clientCredentials.Password))
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new Error("Both password and username properties must be included in the request"));
            }
            int? clientID = _dataAccess.GetRegisteredClientID(clientCredentials.Login, clientCredentials.Password);
            if(!clientID.HasValue)
            {
                return new ServiceResult(
                    HttpStatusCode.Unauthorized,
                    new Error("User with provided login does not exist or provided password is incorrect"));
            }
            return new ServiceResult(HttpStatusCode.OK, new ClientToken(clientID.Value));
        }
    }
}
