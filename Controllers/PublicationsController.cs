using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialMediaMustBeGood2.Models;
using SocialMediaMustBeGood2.ViewModels;

namespace SocialMediaMustBeGood2.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly UserManager<User> _manager;
        private readonly SocialMediaContext _context;

        public PublicationsController(UserManager<User> manager, SocialMediaContext context)
        {
            _manager = manager;
            _context = context;
        }

        // GET: Publications
        public async Task<IActionResult> Index(int page = 1)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var subscribes = await _context.Subscriptions.Where(x => x.WhoSubscribesId == currentUserId).ToListAsync();
            List<int> mySubscribesIds = subscribes.Select(s=> s.ToWhoSubscribesId).ToList();

            int pageSize = 3;
            var libContext = _context.Publications
                .Where(p => mySubscribesIds.Contains(p.UserId))
                .Include(b => b.User);
            var count = await libContext.CountAsync();
            var items = await libContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            PageViewModel pwm = new PageViewModel(count, page, pageSize);
            var comments = new List<List<Comment>>();
            foreach (var list in items)
            {
                var list1 = await _context.Comments.Where(c => c.PublicationId == list.Id).Include(p=>p.User).ToListAsync() ?? new List<Comment>();
                comments.Add(list1);
            }
            var booksListModel = new PublicationsListViewModel
            {
                Publications = items,
                PageViewModel = pwm,
                Comments = comments
            };
            return View(booksListModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(int publicationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentPublication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == publicationId);
            if (!currentPublication.WhoLikedUserIds.Contains(Convert.ToInt32(userId)))
            {
                currentPublication.LikesCount += 1;
                currentPublication.WhoLikedUserIds.Add(Convert.ToInt32(userId));
                _context.Update(currentPublication);
                await _context.SaveChangesAsync();
            }
            else
            {
                currentPublication.LikesCount -= 1;
                currentPublication.WhoLikedUserIds.Remove(Convert.ToInt32(userId));
                _context.Update(currentPublication);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LeaveComment(string inputText, int publicationId)
        {
            int currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentPublication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == publicationId);
            Comment comment = new Comment()
            {
                Text = inputText,
                PublicationId = publicationId,
                UserId = currentUserId
            };
            currentPublication.CommentsCount += 1;
            currentPublication.CommentsUsersId.Add(currentUserId);
            _context.Add(comment);
            _context.Update(currentPublication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.Id == id);
        }
    }
}
