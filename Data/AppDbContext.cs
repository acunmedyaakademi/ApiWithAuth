using System.Data;
using ApiWithAuth.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiWithAuth.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Tweet> Tweets { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}