using cmd.Runner.Arguments;
using Xunit;

namespace cmd.UnitTests.Runner.Arguments
{
    public class PowershellArgumentBuilderTests
    {
        private readonly IArgumentBuilder argumentBuilder;


        public PowershellArgumentBuilderTests()
        {
            argumentBuilder = new PowershellArgumentBuilder();
        }

        [Fact]
        public void ShouldSetSwitchFlag()
        {
            var argument = new Argument("Force", null);

            var builtArgument = argumentBuilder.Build(argument);

            Assert.Equal("-Force", builtArgument);
        }

    }
}