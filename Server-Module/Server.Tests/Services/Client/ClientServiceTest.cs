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
using Server.Services.Result;
using Server.RequestModels;
using Xunit;
using Server.ViewModels;
using Server.Database.DataAccess.Client;
using Server.Services.Client;
using Server.ViewModels.Client;
using Server.RequestModels.Client;

namespace Server.Tests.Services.Client
{
    public class ClientServiceTest
    {
        public ClientServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new ClientAutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _dataAccessMock = new Mock<IClientDataAccess>();
            _transactionMock = new Mock<IDatabaseTransaction>();

            _clientService = new ClientAccountService(_dataAccessMock.Object, _mapper, _transactionMock.Object);
        }
        private ClientAccountService _clientService;
        private Mock<IClientDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;
        private IMapper _mapper;

        #region GetClientInfo
        [Fact]
        public void GetClientInfo_NonExistentClientID_400_ErrorResult()
        {
            int clientID = -1;
            _dataAccessMock.Setup(da => da.GetClientInfo(clientID)).Returns((ClientInfoView)null);

            IServiceResult serviceResult = _clientService.GetClientInfo(clientID);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is ErrorView);
        }

        [Fact]
        public void GetClientInfo_ValidClientID_200_ClientInfoViewResult()
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
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = "",
                Email = null
            };
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, clientInfoUpdate));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, clientInfoUpdate);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, It.IsAny<ClientInfoUpdate>()), Times.Never());
            Assert.Equal("Username and e-mail are null", ((ErrorView)response.Result).Error);
        }
        [Fact]
        public void UpdateClientInfo_UsernameInvalid_400_UsernameInvalidFormatError()
        {
            int clientID = 2;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = "🐈",
                Email = "jelonek@melonek.eu"
            };
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, clientInfoUpdate));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, clientInfoUpdate);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, clientInfoUpdate), Times.Never());
            Assert.Equal("Invalid (or too short/long) username", ((ErrorView)response.Result).Error);
        }
        [Fact]
        public void UpdateClientInfo_UsernameOkEmailInvalid_400_EmailInvalidFormatError()
        {
            int clientID = 3;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = "jelonek",
                Email = "jelonekbezdomeny"
            };
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, clientInfoUpdate));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, clientInfoUpdate);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, clientInfoUpdate), Times.Never());
            Assert.Equal("Invalid (or too short/long) e-mail", ((ErrorView)response.Result).Error);
        }
        [Fact]
        public void UpdateClientInfo_UsernameOkEmailEmpty_200_UsernameChanged()
        {
            int clientID = 2;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = "jelonek",
                Email = ""
            };
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, clientInfoUpdate));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, clientInfoUpdate);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, clientInfoUpdate), Times.Once());

        }
        [Fact]
        public void UpdateClientInfo_UsernameEmptyEmailOk_200_EmailChanged()
        {
            int clientID = 3;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = null,
                Email = "jelonek@melonek.eu"
            };
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, clientInfoUpdate));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, clientInfoUpdate);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, clientInfoUpdate), Times.Once());
        }
        [Fact]
        public void UpdateClientInfo_UsernameAndEmailOk_200_UsernameEmailChanged()
        {
            int clientID = 1;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = "jelonek",
                Email = "jelonek@melonek.eu"
            };
            _dataAccessMock.Setup(da => da.UpdateClientInfo(clientID, clientInfoUpdate));

            IServiceResult response = _clientService.UpdateClientInfo(clientID, clientInfoUpdate);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.UpdateClientInfo(clientID, clientInfoUpdate), Times.Once());
        }
        #endregion

        #region Login
        [Fact]
        public void Login_ClientCredentialsArgumentNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _clientService.Login(null));
        }
        [Fact]
        public void Login_MissingOrEmptyClientLoginOrPasswordProperties_400()
        {
            ClientCredentials nullCredentials = new ClientCredentials();
            ClientCredentials emptyStringCredentials = new ClientCredentials()
            {
                Login = string.Empty,
                Password = string.Empty
            };
            ClientCredentials onlyPasswordEmptyStringCredentials = new ClientCredentials()
            {
                Login = "ValidLogin",
                Password = string.Empty
            };

            IServiceResult resultNull = _clientService.Login(nullCredentials);
            IServiceResult resultEmpty = _clientService.Login(emptyStringCredentials);
            IServiceResult resultPartial = _clientService.Login(onlyPasswordEmptyStringCredentials);

            Assert.Equal(HttpStatusCode.BadRequest, resultNull.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, resultEmpty.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, resultPartial.StatusCode);
        }

        [Fact]
        public void Login_ClientLoginAndPasswordDoNotExistOrIncorrect_401()
        {
            ClientCredentials clientCredentials = new ClientCredentials()
            {
                Login = "NonexistentLogin#@!",
                Password = "password123#@!"
            };
            _dataAccessMock.Setup(da => da.GetRegisteredClientID(clientCredentials.Login, clientCredentials.Password)).Returns((int?)null);

            IServiceResult result = _clientService.Login(clientCredentials);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            _dataAccessMock.Verify(da => da.GetRegisteredClientID(clientCredentials.Login, clientCredentials.Password), Times.Once);
        }

        [Fact]
        public void Login_ClientLoginAndPasswordValid_200_ClientToken()
        {
            ClientCredentials clientCredentials = new ClientCredentials()
            {
                Login = "ValidLogin",
                Password = "ValidPassword"
            };
            int clientID = 1;
            _dataAccessMock.Setup(da => da.GetRegisteredClientID(clientCredentials.Login, clientCredentials.Password)).Returns(clientID);

            IServiceResult result = _clientService.Login(clientCredentials);
            ClientToken clientToken = (ClientToken)result.Result;

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(clientID, clientToken.ID);
            _dataAccessMock.Verify(da => da.GetRegisteredClientID(clientCredentials.Login, clientCredentials.Password), Times.Once);
        }
        #endregion
    }
}