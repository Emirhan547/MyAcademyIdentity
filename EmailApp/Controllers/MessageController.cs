using EmailApp.Context;
using EmailApp.Entities;
using EmailApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApp.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Gelen kutusu - Sadece okunmamış ve silinmemiş mesajlar
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && !x.IsDeleted && !x.IsDraft)
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();

            return View(messages);
        }

        // Okunmuş mesajlar
        public async Task<IActionResult> ReadMessages()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && x.IsRead && !x.IsDeleted && !x.IsDraft)
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();

            return View("Index", messages);
        }

        // Kategoriye göre mesajlar - TEK BIR TANE OLMALI!
        public async Task<IActionResult> Category(string name)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && x.Category == name && !x.IsDeleted && !x.IsDraft)
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();

            ViewBag.CurrentCategory = name;
            return View("Index", messages);
        }

        // Çöp kutusu
        public async Task<IActionResult> Trash()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => (x.ReceiverId == user.Id || x.SenderId == user.Id) && x.IsDeleted)
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();

            return View(messages);
        }

        // Taslaklar
        public async Task<IActionResult> Drafts()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = await _context.Messages
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == user.Id && x.IsDraft)
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();

            return View(messages);
        }

        // Mesaj detayı - Okundu olarak işaretle
        public async Task<IActionResult> MessageDetails(int id)
        {
            var message = await _context.Messages
                .Include(x => x.Sender)
                .FirstOrDefaultAsync(x => x.MessageId == id);

            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }

            return View(message);
        }

        // Mesaj gönderimi
        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sender = await _userManager.FindByNameAsync(User.Identity.Name);
                var receiver = await _userManager.FindByEmailAsync(model.ReceiverEmail);

                if (receiver == null)
                {
                    ModelState.AddModelError("ReceiverEmail", "Kullanıcı bulunamadı");
                    return View(model);
                }

                var message = new Message
                {
                    Body = model.Body,
                    Subject = model.Subject,
                    ReceiverId = receiver.Id,
                    SenderId = sender.Id,
                    SentDate = DateTime.Now,
                    Category = model.Category,
                    IsDraft = model.SaveAsDraft,
                    IsRead = false,
                    IsDeleted = false
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                if (model.SaveAsDraft)
                    return RedirectToAction("Drafts");
                else
                    return RedirectToAction("Index");
            }

            return View(model);
        }

        // Mesajı sil (çöp kutusuna taşı)
        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                message.IsDeleted = true;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // Mesajı kalıcı olarak sil
        [HttpPost]
        public async Task<IActionResult> PermanentDelete(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Trash");
        }

        // Mesajı geri yükle (çöp kutusundan çıkar)
        [HttpPost]
        public async Task<IActionResult> RestoreMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                message.IsDeleted = false;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Trash");
        }

        // Taslağı gönderime hazır hale getir
        public async Task<IActionResult> EditDraft(int id)
        {
            var message = await _context.Messages
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(x => x.MessageId == id && x.IsDraft);

            if (message == null)
                return NotFound();

            var model = new SendMessageViewModel
            {
                ReceiverEmail = message.Receiver?.Email,
                Subject = message.Subject,
                Body = message.Body,
                Category = message.Category
            };

            return View("SendMessage", model);
        }

        // Gönderilen mesajlar
        public async Task<IActionResult> SentMessages()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = await _context.Messages
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == user.Id && !x.IsDraft && !x.IsDeleted)
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();

            return View(messages);
        }

        
    }
}