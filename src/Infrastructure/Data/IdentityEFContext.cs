using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace  MiniBlog.Infrastructure.Data;

public class IdentityEfContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public IdentityEfContext(DbContextOptions opts) : base(opts) {}
}