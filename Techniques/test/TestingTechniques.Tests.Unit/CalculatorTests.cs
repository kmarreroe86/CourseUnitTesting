using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using TestingTechniques;
using Xunit;
using Xunit.Abstractions;

namespace CalculatorLibrary.Tests
{
    public class CalculatorTests : /*IDisposable*/ IAsyncLifetime
    {

        private readonly Calculator _sut = new(); // system under test
        private readonly ITestOutputHelper _output;

        public CalculatorTests(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("Hello from the ctor");
        }


        [Theory]
        //[Theory(Skip ="This break in CI")]
        [InlineData(5, 4, 9)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(-1, -1, -2)]
        [InlineData(-5, 5, 0)]
        [InlineData(-15, -5, -20)]
        [InlineData(-1, -1, 2, Skip = "This is a wrong data, will skip it")]
        public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
        {
            // Arrange
            //var calculator = new Calculator();
            _output.WriteLine($"Hello from the test add {a}, {b}, {expected}");

            // Act
            var result = _sut.Add(a, b);

            // Assert
            result.Should().Be(expected);
        }


        [Fact]
        //[Fact(Skip = "This is a dummy test")] // skip the test
        public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers()
        {
            _output.WriteLine("Hello from the test subtract");

            // Act
            var result = _sut.Subtract(4, 5);

            // Assert
            result.Should().Be(-1);
        }

        [Theory]
        [InlineData(5, 5, 0)]
        [InlineData(15, 5, 10)]
        [InlineData(-15, -5, -10)]
        public void SubtractTheory_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
        {

            // Act
            var result = _sut.Subtract(a, b);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 5, 25)]
        [InlineData(50, 0, 0)]
        [InlineData(-5, 5, -25)]
        public void Multiplaying_ShoudMultiplyTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
        {
            var result = _sut.Multiply(a, b);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 5, 1)]
        [InlineData(15, 5, 3)]
        public void Divide_ShoudDivideTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
        {
            var result = _sut.Divide(a, b);

            result.Should().Be(expected);
        }



        /*public void Dispose()
        {
            _output.WriteLine("Hello from Dispose, cleanup");
        }*/


        public async Task InitializeAsync()
        {
            _output.WriteLine("Hello from InitializeAsync");
            await Task.Delay(1);
        }

        public async Task DisposeAsync()
        {
            _output.WriteLine("Hello from DisposeAsync, cleanup");
            await Task.Delay(1);
        }
    }
}
