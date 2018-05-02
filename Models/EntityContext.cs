using Microsoft.EntityFrameworkCore;

namespace entity_app.Models
{
    public class EntityContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public EntityContext(DbContextOptions<EntityContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Center> centers { get; set; }
        public DbSet<Participant> participants { get; set; }
    }
}