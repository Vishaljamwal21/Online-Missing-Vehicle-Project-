using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using onlinemissingvehical.Data;
using onlinemissingvehical.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace onlinemissingvehical.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_User)]
    public class MissingVehicalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MissingVehicalController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var missingVehicles = await _context.MissingVehicles
                .Where(m => m.UserId == userId)
                .ToListAsync();

            return View(missingVehicles);
        }


        public IActionResult SaveOrEdit(int? id)
        {
            MissingVehicle missingVehicle = new MissingVehicle();
            if (id == null)
                return View(missingVehicle);
            else
            {
                missingVehicle = _context.MissingVehicles.FirstOrDefault(m => m.Id == id);
                if (missingVehicle == null)
                    return NotFound();
                else
                    return View(missingVehicle);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveorEdit(MissingVehicle missingVehicle)
        {
           
            if (_context.MissingVehicles.Any(m => m.LicensePlateNumber == missingVehicle.LicensePlateNumber && m.Id != missingVehicle.Id))
            {
                ModelState.AddModelError("LicensePlateNumber", "License plate number already exists.");
                return View(missingVehicle);
            }
            missingVehicle.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);           

            var webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                var fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(files[0].FileName);
                var uploads = Path.Combine(webRootPath, @"images\missingvehical");
                if (!string.IsNullOrEmpty(missingVehicle.ImageUrl))
                {
                    var oldImagePath = Path.Combine(webRootPath, missingVehicle.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                missingVehicle.ImageUrl = @"\images\missingvehical\" + fileName + extension;
            }

            if (missingVehicle.Id == 0)
                _context.MissingVehicles.Add(missingVehicle);
            else
                _context.MissingVehicles.Update(missingVehicle);
            _context.SaveChanges();

            try
            {
                // SMTP server configuration
                string smtpServer = "smtp-mail.outlook.com";
                int smtpPort = 587;
                string smtpUsername = "thakurvishaljamwal@outlook.com";
                string smtpPassword = "Vishal@2106";
                var licensePlateNumber = missingVehicle.LicensePlateNumber;
                // Create the email message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add("thakurvishaljamwal@gmail.com");
                mail.Subject = "Complaint Submission Confirmation";
                mail.Body = $"Your complaint for vehicle with license plate number {licensePlateNumber} has been submitted successfully. For more updates about your complaint, get a subscription for only $200. Thank you.";

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

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var missingVehicleInDb = _context.MissingVehicles.FirstOrDefault(m => m.Id == id);  
            if (missingVehicleInDb == null)
                return NotFound();
            if (!string.IsNullOrEmpty(missingVehicleInDb.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, missingVehicleInDb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.MissingVehicles.Remove(missingVehicleInDb);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


    }
}
