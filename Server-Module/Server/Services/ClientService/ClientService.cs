﻿using Server.Database;
using Server.Exceptions;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels;
using Server.Database.DataAccess;
using AutoMapper;
using Server.Services.Response;
using System.Net;
using System.Text.RegularExpressions;

namespace Server.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly IClientDataAccess _dataAccess;
        private readonly IMapper _mapper;
        public ClientService(IClientDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
        }
        
        public IServiceResult UpdateClientInfo(int clientID, string username, string email)
        {
            bool usernameEmpty = string.IsNullOrWhiteSpace(username);
            Regex usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{5,30}$");
            bool emailEmpty = string.IsNullOrWhiteSpace(email);
            Regex emailRegex = new Regex(@"^[a-zA-Z]([a-zA-Z0-9]|[\.-_]){0,50}@[a-z]{1,10}\.[a-z]{1,10}$");

            if (usernameEmpty && emailEmpty) return new ServiceResult(HttpStatusCode.BadRequest, "Username and e-mail are null");
            else if (!(usernameEmpty || usernameRegex.IsMatch(username))) return new ServiceResult(HttpStatusCode.BadRequest, "Invalid (or too short/long) username");
            else if (!(emailEmpty || emailRegex.IsMatch(email))) return new ServiceResult(HttpStatusCode.BadRequest, "Invalid (or too short/long) e-mail");
            _dataAccess.UpdateClientInfo(clientID, username, email);

            return new ServiceResult(HttpStatusCode.OK);
        }
    }
}
