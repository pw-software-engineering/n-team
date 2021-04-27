using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using Server.Authentication;
using Server.Database.DataAccess;
using Server.Tests.Database;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Encodings.Web;
using Xunit;

namespace Server.Tests
{
	public class PowerOfTests
	{
		public class HotelAuthTestClass
        {
			private Mock<IHotelTokenDataAccess> hotelTokenDataAcess;
            private Mock<IOptionsMonitor<HotelTokenSchemeOptions>> options;
			private Mock<Microsoft.Extensions.Logging.ILoggerFactory> logger;
			private Mock<UrlEncoder> encoder;
			private Mock<ISystemClock> clock;
			private readonly HotelTokenScheme hotelTokenScheme;
			
			public HotelAuthTestClass()
            {
				options = new Mock<IOptionsMonitor<HotelTokenSchemeOptions>>();
				options.Setup(x => x.Get(It.IsAny<string>())).Returns(new HotelTokenSchemeOptions());
				hotelTokenDataAcess = new Mock<IHotelTokenDataAccess>();
				logger = new Mock<Microsoft.Extensions.Logging.ILoggerFactory>();
				encoder = new Mock<UrlEncoder>();
				clock = new Mock<ISystemClock>();
				hotelTokenScheme = new HotelTokenScheme(hotelTokenDataAcess.Object, options.Object, logger.Object	, encoder.Object, clock.Object);
			}

			[Fact]
            public async void HotelBadAuthTest()
            {
				var a = new HttpClient();
				a.BaseAddress = new Uri("https://127.0.0.1:5000");
				a.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("a","a");
				
				var contex = new DefaultHttpContext();
				//contex.Request.Headers.Clear();
				var p1 = a.DefaultRequestHeaders.GetEnumerator();
				while (p1.MoveNext())
				{
					System.String[] y2 = (string[])p1.Current.Value;
					var t1 = new Microsoft.Extensions.Primitives.StringValues(y2);

					contex.Request.Headers.Add(p1.Current.Key, t1);
				}
				
				var d = new AuthenticationScheme(/*HotelTokenDefaults.AuthenticationScheme*/"HotelTokenScheme", "A", typeof(HotelTokenScheme));
				await hotelTokenScheme.InitializeAsync(d, contex);
				var w = await hotelTokenScheme.AuthenticateAsync();
				
				Assert.True(!w.Succeeded);
			}
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
