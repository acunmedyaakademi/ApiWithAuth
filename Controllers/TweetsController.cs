using ApiWithAuth.Data;
using ApiWithAuth.Migrations;
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

    [HttpGet("all")]
    public ActionResult<Tweet[]> ListAllTweets()
    {
        return _context.Tweets.ToArray();
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult<Tweet>> AddTweet()
    {
        // aslında kullanıcıyı almamıza gerek yok
        // çünkü sadece kullanıcı id'si bizim için yeterli
        // buna bağlı olarak mevcut login olan kullanıcıyı claims üzerinden alabiliriz
        var user = await _userManager.GetUserAsync(User);
        
        var newTweet = new Tweet()
        {
            Body = "Merhaba tweet dünyası",
            User = user
        };
        
        _context.Tweets.Add(newTweet);
        await _context.SaveChangesAsync();
        
        return newTweet;
    }
}