using EmailApp.Context;
using EmailApp.Entities;
using EmailApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.ViewComponents
{
    public class _MainLayoutSidebar : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _MainLayoutSidebar(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SidebarViewModel();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(model); // boş model döndür

            // Sayılar
            model.InboxCount = await _context.Messages.CountAsync(m => m.ReceiverId == user.Id && !m.IsDeleted && !m.IsDraft);
            model.SentCount = await _context.Messages.CountAsync(m => m.SenderId == user.Id && !m.IsDeleted && !m.IsDraft);
            model.DraftCount = await _context.Messages.CountAsync(m => m.SenderId == user.Id && m.IsDraft);
            model.TrashCount = await _context.Messages.CountAsync(m => (m.SenderId == user.Id || m.ReceiverId == user.Id) && m.IsDeleted);
            var categories = new List<string> { "Önemli", "İş", "Aile", "Kişisel", "Diğer" };
            var colors = new Dictionary<string, (string Badge, string Text)>
{
    { "Önemli", ("bg-danger", "text-danger") },
    { "İş", ("bg-info", "text-info") },
    { "Aile", ("bg-success", "text-success") },
    { "Kişisel", ("bg-warning", "text-warning") },
    { "Diğer", ("bg-secondary", "text-secondary") }
};

            // Ensure dictionaries are initialized
            model.CategoryCounts = new Dictionary<string, int>();
            model.CategoryColors = new Dictionary<string, string>();
            model.CategoryTextColors = new Dictionary<string, string>();

            foreach (var cat in categories)
            {
                var cnt = await _context.Messages.CountAsync(m =>
                    m.ReceiverId == user.Id &&
                    m.Category == cat &&
                    !m.IsDeleted &&
                    !m.IsDraft);

                model.CategoryCounts[cat] = cnt;

                if (colors.ContainsKey(cat))
                {
                    model.CategoryColors[cat] = colors[cat].Badge;
                    model.CategoryTextColors[cat] = colors[cat].Text;
                }
                else
                {
                    model.CategoryColors[cat] = "bg-primary";
                    model.CategoryTextColors[cat] = "text-primary";
                }
            }


            return View(model);
        }
    }
}
