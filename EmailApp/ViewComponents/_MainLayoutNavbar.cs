using EmailApp.Context;
using EmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.ViewComponents
{
    public class _MainLayoutNavbar : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _MainLayoutNavbar(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int recentCount = 3)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(new List<Message>());

            var messages = await _context.Messages
                .Where(m => m.ReceiverId == user.Id && !m.IsDeleted && !m.IsDraft)
                .OrderByDescending(m => m.SentDate)
                .Take(recentCount)
                .Include(m => m.Sender)
                .ToListAsync();

            return View(messages);
        }
    }
}