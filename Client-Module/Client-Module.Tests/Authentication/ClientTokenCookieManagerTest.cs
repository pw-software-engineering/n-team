using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Client_Module.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;

namespace ClientModule.Tests.Authentication
{
    delegate void GetClientInfoCallback(string cookieToken, out string error);
    delegate ClientInfo GetClientInfoReturns(string cookieToken, out string error);
    public class ClientTokenCookieManagerTest
    {
        private IServiceProvider _serviceProvider;
        private Mock<IClientInfoAccessor> _mockAccessor;
        public ClientTokenCookieManagerTest(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Mock<IClientInfoAccessor> mockAccessor = new Mock<IClientInfoAccessor>();
            _mockAccessor = mockAccessor;
        }
        [Fact]
        public void ValidateCookieToken_CookieTokenNullOrEmpty_ReturnsNullAndNonNullError()
        {
            ClientCookieTokenManager manager = new ClientCookieTokenManager(_mockAccessor.Object);
            
            string errorNull, errorEmpty;
            ClientInfo clientInfoNull = manager.ValidateCookieToken(null, out errorNull);
            ClientInfo clientInfoEmpty = manager.ValidateCookieToken(string.Empty, out errorEmpty);

            Assert.Null(clientInfoNull);
            Assert.NotNull(errorNull);
            Assert.Null(clientInfoEmpty);
            Assert.NotNull(errorEmpty);
        }

        [Fact]
        public void ValidateCookieToken_CookieTokenMalformed_ReturnsNullAndNonNullError()
        {
            _mockAccessor.Setup(mock => mock.GetClientInfo("malformed", out It.Ref<string>.IsAny))
                .Callback(new GetClientInfoCallback(
                    (string token, out string error) =>
                    {
                        error = "Token is malformed.";
                    }))
                .Returns((ClientInfo)null);
            ClientCookieTokenManager manager = new ClientCookieTokenManager(_mockAccessor.Object);
            string cookieToken = "malformed";

            string error;
            ClientInfo clientInfo = manager.ValidateCookieToken(cookieToken, out error);

            Assert.Null(clientInfo);
            Assert.NotNull(error);
        }

        [Fact]
        public void ValidateCookieToken_CookieTokenValid_ReturnsClientInfoAndNullError()
        {
            ClientInfo clientInfo = new ClientInfo()
            {
                Name = "name1",
                Surname = "surname1",
                Email = "mail@example.com",
                Username = "username1"
            };
            string clientInfoJSON = JsonConvert.SerializeObject(
                clientInfo,
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            _mockAccessor.Setup(mock => mock.GetClientInfo(clientInfoJSON, out It.Ref<string>.IsAny))
                .Callback(new GetClientInfoCallback(
                    (string token, out string error) =>
                    {
                        error = null;
                    }))
                .Returns(clientInfo);
            ClientCookieTokenManager manager = new ClientCookieTokenManager(_mockAccessor.Object);
            string cookieToken = clientInfoJSON;

            ClientInfo clientInfoRes = manager.ValidateCookieToken(cookieToken, out string validationError);
            Assert.Null(validationError);
            Assert.Equal(clientInfoRes, clientInfo);
        }

        [Fact]
        public void CreatePrincipal_NullClientInfo_ThrowsArgumentNullException()
        {
            ClientCookieTokenManager manager = new ClientCookieTokenManager(_mockAccessor.Object);

            Assert.Throws<ArgumentNullException>(() => manager.CreatePrincipal(null));
        }

        [Fact]
        public void CreatePrincipal_NonNullClientInfo_ContainsClaimsWithAllClientInfoData()
        {
            ClientInfo clientInfo = new ClientInfo()
            {
                Name = "name1",
                Surname = "surname1",
                Email = "mail@example.com",
                Username = "username1"
            };
            ClientCookieTokenManager manager = new ClientCookieTokenManager(_mockAccessor.Object);

            ClaimsPrincipal principal = manager.CreatePrincipal(clientInfo);

            Assert.Equal("name1", principal.FindFirst("name").Value);
            Assert.Equal("surname1", principal.FindFirst("surname").Value);
            Assert.Equal("mail@example.com", principal.FindFirst("email").Value);
            Assert.Equal("username1", principal.FindFirst("username").Value);
        }
    }
}
