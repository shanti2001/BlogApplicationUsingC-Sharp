﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogApplication.Models;
using com.blogApplication.BlogApplication2.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<Post> posts = new List<Post>();
        using(var DbContext = new DbConfigure()){
            posts = DbContext.Posts.Include(p=>p.Author).Include(p=>p.Tags).ToList();
        }
        return View(posts);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Login(){
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(User user){
            
         return RedirectToAction("Index","Home");
    }
    public IActionResult Register(){
        return View();
    }
    [HttpPost]
    public IActionResult AddUser(string name,string email,string password,string confirmPassword){
        if(password!=null && confirmPassword!=null && password.Equals(confirmPassword)){
            using (var dbContext = new DbConfigure())
            {
                var newUser = new User
                {
                    Name = name,
                    Email = email,
                    Password = password
                };
                dbContext.Users.Add(newUser);
                
                dbContext.SaveChanges();
            }
            
            return RedirectToAction("Login");
        }
        else{
            Console.WriteLine("Hii");
            Console.WriteLine(password+" "+confirmPassword+"hii");
            return RedirectToAction("Register");
        }
        
    }
    [HttpPost]
    public IActionResult LogedIn(){
        return RedirectToAction("Index");
    }

    public IActionResult UserPost(){
        List<Post> posts = new List<Post>();
        using(var dbContext = new DbConfigure()){
            posts = dbContext.Posts.Include(p=>p.Tags).ToList();
        }
        return View(posts);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
