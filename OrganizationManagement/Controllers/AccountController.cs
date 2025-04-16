﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Threading.Tasks;

namespace OrganizationManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _tables;

        public AccountController(ApplicationDbContext tables)
        {
            _tables = tables;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new AdminDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login( AdminDto model)
        
        {
             Console.WriteLine("LOGIN ACTION REACHED");
        //   var abc=  _tables.Admins.ToList();

        //   foreach(var a in abc){
        //     if(a.Role=="user"){
        //     Console.WriteLine(a.Name);
        //     Console.WriteLine(a.Email);
        //     Console.WriteLine(a.Password);
        //     }
        //   }
             
        //       Console.WriteLine("1");

            var user = await _tables.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (user == null || user.Role != "user")
            {
                // Console.WriteLine("2");
                ModelState.AddModelError("", "Invalid email or unauthorized role.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<Admin>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid password.");
                return View(model);
            }
              Console.WriteLine("Redirecting to PostLoginOptions");
           return RedirectToAction("PostLoginOptions", "Account", new { userId = user.Id });

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new AdminDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdminDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _tables.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Email already registered.");
                return View(model);
            }

            if (model.Role != "user")
            {
                ModelState.AddModelError("", "Only role 'user' is allowed.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<Admin>();
            var admin = new Admin
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                Password = passwordHasher.HashPassword(null, model.Password)
            };

            await _tables.Admins.AddAsync(admin);
            await _tables.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> PostLoginOptions(int userId)
        {
            // Console.WriteLine("1");
            var user = await _tables.Admins.FindAsync(userId);
            if (user == null || user.Role != "user")
            {
            //   Console.WriteLine("2");
                return RedirectToAction("Login");
            }
        //   Console.WriteLine("3");
            ViewBag.UserId = userId;
            return View();
        }
        
    }
}
