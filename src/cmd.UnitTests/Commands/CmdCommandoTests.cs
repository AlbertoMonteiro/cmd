using cmd.Commands;
using cmd.Runner;
using NSubstitute;
using Xunit;

namespace cmd.UnitTests.Commands
{

    public class CmdCommandoTests
    {
        private readonly IRunner _runner;
        private readonly dynamic cmd;


        public CmdCommandoTests()
        {
            _runner = Substitute.For<IRunner>();
            _runner.GetCommand().Returns(new CmdCommando(_runner));
            cmd = new Cmd(_runner);
        }

        [Fact]
        public void ShouldRunTheCommandAgainstCmd()
        {
            IRunOptions expectedRunOptions = null;
            _runner.Run(Arg.Any<IRunOptions>())
                .ReturnsForAnyArgs("")
                .AndDoes(info => expectedRunOptions = info.Arg<IRunOptions>());

            cmd.dir();

            Assert.Equal("cmd", expectedRunOptions.Command);
            Assert.Equal("/c dir", expectedRunOptions.Arguments);
        }
    }
}