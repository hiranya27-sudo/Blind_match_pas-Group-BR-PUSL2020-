using BlindMatchPAS.Core.Entities;
using BlindMatchPAS.Core.Enums;
using BlindMatchPAS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Web.Controllers
{
    [Authorize(Roles = Roles.ModuleLeader + "," + Roles.Admin)]
    public class ModuleLeaderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ModuleLeaderController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var projects = await _context.Projects
                .Include(p => p.Student)
                .Include(p => p.ResearchArea)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var matches = await _context.Matches
                .Include(m => m.Project)
                    .ThenInclude(p => p!.ResearchArea)
                .Include(m => m.Project)
                    .ThenInclude(p => p!.Student)
                .Include(m => m.Supervisor)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            ViewBag.TotalProjects = projects.Count;
            ViewBag.TotalMatches = matches.Count;
            ViewBag.PendingProjects = projects.Count(p => p.Status == ProjectStatus.Pending);
            ViewBag.ConfirmedMatches = matches.Count(m => m.Status == MatchStatus.Confirmed);
            ViewBag.Matches = matches;

            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> ResearchAreas()
        {
            var areas = await _context.ResearchAreas.OrderBy(a => a.Name).ToListAsync();
            return View(areas);
        }

        [HttpGet]
        public IActionResult CreateResearchArea()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateResearchArea(ResearchArea model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool exists = await _context.ResearchAreas.AnyAsync(a => a.Name == model.Name);

            if (!exists)
            {
                _context.ResearchAreas.Add(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ResearchAreas));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteResearchArea(int id)
        {
            var area = await _context.ResearchAreas.FindAsync(id);

            if (area == null)
                return NotFound();

            bool isUsed = await _context.Projects.AnyAsync(p => p.ResearchAreaId == id)
                       || await _context.SupervisorExpertises.AnyAsync(se => se.ResearchAreaId == id);

            if (isUsed)
            {
                TempData["Error"] = "Cannot delete a research area that is already in use.";
                return RedirectToAction(nameof(ResearchAreas));
            }

            _context.ResearchAreas.Remove(area);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ResearchAreas));
        }

        [HttpGet]
        public async Task<IActionResult> Reassign(int matchId)
        {
            var match = await _context.Matches
                .Include(m => m.Project)
                    .ThenInclude(p => p!.ResearchArea)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null || match.Project == null)
                return NotFound();

            var supervisors = await _context.SupervisorExpertises
                .Include(se => se.Supervisor)
                .Where(se => se.ResearchAreaId == match.Project.ResearchAreaId)
                .Select(se => se.Supervisor!)
                .Distinct()
                .ToListAsync();

            ViewBag.Match = match;
            ViewBag.Supervisors = supervisors;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reassign(int matchId, string newSupervisorId)
        {
            var match = await _context.Matches
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
                return NotFound();

            match.SupervisorId = newSupervisorId;
            match.Status = MatchStatus.Reassigned;
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