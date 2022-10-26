using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInternetProgramming.Context;
using WebInternetProgramming.Models.Shop.ViewModel;
using WebInternetProgramming.Service;

namespace WebInternetProgramming.Controllers
{
    public class ShopController : Controller
    {
        #region CONSTRUCTOR
        private readonly ILogger<ShopController> _logger;
        private ApplicationContext db;
        private EmailService emailService = new EmailService();

        public ShopController(ILogger<ShopController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;

        }
        #endregion

        public IActionResult Index()
        {
            var model = new ShopIndexViewModel();
            model.Goods = db.Goods.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Indexbuy(int id)
        {
            return View(id);
        }
        [HttpPost]
        public async Task<IActionResult> Indexbuy(int Id,string Email, string Text)
        {
            if (User.Identity.IsAuthenticated)
                Email = User.Identity.Name;
            var item = db.IndexCheckList.ToList().Where(x => x.Id == Id).First();
            await emailService.SendEmailAsync(emailService.MailLoginEmail, Email, 
                $"{Email} заказал услугу {item.Name} дополнительно {Text}");
            TempData["message"] = $"Вы успешно заказали услугу {item.Name} Ждите, с вами свяжутся";
            
            return RedirectToAction("index","home");
        }
        [HttpGet]
        public IActionResult Buy(int id)
        {
            return View(id);
        }
        [HttpPost]
        public async Task<IActionResult> Buy(int Id, string Email, string Text)
        {
            if (User.Identity.IsAuthenticated)
                Email = User.Identity.Name;
            var item = db.Goods.ToList().Where(x => x.Id == Id).First();
            item.Amount--;
            await db.SaveChangesAsync();
            await emailService.SendEmailAsync(emailService.MailLoginEmail, Email,
                $"{Email} заказал товар {item.Name} дополнительно {Text}");
            TempData["message"] = $"Вы успешно заказали товар {item.Name} Ждите, с вами свяжутся";

            return RedirectToAction("index", "home");
        }
    }
}
