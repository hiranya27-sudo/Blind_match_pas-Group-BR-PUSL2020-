using BlindMatchPAS.Core.Enums;
using Xunit;

namespace BlindMatchPAS.Tests.Unit
{
    public class ProjectStatusTests
    {
        [Fact]
        public void ProjectStatus_HasPendingValue()
        {
            Assert.Equal("Pending", ProjectStatus.Pending.ToString());
        }

        [Fact]
        public void MatchStatus_HasConfirmedValue()
        {
            Assert.Equal("Confirmed", MatchStatus.Confirmed.ToString());
        }
    }
}