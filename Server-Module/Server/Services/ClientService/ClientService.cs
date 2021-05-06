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

        public IServiceResult UpdateClientInfo(int clientID, string username, string email)
        {
            bool usernameEmpty = string.IsNullOrWhiteSpace(username);
            Regex usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{5,60}$");
            bool emailEmpty = string.IsNullOrWhiteSpace(email);
            Regex emailRegex = new Regex(@"^[a-zA-Z]([a-zA-Z0-9]|[\.\-_]){0,100}\@[a-z]{1,40}\.[a-z]{1,40}$");

            if (usernameEmpty && emailEmpty)
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Username and e-mail are null"));
            else if (!usernameEmpty && !usernameRegex.IsMatch(username))
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Invalid (or too short/long) username"));
            else if (!emailEmpty && !emailRegex.IsMatch(email))
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Invalid (or too short/long) e-mail"));

            _transaction.BeginTransaction();
            _dataAccess.UpdateClientInfo(clientID, username, email);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult Login(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new Error("Both password and username properties must be included in the request"));
            }
            int? clientID = _dataAccess.GetRegisteredClientID(username, password);
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
