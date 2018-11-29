namespace FIT5032_fitness.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GroupModel : DbContext
    {
        public GroupModel()
            : base("name=GroupModel")
        {
        }

        public virtual DbSet<GroupTraining> GroupTrainings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupTraining>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}
