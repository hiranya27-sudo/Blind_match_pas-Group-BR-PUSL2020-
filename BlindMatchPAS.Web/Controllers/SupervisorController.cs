using BlindMatchPAS.Core.Entities;
using BlindMatchPAS.Core.Enums;
using BlindMatchPAS.Infrastructure.Data;
using BlindMatchPAS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Web.Controllers
{
    [Authorize(Roles = Roles.Supervisor)]
    public class SupervisorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupervisorController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            var expertises = await _context.SupervisorExpertises
                .Include(se => se.ResearchArea)
                .Where(se => se.SupervisorId == user!.Id)
                .ToListAsync();

            var matches = await _context.Matches
                .Include(m => m.Project)
                    .ThenInclude(p => p!.ResearchArea)
                .Where(m => m.SupervisorId == user!.Id)
                .ToListAsync();

            ViewBag.Expertises = expertises;

            return View(matches);
        }

        [HttpGet]
        public async Task<IActionResult> AddExpertise()
        {
            ViewBag.ResearchAreas = await _context.ResearchAreas.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddExpertise(SupervisorExpertiseViewModel model)
        {
            ViewBag.ResearchAreas = await _context.ResearchAreas.ToListAsync();

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            bool exists = await _context.SupervisorExpertises.AnyAsync(se =>
                se.SupervisorId == user!.Id && se.ResearchAreaId == model.ResearchAreaId);

            if (!exists)
            {
                var expertise = new SupervisorExpertise
                {
                    SupervisorId = user!.Id,
                    ResearchAreaId = model.ResearchAreaId
                };

                _context.SupervisorExpertises.Add(expertise);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> BrowseProjects()
        {
            var user = await _userManager.GetUserAsync(User);

            var areaIds = await _context.SupervisorExpertises
                .Where(se => se.SupervisorId == user!.Id)
                .Select(se => se.ResearchAreaId)
                .ToListAsync();

            var matchedProjectIds = await _context.Matches
                .Select(m => m.ProjectId)
                .ToListAsync();

            var projects = await _context.Projects
                .Include(p => p.ResearchArea)
                .Where(p => areaIds.Contains(p.ResearchAreaId)
                            && p.Status == ProjectStatus.Pending
                            && !matchedProjectIds.Contains(p.Id))
                .ToListAsync();

            return View(projects);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressInterest(int projectId)
        {
            var user = await _userManager.GetUserAsync(User);

            bool exists = await _context.Matches.AnyAsync(m =>
                m.ProjectId == projectId && m.SupervisorId == user!.Id);

            if (!exists)
            {
                var match = new Match
                {
                    ProjectId = projectId,
                    SupervisorId = user!.Id,
                    Status = MatchStatus.Interested,
                    IdentityRevealed = false
                };

                _context.Matches.Add(match);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmMatch(int matchId)
        {
            var user = await _userManager.GetUserAsync(User);

            var match = await _context.Matches
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.Id == matchId && m.SupervisorId == user!.Id);

            if (match == null)
                return NotFound();

            match.Status = MatchStatus.Confirmed;
            match.IdentityRevealed = true;

            if (match.Project != null)
            {
                match.Project.Status = ProjectStatus.Matched;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }
    }
}