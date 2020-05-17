using cmd.Commands;
using cmd.Runner;
using NSubstitute;
using Xunit;

namespace cmd.UnitTests.Runner.Shells
{
    public class PowershellDSLTests
    {
        private readonly dynamic cmd;
        private readonly IRunner _runner;


        public PowershellDSLTests()
        {
            _runner = Substitute.For<IRunner>();
            _runner.Run(Arg.Any<IRunOptions>()).Returns("result");
            _runner.GetCommand().Returns(new Commando(_runner));
            cmd = new Cmd(_runner);
        }

        [Fact]
        public void ShouldInvokeACmdlet()
        {
            cmd.Write.Host();
        }

        [Fact]
        public void ShouldInvokeACmdletWithArgument()
        {
            cmd.Write.Host("test");
        }

        [Fact]
        public void ShouldInvokeACmdletWithParameter()
        {
            cmd.Write.Host(Object: "test");
        }

        [Fact]
        public void ShouldInvokeACmdletWithSwitchFlag()
        {
            cmd.Write.Host(NoNewLine: true);
        }
    }
}