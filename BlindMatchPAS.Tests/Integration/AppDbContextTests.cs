using BlindMatchPAS.Core.Entities;
using BlindMatchPAS.Core.Enums;
using BlindMatchPAS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlindMatchPAS.Tests.Integration
{
    public class AppDbContextTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Can_Add_ResearchArea()
        {
            using var context = GetDbContext();

            context.ResearchAreas.Add(new ResearchArea { Name = "AI" });
            await context.SaveChangesAsync();

            Assert.Single(context.ResearchAreas);
        }

        [Fact]
        public async Task Can_Add_Project()
        {
            using var context = GetDbContext();

            var area = new ResearchArea { Name = "Software Engineering" };
            context.ResearchAreas.Add(area);
            await context.SaveChangesAsync();

            var project = new Project
            {
                Title = "Test Proposal",
                Abstract = "Testing project insert",
                ResearchAreaId = area.Id,
                StudentId = "student-1",
                Status = ProjectStatus.Pending
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            Assert.Single(context.Projects);
        }

        [Fact]
        public async Task Can_Add_Match()
        {
            using var context = GetDbContext();

            var area = new ResearchArea { Name = "Cybersecurity" };
            context.ResearchAreas.Add(area);
            await context.SaveChangesAsync();

            var project = new Project
            {
                Title = "Security Proposal",
                Abstract = "Security abstract",
                ResearchAreaId = area.Id,
                StudentId = "student-1",
                Status = ProjectStatus.Pending
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var match = new Match
            {
                ProjectId = project.Id,
                SupervisorId = "supervisor-1",
                Status = MatchStatus.Interested
            };

            context.Matches.Add(match);
            await context.SaveChangesAsync();

            Assert.Single(context.Matches);
        }
    }
}