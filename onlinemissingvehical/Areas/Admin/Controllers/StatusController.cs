using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlinemissingvehical.Data;
using onlinemissingvehical.Models;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace onlinemissingvehical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var missingVehicles = await _context.MissingVehicles
                .Include(m => m.User)
                .Where(m => !_context.StatusUpdates
                                .Where(s => s.Status == "Recovered")
                                .Select(s => s.MissingVehicleId)
                                .Contains(m.Id))
                .ToListAsync();
            return View(missingVehicles);
        }


        public IActionResult Create(int id)
        {
            var missingVehicle = _context.MissingVehicles.Find(id);
            if (missingVehicle == null)
            {
                return NotFound();
            }

            var statusUpdate = new StatusUpdate
            {
                MissingVehicleId = missingVehicle.Id,
                MissingVehicle = missingVehicle
            };

            return View(statusUpdate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StatusUpdate statusUpdate)
        {
            if (ModelState.IsValid)
            {
                var existingStatusUpdate = _context.StatusUpdates.FirstOrDefault(s => s.MissingVehicleId == statusUpdate.MissingVehicleId);

                if (existingStatusUpdate != null)
                {
                    existingStatusUpdate.Status = statusUpdate.Status;
                    _context.StatusUpdates.Update(existingStatusUpdate);
                }
                else
                {
                    statusUpdate.Id = 0;
                    _context.StatusUpdates.Add(statusUpdate);
                }

                _context.SaveChanges();
                var userId = _context.MissingVehicles
                                    .Where(m => m.Id == statusUpdate.MissingVehicleId)
                                    .Select(m => m.UserId)
                                    .FirstOrDefault();
                var existingSubscription = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);

                if (existingSubscription != null)
                {
                    try
                    {
                        // SMTP server configuration
                        string smtpServer = "smtp-mail.outlook.com";
                        int smtpPort = 587;
                        string smtpUsername = "thakurvishaljamwal@outlook.com";
                        string smtpPassword = "Vishal@2106";

                        // Fetch license plate number of the missing vehicle
                        var licensePlateNumber = _context.MissingVehicles
                                                    .Where(m => m.Id == statusUpdate.MissingVehicleId)
                                                    .Select(m => m.LicensePlateNumber)
                                                    .FirstOrDefault();

                        // Create the email message
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(smtpUsername);
                        mail.To.Add("thakurvishaljamwal@gmail.com");
                        mail.Subject = "Complaint Status";

                        // Set email body based on the selected status and include license plate number
                        string status = statusUpdate.Status;
                        switch (status)
                        {
                            case "Pending":
                                mail.Body = $"Your complaint for vehicle with license plate number {licensePlateNumber} is pending.We can find it as soon as possible.";
                                break;
                            case "Processing":
                                mail.Body = $"Your complaint for vehicle with license plate number {licensePlateNumber} is under processing.We can find it as soon as possible.";
                                break;
                            case "Recovered":
                                mail.Body = $"Your Vehicle with license plate number {licensePlateNumber} has been recovered. Your vehicle will be delivered to your address shortly.";
                                break;
                            default:
                                mail.Body = "Your complaint status is unknown.";
                                break;
                        }

                        // SMTP client configuration and sending email
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
                }

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            return View(statusUpdate);
        }


    }
}
