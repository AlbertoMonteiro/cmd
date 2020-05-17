using Xunit;
using cmd.Runner.Shells;

namespace cmd.UnitTests.Runner.Shells
{
    public class ShellTests
    {
        [Fact]
        public void ShouldReturnAProcessRunnerAsDEfaul()
        {
            Assert.NotNull(Shell.Default as ProcessRunner);
        }
        
        [Fact]
        public void ShouldReturnACmdShellRunner()
        {
            Assert.NotNull(Shell.Cmd as CmdShell);
        }
        
        [Fact]
        public void ShouldReturnAPowerShellRunner()
        {
            Assert.NotNull(Shell.Powershell as Posh);
        }

    }
}
