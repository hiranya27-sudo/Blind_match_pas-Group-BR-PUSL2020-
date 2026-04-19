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
    [Authorize(Roles = Roles.Student)]
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            var projects = await _context.Projects
                .Include(p => p.ResearchArea)
                .Where(p => p.StudentId == user!.Id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.ResearchAreas = await _context.ResearchAreas.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            ViewBag.ResearchAreas = await _context.ResearchAreas.ToListAsync();

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            var project = new Project
            {
                Title = model.Title,
                Abstract = model.Abstract,
                ResearchAreaId = model.ResearchAreaId,
                StudentId = user!.Id,
                Status = ProjectStatus.Pending
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.StudentId == user!.Id);

            if (project == null || project.Status != ProjectStatus.Pending)
                return NotFound();

            ViewBag.ResearchAreas = await _context.ResearchAreas.ToListAsync();

            var model = new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Abstract = project.Abstract,
                ResearchAreaId = project.ResearchAreaId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectViewModel model)
        {
            ViewBag.ResearchAreas = await _context.ResearchAreas.ToListAsync();

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == model.Id && p.StudentId == user!.Id);

            if (project == null || project.Status != ProjectStatus.Pending)
                return NotFound();

            project.Title = model.Title;
            project.Abstract = model.Abstract;
            project.ResearchAreaId = model.ResearchAreaId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.StudentId == user!.Id);

            if (project == null || project.Status != ProjectStatus.Pending)
                return NotFound();

            project.Status = ProjectStatus.Withdrawn;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }
    }
}