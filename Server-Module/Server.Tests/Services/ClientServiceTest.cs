using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using Moq;
using Server.Authentication.Client;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using Server.Models;
using Server.Services.ClientService;
using Server.Services.Result;
using Server.RequestModels;
using Xunit;
using Server.ViewModels;

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
            _transactionMock = new Mock<IDatabaseTransaction>();

            _clientService = new ClientService(_dataAccessMock.Object, _mapper, _transactionMock.Object);
        }
        private ClientService _clientService;
        private Mock<IClientDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;
        private IMapper _mapper;

        #region GetClientInfo
        [Fact]
        public void GetClientInfo_NonExistentClientID_400_NonNullError()
        {
            int clientID = -1;
            _dataAccessMock.Setup(da => da.GetClientInfo(clientID)).Returns((ClientInfoView)null);

            IServiceResult serviceResult = _clientService.GetClientInfo(clientID);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is Error);
        }

        [Fact]
        public void GetClientInfo_ValidClientID_200_ClientInfoViewObject()
        {
            ClientInfoView clientInfoView = new ClientInfoView()
            {
                Name = "TestName",
                Surname = "TestSurname",
                Email = "TestEmail",
                Username = "TestUsername"
            };
            int clientID = 1;
            _dataAccessMock.Setup(da => da.GetClientInfo(clientID)).Returns(clientInfoView);

            IServiceResult serviceResult = _clientService.GetClientInfo(clientID);
            ClientInfoView clientInfoViewResult = serviceResult.Result as ClientInfoView;

            Assert.Equal(HttpStatusCode.OK, serviceResult.StatusCode);
            Assert.Equal(clientInfoView.Name, clientInfoViewResult.Name);
            Assert.Equal(clientInfoView.Surname, clientInfoViewResult.Surname);
            Assert.Equal(clientInfoView.Email, clientInfoViewResult.Email);
            Assert.Equal(clientInfoView.Username, clientInfoViewResult.Username);
        }
        #endregion

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
            ClientToken clientToken = (ClientToken)result.Result;
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(clientID, clientToken.ID);
            _dataAccessMock.Verify(da => da.GetRegisteredClientID(username, password), Times.Once);
        }
        #endregion
    }
}