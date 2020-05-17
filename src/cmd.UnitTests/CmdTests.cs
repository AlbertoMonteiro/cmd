using cmd.Commands;
using cmd.Runner;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace cmd.UnitTests
{
    public class CmdTests
    {
        private readonly dynamic cmd;
        private readonly IRunner _runner;

        public CmdTests()
        {
            _runner = Substitute.For<IRunner>();
            _runner.GetCommand().Returns(new Commando(_runner));
            cmd = new Cmd(_runner);
        }

        [Fact]
        public void ShouldBeAbleToBuildACommandAsProperty()
        {
            var commando = cmd.git;

            Assert.NotNull(commando);
        }

        [Fact]
        public void ShouldCreateCommandWithRunner()
        {
            cmd.git();

            _runner.Received().Run(Arg.Any<IRunOptions>());
        }

        [Fact]
        public void ShouldBeAbleToBuildMultipleCommandsOnCmd()
        {
            _runner.GetCommand().Returns(new Commando(_runner));
            var git = cmd.git;
            _runner.GetCommand().Returns(new Commando(_runner));
            var svn = cmd.svn;

            Assert.NotEqual(git, svn);
        }

        [Fact]
        public void ShouldBeAbleToRunMultipleCommandsOnCmd()
        {
            _runner.GetCommand().Returns(new Commando(_runner));
            cmd.git();
            _runner.GetCommand().Returns(new Commando(_runner));
            cmd.svn();

            _runner.Received().Run(Arg.Is<IRunOptions>(options => options.Command == "git"));
            _runner.Received().Run(Arg.Is<IRunOptions>(options => options.Command == "svn"));
        }

        [Fact]
        public void ShouldBeAbleToSetEnvironmentVariablesOnCmd()
        {
            var environmentDictionary = new Dictionary<string, string> { { "PATH", @"C:\" } };
            cmd._Env(environmentDictionary);

            _runner.Received().EnvironmentVariables = environmentDictionary;
        }
    }
}