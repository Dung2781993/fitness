namespace FIT5032_fitness.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EditorModels : DbContext
    {
        public EditorModels()
            : base("name=EditorModels")
        {
        }

        public virtual DbSet<Editor> Editors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Editor>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Editor>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Editor>()
                .Property(e => e.authorName)
                .IsUnicode(false);

            modelBuilder.Entity<Editor>()
                .Property(e => e.path)
                .IsUnicode(false);
        }
    }
}
