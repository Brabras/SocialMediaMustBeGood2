using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
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
    public class MyPublicationsController : Controller
    {
        private readonly UserManager<User> _manager;
        private readonly SocialMediaContext _context;

        public MyPublicationsController(UserManager<User> manager, SocialMediaContext context)
        {
            _manager = manager;
            _context = context;
        }

        // GET: Publications
        [Authorize]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 3;
            var libContext = _context.Publications.Where(p=>p.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))).Include(b => b.User);
            var count = await libContext.CountAsync();
            var items = await libContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            PageViewModel pwm = new PageViewModel(count, page, pageSize);
            var comments = new List<List<Comment>>();
            foreach (var list in items)
            {
                var list1 = await _context.Comments.Where(c => c.PublicationId == list.Id).Include(p => p.User).ToListAsync() ?? new List<Comment>();
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

        // GET: Publications/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Publications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Header,PictureLink,Text,LikesCount,WhoLikedUserIds,CommentsCount,AuthoeId")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                publication.LikesCount = 0;
                publication.WhoLikedUserIds = new List<int>();
                publication.CommentsCount = 0;
                publication.CommentsUsersId = new List<int>();
                publication.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _manager.FindByIdAsync(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)).ToString()).Result.PublicationsCount += 1;
                _context.Add(publication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publication);
        }

        // GET: Publications/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }
        [Authorize]
        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var publication = await _context.Publications.FindAsync(id);

            if (publication.UserId == Convert.ToInt32(currentUserId))
            {
                _context.Publications.Remove(publication);
                _manager.FindByIdAsync(currentUserId.ToString()).Result.PublicationsCount -= 1;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(int publicationId)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentPublication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == publicationId);
            if (!currentPublication.WhoLikedUserIds.Contains(currentUserId))
            {
                currentPublication.LikesCount += 1;
                currentPublication.WhoLikedUserIds.Add(currentUserId);
                _context.Update(currentPublication);
                await _context.SaveChangesAsync();
            }
            else
            {
                currentPublication.LikesCount -= 1;
                currentPublication.WhoLikedUserIds.Remove(currentUserId);
                _context.Update(currentPublication);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LeaveComment(string inputText, int publicationId)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentPublication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == publicationId);
            Comment comment = new Comment()
            {
                Text = inputText,
                PublicationId = publicationId,
                UserId = currentUserId
            };
            currentPublication.CommentsCount += 1;
            currentPublication.CommentsUsersId.Add(currentUserId);
            _context.Update(currentPublication);
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.Id == id);
        }
    }
}
