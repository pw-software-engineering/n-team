using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Net.Http;
using Xunit;

namespace Server.Tests
{
	public class PowerOfTests
	{
		[Fact]
		public void AuthTest()
        {/*
			var mocks = new MockRepository();
			var controller = new  Controllers.Hotel.HotelAccountController(null);
			var httpContext = HttpContext(mocks, true);
			controller.ControllerContext = new ControllerContext
			{
				Controller = controller,
				RequestContext = new RequestContext(httpContext, new RouteData())
			};

			httpContext.User.Expect(u => u.IsInRole("User")).Return(false);
			mocks.ReplayAll();

			// Act
			var result =
				controller.ActionInvoker.InvokeAction(controller.ControllerContext, "Index");
			var statusCode = httpContext.Response.StatusCode;

			// Assert
			Assert.IsTrue(result, "Invoker Result");
			Assert.AreEqual(401, statusCode, "Status Code");
			mocks.VerifyAll();*/
		}

		public class IsPowerOf
		{
			public bool IsPowerOfTwo(uint candidate) => ((candidate - 1u) & candidate) == 0u;
			public bool IsPowerOfFour(uint candidate)
			{
				if (candidate == 0u) return true;

				uint zeromask = 0x3u;
				if (!IsPowerOfTwo(candidate)) return false;
				while ((candidate & zeromask) == 0u) candidate >>= 2;

				return (candidate & zeromask) == 1u;
			}
		}

		readonly IsPowerOf PowerTester = new IsPowerOf();

		[
			Theory,
			InlineData(2u), InlineData(4u), InlineData(0u), InlineData(512u), InlineData(65536u)
		]
		public void PowersOf2TrueForPowerOf2Test(uint candidate) => Assert.True(PowerTester.IsPowerOfTwo(candidate), $"{candidate} should be a power of two");

		[
			Theory,
			InlineData(4u), InlineData(0u), InlineData(256u), InlineData(65536u), InlineData(4096u)
		]
		public void PowersOf4TrueForPowerOf4Test(uint candidate) => Assert.True(PowerTester.IsPowerOfFour(candidate), $"{candidate} should be a power of four");
		[
			Theory,
			InlineData(7u), InlineData(7777777u), InlineData(58732956u), InlineData(65u), InlineData(18u)
		]
		public void RandomNumsFalseForPowerOf2Test(uint candidate) => Assert.False(PowerTester.IsPowerOfTwo(candidate), $"{candidate} should NOT be a power of two");
		[
			Theory,
			InlineData(2u), InlineData(8u), InlineData(32u), InlineData(512u), InlineData(32768u)
		]
		public void PowersOf2ButNot4FalseForPowerOf4Test(uint candidate) => Assert.True(PowerTester.IsPowerOfTwo(candidate) & !PowerTester.IsPowerOfFour(candidate), $"{candidate} should be a power of two but NOT four");
	}
}
