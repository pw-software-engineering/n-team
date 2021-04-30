﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using Moq;
using Server.Authentication.Client;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Exceptions;
using Server.Models;
using Server.Services.ClientService;
using Server.Services.Response;
using Server.Services.Result;
using Server.ViewModels;
using Xunit;

namespace Server.Tests.Services
{
    public class ClientServiceTest
    {
        public ClientServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _dataAccessMock = new Mock<IClientDataAccess>();

            _clientService = new ClientService(_dataAccessMock.Object, _mapper);
        }
        private ClientService _clientService;
        private Mock<IClientDataAccess> _dataAccessMock;
        private IMapper _mapper;

		#region UpdateClientInfo
        [Fact]
		public void UpdateClientInfo_UsernameAndEmailEmpty_400_UsernameAndEmailEmptyError()
		{
            int clientID = 1;
            string username = "", email = null;
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, username, email));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, username, email);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            Assert.Contains(@"errorMessage = Username and e-mail are null", response.ResponseBody.ToString());
        }
        [Fact]
        public void UpdateClientInfo_UsernameInvalid_400_UsernameInvalidFormatError()
        {
            int clientID = 2;
            string username = "🐈", email = "jelonek@melonek.eu";
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, username, email));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, username, email);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, username, email), Times.Never());
            Assert.Contains(@"errorMessage = Invalid (or too short/long) username", response.ResponseBody.ToString());
        }
        [Fact]
        public void UpdateClientInfo_UsernameOkEmailInvalid_400_EmailInvalidFormatError()
        {
            int clientID = 3;
            string username = "jelonek", email = "jelonekbezdomeny";
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, username, email));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, username, email);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, username, email), Times.Never());
            Assert.Contains(@"errorMessage = Invalid (or too short/long) e-mail", response.ResponseBody.ToString());
        }
        [Fact]
        public void UpdateClientInfo_UsernameOkEmailEmpty_200_UsernameChanged()
        {
            int clientID = 2;
            string username = "jelonek", email = "";
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, username, email));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, username, email);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, username, email), Times.Once());

        }
        [Fact]
        public void UpdateClientInfo_UsernameEmptyEmailOk_200_EmailChanged()
        {
            int clientID = 3;
            string username = null, email = "jelonek@melonek.eu";
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, username, email));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, username, email);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, username, email), Times.Once());
        }
        [Fact]
        public void UpdateClientInfo_UsernameAndEmailOk_200_UsernameEmailChanged()
        {
            int clientID = 1;
            string username = "jelonek", email = "jelonek@melonek.eu";
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, username, email));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, username, email);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, username, email), Times.Once());
        }
        #endregion

        #region Login
        [Fact]
        public void Login_MissingOrEmptyUsernameOrPasswordProperties_400()
        {
            IServiceResult resultNull = _clientService.Login(null, null);
            IServiceResult resultEmpty = _clientService.Login(string.Empty, string.Empty);
            IServiceResult resultPartial = _clientService.Login("ValidUsername", string.Empty);

            Assert.Equal(HttpStatusCode.BadRequest, resultNull.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, resultEmpty.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, resultPartial.StatusCode);
        }

        [Fact]
        public void Login_UsernameAndPasswordDoNotExistOrIncorrect_401()
        {
            string username = "NonexistentUsername#@!";
            string password = "password123#@!";
            _dataAccessMock.Setup(da => da.GetRegisteredClientID(username, password)).Returns((int?)null);

            IServiceResult result = _clientService.Login(username, password);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            _dataAccessMock.Verify(da => da.GetRegisteredClientID(username, password), Times.Once);
        }

        [Fact]
        public void Login_UsernameAndPasswordValid_200_ClientToken()
        {
            string username = "ValidUsername";
            string password = "ValidPassword";
            int clientID = 1;
            _dataAccessMock.Setup(da => da.GetRegisteredClientID(username, password)).Returns(clientID);

            IServiceResult result = _clientService.Login(username, password);
            ClientToken clientToken = (ClientToken)result.ResponseBody;
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(clientID, clientToken.ID);
            _dataAccessMock.Verify(da => da.GetRegisteredClientID(username, password), Times.Once);
        }
        #endregion
    }
}