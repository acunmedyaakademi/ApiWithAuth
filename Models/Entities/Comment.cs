using Microsoft.AspNetCore.Identity;

namespace ApiWithAuth.Models.Entities;

public class Comment
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public int TweetId { get; set; }
    public Tweet Tweet { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}