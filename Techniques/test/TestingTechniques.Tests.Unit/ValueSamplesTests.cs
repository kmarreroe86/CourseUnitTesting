using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TestingTechniques.Tests.Unit
{
    public class ValueSamplesTests
    {

        private readonly ValueSamples _sut = new();


        [Fact]
        public void StringAssertingExample()
        {
            var fullName = _sut.FullName;

            fullName.Should().Be("Nick Chaspas");
            fullName.Should().NotBeEmpty();
            fullName.Should().StartWith("Nick");
            fullName.Should().EndWith("Chaspas");
            fullName.Should().HaveLength(12);
        }


        [Fact]
        public void NumbersAssertionExample()
        {

            var age = _sut.Age;

            age.Should().Be(21);
            age.Should().BePositive();
            age.Should().BeGreaterThan(18);
            age.Should().BeLessOrEqualTo(60);
            age.Should().BeInRange(18, 60);

        }


        [Fact]
        public void DateAssertionExample()
        {
            var dateOfBirth = _sut.DateOfBirth;

            dateOfBirth.Should().Be(new(2000, 6, 9));
            dateOfBirth.Should().BeInRange(new(2000, 1, 1), new(2001, 1, 1));
            dateOfBirth.Should().BeGreaterThan(new(2000, 1, 1));
        }


        [Fact]
        public void ObjectAssertionsExample()
        {
            var expected = new User
            {
                FullName = "Nick Chaspas",
                Age = 21,
                DateOfBirth = new(200, 6, 9)
            };

            var user = _sut.AppUser;

            user.Should().BeEquivalentTo(expected);

        }


        [Fact]
        public void EnumerableObjectAssertionsExample()
        {
            var expected = new User
            {
                FullName = "Nick Chaspas",
                Age = 21,
                DateOfBirth = new(2000, 6, 9)
            };

            var users = _sut.Users.As<User[]>();

            users.Should().ContainEquivalentOf(expected);
            users.Should().HaveCount(3);
            users.Should().Contain(x => x.FullName.StartsWith("Nick") && x.Age > 20);
        }


        [Fact]
        public void EnumerableNumbersExample()
        {

            var numbers = _sut.Numbers.As<int[]>();
            numbers.Should().Contain(5);
        }


        [Fact]
        public void ExceptionThrownAssertionExample()
        {
            var calculator = new Calculator();

            // var result = ;
            Action result = () => calculator.Divide(3, 0);

            // result.Should().Be(0);
            result.Should()
            .Throw<DivideByZeroException>()
            .WithMessage("Attempted to divide by zero.");
        }


        [Fact]
        public void EventRaisedAssertionExample()
        {
            var monitorSubject = _sut.Monitor();

            _sut.RaiseExampleEvent();

            monitorSubject.Should().Raise("ExampleEvent");

        }


        [Fact]
        public void TestingInternalMembersExample()
        {
            var number = _sut.InternalSecretNumber;

            number.Should().Be(42);
        }
    }
}