using System.ComponentModel.DataAnnotations;

namespace ApiWithAuth.Models.Dtos.Base;

public class AddDto
{
    [Required]
    public string Body { get; set; }
}