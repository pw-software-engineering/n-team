using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Server.Authentication.Client;
using Server.Database.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using Xunit;

namespace Server.Tests.Authentication
{
    public static class TestSetup
    {
        static TestSetup()
        {
            var tokenDataAccessMock = new Mock<IClientTokenDataAccess>();
            tokenDataAccessMock.Setup(dataAccess => dataAccess.CheckIfClientExists(1)).Returns(true);
            tokenDataAccessMock.Setup(dataAccess => dataAccess.CheckIfClientExists(It.IsNotIn(new int[] { 1 }))).Returns(false);
            TokenManager = new ClientTokenManager(tokenDataAccessMock.Object);
        }

        public static IClientTokenManager TokenManager { get; }
    }

    public class ClientTokenHandlerTest : ClientTokenScheme
    {
        public ClientTokenHandlerTest() : base(TestSetup.TokenManager, null, null, null, null)
        {

        }

        //[Fact]
        //public void HandleAuthenticateAsync_EmptyHeaderToken_ReturnsAuthenticateNoResultAndSetsError()
        //{
            
        //}
    }
}
