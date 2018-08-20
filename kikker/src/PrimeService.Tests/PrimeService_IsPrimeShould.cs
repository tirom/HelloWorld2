using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PrimeService.Tests
{
    public class PrimeService_IsPrimeShould
    {
		private readonly PrimeService _primeService;

		public PrimeService_IsPrimeShould()
		{
			_primeService = new PrimeService();
		}

		
		/// <summary>
		/// [Theory] represents a suite of tests that execute the same code but have different input arguments.
		/// [InlineData] attribute specifies values for those inputs.
		/// </summary>
		/// <param name="value"></param>
		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		public void ReturnFalseGivenValuesLessThan2(int value)
		{
			var result = _primeService.IsPrime(value);

			Assert.False(result, $"{value} should not be prime");
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(5)]
		[InlineData(7)]
		public void ReturnTrueGivenPrimesLessThan10(int value)
		{
			var result = _primeService.IsPrime(value);

			Assert.True(result, $"{value} should be prime");
		}

		[Theory]
		[InlineData(4)]
		[InlineData(6)]
		[InlineData(8)]
		[InlineData(9)]
		public void ReturnFalseGivenNonPrimesLessThan10(int value)
		{
			var result = _primeService.IsPrime(value);

			Assert.False(result, $"{value} should not be prime");
		}
	}
}
