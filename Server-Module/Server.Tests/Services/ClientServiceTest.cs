using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Exceptions;
using Server.Models;
using Server.Services.ClientService;
using Server.Services.Result;
using Server.RequestModels;
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

		#region Patch
        [Fact]
		public void UpdateClientInfo_UsernameAndEmailEmpty_400_UsernameAndEmailEmptyError()
		{
            int clientID = 1;
            string username = "", email = null;
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, username, email));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, username, email);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            Assert.Equal("Username and e-mail are null", ((Error)response.Result).error);
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
            Assert.Equal("Invalid (or too short/long) username", ((Error)response.Result).error);
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
            Assert.Equal("Invalid (or too short/long) e-mail", ((Error)response.Result).error);
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
    }
}