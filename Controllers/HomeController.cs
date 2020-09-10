using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EZCourse.Models;
using EZCourse.Services;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;

namespace EZCourse.Controllers
{
    public class HomeController : Controller
    {
		readonly Smtp _smtpService;
		readonly ContactOptions _contactOptions; 

		public HomeController(Smtp smtpService, IOptions<ContactOptions> contactOptions) 
		{
			_smtpService = smtpService;
			_contactOptions = contactOptions.Value;
		}

		public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

		[HttpPost]
		public IActionResult Contact(Contact formData)
		{
			
			if (!ModelState.IsValid) 
			{
				return View(formData);
			}

			//1
			using (var client = new SmtpClient())
			{
				client.Connect("mail.domain.com");
				client.Authenticate("username", "password");
				var bodyBuilder = new BodyBuilder();
				bodyBuilder.HtmlBody = $"<p>{formData.Name} ({formData.Email})</p><p>{formData.Phone}</p><p>{formData.Message}</p>";
				bodyBuilder.TextBody = "{formData.Name} ({formData.Email})\r\n{formData.Phone}\r\n{formData.Message}";

				var message = new MimeMessage();
				message.Body = bodyBuilder.ToMessageBody();
				message.From.Add(new MailboxAddress("noreply mysite", "noreply@videotrainingsolutions.com"));
				message.To.Add(new MailboxAddress("RM", "contact@domain.com"));
				message.Subject = "Contact Form";
				client.Send(message);

				client.Disconnect(true);
			}

			/*1. Do something
			var htmlBody = $"<p>{formData.Name} ({formData.Email})</p><p>{formData.Phone}</p><p>{formData.Message}</p>";
			var textBody = "{formData.Name} ({formData.Email})\r\n{formData.Phone}\r\n{formData.Message}";

			_smtpService.SendSingle("Contact Form", htmlBody, textBody,
									_contactOptions.ContactToName, _contactOptions.ContactToAddress,
									_contactOptions.ContactFromName, _contactOptions.ContactFromAddress);
			*/

			//2. Set a message
			TempData["Message"] = "Thank you! Your message is sent to us.";
			//3. Redirect the browser
			return RedirectToAction("Contact");
		}

		public IActionResult Error()
        {
            return View();
        }
    }
}
