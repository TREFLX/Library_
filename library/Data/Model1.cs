using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace library.Data
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<books> books { get; set; }
        public virtual DbSet<employees> employees { get; set; }
        public virtual DbSet<readers> readers { get; set; }
        public virtual DbSet<users> users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<books>()
                .HasMany(e => e.readers)
                .WithMany(e => e.books)
                .Map(m => m.ToTable("listbooks").MapLeftKey("id_book").MapRightKey("id_reader"));

            modelBuilder.Entity<users>()
                .HasMany(e => e.employees)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.readers)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);
        }
    }
}
