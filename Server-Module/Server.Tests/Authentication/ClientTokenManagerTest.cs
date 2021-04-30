using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Server.Database.DataAccess;
using Microsoft.Extensions.Primitives;
using Server.Authentication.Client;
using System.Text.Json;
using Xunit.Abstractions;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Linq;

namespace Server.Tests.Authentication
{
    public class ClientTokenManagerTest
    {
        private Mock<IClientTokenDataAccess> _tokenDataAccessMock;
        private ClientTokenManager _clientTokenManager;
        public ClientTokenManagerTest(ITestOutputHelper output)
        {
            _tokenDataAccessMock = new Mock<IClientTokenDataAccess>();
            _clientTokenManager = new ClientTokenManager(_tokenDataAccessMock.Object);
        }

        [Fact]
        public void ParseTokenHeader_EmptyHeader_NullClientTokenAndNonNullError()
        {
            ClientToken token = _clientTokenManager.ParseTokenHeader(StringValues.Empty, out string parseError);
            Assert.Null(token);
            Assert.NotNull(parseError);
        }

        [Fact]
        public void ParseTokenHeader_MultipleValuesInHeader_NullClientTokenAndNonNullError()
        {
            StringValues tokenHeader = new StringValues(new string[] { "val1", "val2" });
            ClientToken token = _clientTokenManager.ParseTokenHeader(tokenHeader, out string parseError);
            Assert.Null(token);
            Assert.NotNull(parseError);
        }

        [Fact]
        public void ParseTokenHeader_SingleHeaderValueAndMalformedToken_NullClientTokenAndNonNullError()
        {
            StringValues tokenHeader = new StringValues("{id:9,badproperty:\"val\"}");
            ClientToken token = _clientTokenManager.ParseTokenHeader(tokenHeader, out string parseError);
            Assert.Null(token);
            Assert.NotNull(parseError);
        }

        [Fact]
        public void ParseTokenHeader_SingleHeaderValueCorrectToken_NonNullClientTokenAndNullError()
        {
            ClientToken token = new ClientToken(1);
            string tokenJSON = JsonSerializer.Serialize(
                token,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            );
            File.WriteAllText("C:/Users/CLEVO/Desktop/test-output.txt", tokenJSON);

            StringValues tokenHeader = new StringValues(tokenJSON);
            ClientToken resToken = _clientTokenManager.ParseTokenHeader(tokenHeader, out string parseError);
            Assert.NotNull(token);
            Assert.Null(parseError);
            Assert.Equal(token.ID, resToken.ID);
            Assert.Equal(token.CreatedAt, resToken.CreatedAt);
        }

        [Fact]
        public void ValidateToken_NullClientToken_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _clientTokenManager.ValidateToken(null, out string _));
        }

        [Fact]
        public void ValidateToken_ClientTokenContainsValidID_ReturnsTrue()
        {
            _tokenDataAccessMock.Setup(dataAccess => dataAccess.CheckIfClientExists(1)).Returns(true);
            ClientToken token = new ClientToken(1);
            Assert.True(_clientTokenManager.ValidateToken(token, out string _));
        }

        [Fact]
        public void ValidateToken_ClientTokenContainsInvalidID_ReturnsFalseAndErrorNonNull()
        {
            _tokenDataAccessMock.Setup(dataAccess => dataAccess.CheckIfClientExists(It.IsNotIn(new int[] { 1 }))).Returns(false);
            ClientToken token = new ClientToken(-1);
            Assert.False(_clientTokenManager.ValidateToken(token, out string validationError));
            Assert.NotNull(validationError);
        }

        [Fact]
        public void CreatePrincipal_NullClientToken_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _clientTokenManager.CreatePrincipal(null));
        }

        [Fact]
        public void CreatePrincipal_NonNullClientToken_ContainsIDClaimWithTheSameValue()
        {
            ClientToken token = new ClientToken(-1);
            ClaimsPrincipal principal = _clientTokenManager.CreatePrincipal(token);
            Assert.Equal(token.ID, int.Parse(principal.FindFirstValue("id")));
        }
    }
}
