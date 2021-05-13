using Server.Database;
using Server.Database.Models;
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
using Server.Database.DataAccess.Client;
using Server.RequestModels.Client;
using Server.ViewModels.Client;

namespace Server.Services.Client
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly IClientDataAccess _dataAccess;
        private readonly IMapper _mapper;
        private readonly IDatabaseTransaction _transaction;
        public ClientAccountService(IClientDataAccess dataAccess, IMapper mapper, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
            _transaction = transaction;
        }

        public IServiceResult UpdateClientInfo(int clientID, ClientInfoUpdate clientInfoUpdate)
        {
            bool usernameEmpty = string.IsNullOrWhiteSpace(clientInfoUpdate.Username);
            Regex usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{5,60}$");
            bool emailEmpty = string.IsNullOrWhiteSpace(clientInfoUpdate.Email);
            Regex emailRegex = new Regex(@"^[a-zA-Z]([a-zA-Z0-9]|[\.\-_]){0,100}\@[a-z]{1,40}\.[a-z]{1,40}$");

            if (usernameEmpty && emailEmpty)
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Username and e-mail are null"));
            else if (!usernameEmpty && !usernameRegex.IsMatch(clientInfoUpdate.Username))
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Invalid (or too short/long) username"));
            else if (!emailEmpty && !emailRegex.IsMatch(clientInfoUpdate.Email))
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Invalid (or too short/long) e-mail"));

            _transaction.BeginTransaction();
            _dataAccess.UpdateClientInfo(clientID, clientInfoUpdate);
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
                    new ErrorView($"User with ID equal to {clientID} does not exist"));
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
                    new ErrorView("Both password and login properties must be included in the request"));
            }
            int? clientID = _dataAccess.GetRegisteredClientID(clientCredentials.Login, clientCredentials.Password);
            if(!clientID.HasValue)
            {
                return new ServiceResult(
                    HttpStatusCode.Unauthorized,
                    new ErrorView("User with provided login does not exist or provided password is incorrect"));
            }
            return new ServiceResult(HttpStatusCode.OK, new ClientToken(clientID.Value));
        }
    }
}
