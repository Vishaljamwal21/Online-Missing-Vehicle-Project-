using Microsoft.AspNetCore.Mvc;
using onlinemissingvehical.Data;
using onlinemissingvehical.Models;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace onlinemissingvehical.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var subscriptions = _context.Subscriptions
                .Include(s => s.User)
                .ToList();

            return View(subscriptions);
        }

        public IActionResult Subscribe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingSubscription = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);
            if (existingSubscription != null)
            {
                ViewBag.Message = "You are already subscribed.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var subscription = new Subscription
            {
                UserId = userId,
                User = (ApplicationUser)user,
                SubscriptionPrice = 200
            };

            return View(subscription);
        }

        [HttpPost]
        public IActionResult Subscribe(Subscription subscription)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingSubscription = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);
            if (existingSubscription != null)
            {
                ViewBag.Message = "You are already subscribed.";
                return View();
            }
            subscription.UserId = userId;
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();
            try
            {
              
                string smtpServer = "smtp-mail.outlook.com";
                int smtpPort = 587;
                string smtpUsername = "thakurvishaljamwal@outlook.com";
                string smtpPassword = "Vishal@2106";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add("thakurvishaljamwal@gmail.com");
                mail.Subject = "Subscription Confirmation ";
                mail.Body = "Your Subscription Confirm Successfully.";

                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);

                ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
            }
            return RedirectToAction("Index", "MissingVehical", new { area = "Customer" });
        }
    }
}
