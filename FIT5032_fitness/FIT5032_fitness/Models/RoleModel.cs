namespace FIT5032_fitness.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RoleModel : DbContext
    {
        public RoleModel()
            : base("name=RoleModel")
        {
        }

        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
