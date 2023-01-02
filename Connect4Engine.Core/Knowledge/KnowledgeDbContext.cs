using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class KnowledgeDbContext : DbContext
    {
        public DbSet<KnowledgeEntry> KnowledgeEntries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeEntry>()
                .HasKey(builder => new { builder.PartialPositionKey1, builder.PartialPositionKey2 });
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseSqlite("Data Source=blog.db");
    }
}

