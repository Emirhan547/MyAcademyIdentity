using EmailApp.Context;
using EmailApp.Entities;
using EmailApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmailApp.Controllers
{
    [Authorize]
    public class MessageController(AppDbContext _context,UserManager<AppUser>_userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user=await _userManager.FindByNameAsync(User.Identity.Name);
            var messages= _context.Messages.Include(x=>x.Sender).Where(x=>x.ReceiverId==user.Id).ToList();
            return View(messages);
        }
        public IActionResult MessageDetails(int id)
        {
           var message=  _context.Messages.Include(x=>x.Sender).FirstOrDefault(x=>x.MessageId==id);
            return View(message);
        }
        public IActionResult SendMessage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        {
            
            var sender=await _userManager.FindByNameAsync(User.Identity.Name);
            var receiver=await _userManager.FindByEmailAsync(model.ReceiverEmail);
            var message=new Message
            {
                Body = model.Body,
                Subject =model.Subject,
                ReceiverId = receiver.Id,
                SenderId =sender.Id, 
                SentDate=DateTime.Now
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
