using System;
using Xunit;

namespace Hotel.Tests
{
	public class PowerOfTests
	{
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
