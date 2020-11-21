using Xunit;
using RioParser.Domain.Extensions;
using FluentAssertions;

namespace RioParser.Domain.Tests.Extensions
{
    public class StringExtensionsTests
    {
        private readonly string STARTMARKER = "[Fox]";
        private readonly string ENDMARKER = "[caves]";

        [Theory]
        [InlineData("The [Fox] looks for chicken in [caves] along the river.", "looks for chicken in")]
        [InlineData("The [Fox] looks for chicken in [caves] along the river, if there are [caves].", "looks for chicken in")]
        [InlineData("The [Fox] of all [Fox]es looks for chicken in [caves] along the river, if there are [caves].", "of all [Fox]es looks for chicken in")]
        public void Finds_Substring_Between_Markers(string subject, string expected)
        {
            var result = subject.BetweenSingle(STARTMARKER, ENDMARKER);

            result.Should().Be(expected);
        }
    }
}
