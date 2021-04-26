using Server.Database;
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
using Server.Services.Result;

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
            // todo: check if email is of correct form


            return _dataAccess.UpdateClientInfo(clientID, username, email)
                ? new ServiceResult(HttpStatusCode.OK) 
                : new ServiceResult(HttpStatusCode.BadRequest);
        }
    }
}
