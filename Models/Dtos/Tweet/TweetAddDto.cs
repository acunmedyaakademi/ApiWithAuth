using System.ComponentModel.DataAnnotations;

namespace ApiWithAuth.Models.Dtos.Tweet;

public class TweetAddDto
{
    [Required]
    public string Body { get; set; }
}