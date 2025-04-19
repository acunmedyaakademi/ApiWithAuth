using ApiWithAuth.Data;
using ApiWithAuth.Migrations;
using ApiWithAuth.Models.Dtos.Tweet;
using ApiWithAuth.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWithAuth.Controllers;

[ApiController]
[Route("[controller]")]
public class TweetsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TweetsController(UserManager<IdentityUser> userManager, AppDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public ActionResult<TweetDto[]> Index()
    {
        return _mapper.Map<TweetDto[]>(_context.Tweets
            .Include(t => t.User)
            .ToArray());
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<TweetDto[]>> GetTweetsByUserId(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        var userTweets = _context.Tweets.Where(t => t.UserId == userId).Include(t => t.User).ToArray();
        return _mapper.Map<TweetDto[]>(userTweets);
    }

    [HttpGet("{userId}/{tweetId}")]
    public ActionResult<TweetDto> GetTweetById(string userId, int tweetId)
    {
        var tweet = _context.Tweets.Where(t => t.Id == tweetId && t.UserId == userId).Include(t => t.User)
            .FirstOrDefault();
        if (tweet == null)
        {
            return NotFound();
        }
        return  _mapper.Map<TweetDto>(tweet);
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult> AddTweet(TweetAddDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // aslında kullanıcıyı almamıza gerek yok
        // çünkü sadece kullanıcı id'si bizim için yeterli
        // buna bağlı olarak mevcut login olan kullanıcıyı claims üzerinden alabiliriz
        // var user = await _userManager.GetUserAsync(User);
        var userId = _userManager.GetUserId(User);
        
        var newTweet = _mapper.Map<Tweet>(model);
        newTweet.UserId = userId;
        
        _context.Tweets.Add(newTweet);
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTweet(int id)
    {
        var tweet = await _context.Tweets.FindAsync(id);
        if (tweet == null)
        {
            return NotFound();
        }
        
        var userId = _userManager.GetUserId(User);
        if (tweet.UserId != userId)
        {
            return Unauthorized();
        }
        
        _context.Tweets.Remove(tweet);
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}