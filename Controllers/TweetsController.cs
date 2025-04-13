using ApiWithAuth.Data;
using ApiWithAuth.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithAuth.Controllers;

[ApiController]
[Route("[controller]")]
public class TweetsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;

    public TweetsController(UserManager<IdentityUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Tweet dünyasına hoş geldiniz!");
    }

    [Authorize]
    [HttpGet("all")]
    public IActionResult ListAllTweets()
    {
        var user = _userManager.GetUserId(User);
        return Ok(user);
    }

    [Authorize]
    [HttpPost("add")]
    public IActionResult AddTweet()
    {
        var newTweet = new Tweet()
        {
            Body = "Merhaba tweet dünyası",
            UserId = _userManager.GetUserId(User)
        };
        
        _context.Tweets.Add(newTweet);
        _context.SaveChanges();
        
        return Ok(newTweet);
    }
}