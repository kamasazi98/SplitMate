using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SplitMate.Domain.Entities;

namespace SplitMate.Infrastracture.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
	{
		public DbSet<User> KnownUsers { get; set; }
		public DbSet<ShoppingList> ShoppingLists { get; set; }
		public DbSet<ShoppingItem> ShoppingItems { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
