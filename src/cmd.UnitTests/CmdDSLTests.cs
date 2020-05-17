using cmd.Commands;
using cmd.Runner;
using cmd.Runner.Shells;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace cmd.UnitTests
{
    public class CmdDslTests
    {
        private readonly dynamic cmd;
        private readonly IRunner _runner;

        public CmdDslTests()
        {
            _runner = Substitute.For<IRunner>();
            _runner.Run(Arg.Any<IRunOptions>()).Returns("result");
            _runner.GetCommand().Returns(new Commando(_runner));
            cmd = new Cmd(_runner);
        }

        [Fact]
        public void ShouldBeAbleToCallArbitraryCommandOnCmd()
        {
            cmd.git();
        }

        [Fact]
        public void ShouldBeAbleToCallArbitrarySubCommand()
        {
            var clone = cmd.git.clone;
        }

        [Fact]
        public void ShouldBeAbleToExecuteWithSubCommand()
        {
            cmd.git.Clone();
        }

        [Fact]
        public void ShouldBeAbleToPassArgumentsToACommand()
        {
            cmd.Git.Clone("http://github.com/manojlds/cmd");
        }

        [Fact]
        public void ShouldBeAbleToPassFlags()
        {
            cmd.Git.Pull(r: true);
        }

        [Fact]
        public void ShouldBeAbleToPassArgumentstoFlags()
        {
            cmd.Git.Checkout(b: "master");
        }

        [Fact]
        public void ShouldBeAbleToPreBuildACommandAndThenExecuteIt()
        {
            var git = cmd.git;
            git.Clone();
        }

        [Fact]
        public void ShouldBeAbleToBuildMultipleCommandOnCmd()
        {
            var git = cmd.git;
            var svn = cmd.svn;
        }

        [Fact]
        public void ShouldBeAbleToRunMultipleCommandOnCmd()
        {
            cmd.git();
            cmd.svn();
        }

        [Fact]
        public void ShouldBeAbleToChooseADifferentShell()
        {
            dynamic cmd = new Cmd(Shell.Cmd);
        }

        [Fact]
        public void ShouldBeAbleToSetEnvironmentVariables()
        {
            cmd._Env(new Dictionary<string, string> { { "PATH", @"C:\" } });
        }
    }
}
