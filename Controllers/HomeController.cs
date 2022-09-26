using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMediaMustBeGood2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialMediaMustBeGood2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<User> _userManager;
        private readonly SocialMediaContext _context;


        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SocialMediaContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public IActionResult SearchForUser(string someString)
        {
            var users = _userManager.Users.Where(u => (u.Login.Contains(someString) || u.Email.Contains(someString) || u.UserName.Contains(someString) || u.UserInfo.Contains(someString))).ToList().Where(u => !_userManager.IsInRoleAsync(u, "admin").Result);
            ViewBag.Search = someString;
            return View(users);
        }

        public async Task<IActionResult> Subscribe(int id, string search)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentSub = await _context.Subscriptions.FirstOrDefaultAsync(s => s.WhoSubscribesId == currentUserId && s.ToWhoSubscribesId == id);
            User whoSub = await _userManager.FindByIdAsync(currentUserId.ToString());
            User toWhoSub = await _userManager.FindByIdAsync(id.ToString());
            if (currentSub!=null)
            {
                whoSub.SubscribesCount -= 1;
                toWhoSub.SubscribersCount -= 1;
                _context.Remove(currentSub);
                ModelState.AddModelError("", "Подписка отменена");
            }
            else
            {
                whoSub.SubscribesCount += 1;
                toWhoSub.SubscribersCount += 1;
                Subscribe subscribe = new Subscribe()
                {
                    WhoSubscribesId = currentUserId,
                    ToWhoSubscribesId = id
                };
                await _context.AddAsync(subscribe);
            }
            _context.Update(whoSub);
            _context.Update(toWhoSub);
            await _context.SaveChangesAsync();
            return RedirectToAction("SearchForUser", "Home", new { someString = search});
        }
    }
}
