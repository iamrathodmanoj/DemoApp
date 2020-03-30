using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options) : base (options){}

// it will create the Table with name Values 
        public DbSet<Value> Values {get;set;}

        public DbSet<User> USers  {get; set;}

        public DbSet<Photo> Photos { get; set; }
    }
}