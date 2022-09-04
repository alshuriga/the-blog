using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace  MiniBlog.Infrastructure.Data;

public class IdentityEFContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public IdentityEFContext(DbContextOptions opts) : base(opts) {}
}