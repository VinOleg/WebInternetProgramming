using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebInternetProgramming.Context;
using WebInternetProgramming.Models;
using WebInternetProgramming.Models.ViewModel;
using WebInternetProgramming.Service;

namespace WebInternetProgramming.Controllers
{
    public class HomeController : Controller
    {
        #region CONSTRUCTOR
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private EmailService emailService = new EmailService();

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, 
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        public IActionResult Index()
        {
            var model = new IndexViewModel();
            model.indexCheckList = db.IndexCheckList.ToList();
            return View(model);
        }
        #region USER
        // вью регистрации
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        //сабмит регистрации
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, FIO = model.FIO };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await emailService.SendEmailAsync(user.Email,"Приветсвие", "Добро пожаловать в наш сервис");
                    await _signInManager.SignInAsync(user, false);
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        //логин пользователя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(PersonalAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.LoginModel.Email,
                    model.LoginModel.Password, model.LoginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    return View("PersonalAccount", model);
                }
            }
            return View(model);
        }
        //выход из регистрации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        //личный кабинет(вьюшка)
        [HttpGet]
        public IActionResult PersonalAccount()
        {
            var paViewModel = new PersonalAccountViewModel();
            return View(paViewModel);
        }

        #endregion
        //вьюшка "контакт"
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(string Email, string Text)
        {
            if (User.Identity.IsAuthenticated)
                Email = User.Identity.Name;
            await emailService.SendEmailAsync(emailService.MailLoginEmail, Email, $"{Email} написал {Text}");
           // await emailService.SendEmailAsync(Email, emailService.MailLoginEmail, $"Ваш вопрос получен, ожидайте ответа");
            return View("Index",new IndexViewModel {indexCheckList= db.IndexCheckList.ToList()});
        }
        //обработка ошибок сток
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult Office()
        {
            return View();
        }
    }
}
