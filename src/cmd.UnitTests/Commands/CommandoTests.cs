using cmd.Commands;
using cmd.Runner;
using cmd.Runner.Arguments;
using NSubstitute;
using Xunit;

namespace cmd.UnitTests.Commands
{
    public class CommandoTests
    {
        private readonly IRunner _runner;
        private readonly dynamic cmd;

        public CommandoTests()
        {
            _runner = Substitute.For<IRunner>();
            _runner.GetCommand().Returns(new Commando(_runner));
            cmd = new Cmd(_runner);
        }

        [Fact]
        public void ShouldBeAbleToBuildACommand()
        {
            var command = cmd.git;
        }

        [Fact]
        public void ShouldBeAbleToRunACommand()
        {
            cmd.git();

            _runner.Received().Run(Arg.Is<IRunOptions>(options => options.Command == "git" && options.Arguments == string.Empty));
        }

        [Fact]
        public void ShouldBeAbleToGetOutputFromCommand()
        {
            _runner.Run(Arg.Any<IRunOptions>()).Returns("out");

            var output = cmd.git();

            Assert.Equal(output, "out");
        }

        [Fact]
        public void ShouldBeAbleToBuildSubCommands()
        {
            var command = cmd.git.clone;
        }

        [Fact]
        public void ShouldBeAbleToRunWithSubCommand()
        {
            IRunOptions expectedRunOptions = null;
            _runner.Run(Arg.Any<IRunOptions>())
                .ReturnsForAnyArgs("")
                .AndDoes(info => expectedRunOptions = info.Arg<IRunOptions>());

            cmd.git.clone();

            Assert.NotNull(expectedRunOptions);
            Assert.Equal("git", expectedRunOptions.Command);
            Assert.Equal("clone", expectedRunOptions.Arguments);
        }

        [Fact]
        public void ShouldBeAbleToRunWithArgumentsOnCommand()
        {
            const string Argument = "--help";
            IRunOptions expectedRunOptions = null;
            _runner.BuildArgument(Arg.Any<Argument>()).Returns(Argument);
            _runner.Run(Arg.Any<IRunOptions>())
                .ReturnsForAnyArgs("")
                .AndDoes(info => expectedRunOptions = info.Arg<IRunOptions>());

            cmd.git(help: true);

            Assert.NotNull(expectedRunOptions);
            Assert.Equal("git", expectedRunOptions.Command);
            Assert.Equal(expectedRunOptions.Arguments, Argument);
        }

        [Fact]
        public void ShouldBeAbleToRunWithArgumentsOnSubCommand()
        {
            const string Argument = "https://github.com/manojlds/cmd";
            IRunOptions expectedRunOptions = null;
            _runner.BuildArgument(Arg.Any<Argument>()).Returns(Argument);
            
            _runner.Run(Arg.Any<IRunOptions>())
                .ReturnsForAnyArgs("")
                .AndDoes(info => expectedRunOptions = info.Arg<IRunOptions>());

            cmd.git.clone(Argument);

            Assert.NotNull(expectedRunOptions);
            Assert.Equal("git", expectedRunOptions.Command);
            Assert.Equal(expectedRunOptions.Arguments, string.Concat("clone ", Argument));
        }

        [Fact]
        public void ShouldBeAbleToCallMultipleCommandsWithPreBuiltCommando()
        {
            IRunOptions branchRunOptions = null;
            _runner.Run(Arg.Is<IRunOptions>(options => options.Arguments.StartsWith("branch")))
            .ReturnsForAnyArgs("")
                .AndDoes(info => branchRunOptions = info.Arg<IRunOptions>());

            IRunOptions cloneRunOptions = null;
            _runner.Run(Arg.Is<IRunOptions>(options => options.Arguments.StartsWith("clone")))
            .ReturnsForAnyArgs("")
                .AndDoes(info => cloneRunOptions = info.Arg<IRunOptions>());

            var git = cmd.git;
            git.Clone();
            git.branch(async: true);

            Assert.NotNull(branchRunOptions);
            Assert.NotNull(cloneRunOptions);
        }
    }
}
