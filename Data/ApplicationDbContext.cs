using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PowerBI.Models;

namespace PowerBI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para la entidad User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user"); // Nombre de la tabla en la base de datos.
                entity.HasKey(e => e.idUser);

                entity.Property(e => e.idUser).HasColumnName("idUser");
                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.lastname).HasColumnName("lastname");
                entity.Property(e => e.nickname).HasColumnName("nickname");
                entity.Property(e => e.password).HasColumnName("password");
            });

            // Configuración para la entidad Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories"); // Nombre de la tabla en la base de datos.
                entity.HasKey(e => e.idCategory);

                entity.Property(e => e.idCategory).HasColumnName("idCategory");
                entity.Property(e => e.idUser).HasColumnName("idUser");
                entity.Property(e => e.Name).HasColumnName("name");

                // Configuración de la relación con User
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Categories)             
                      .HasForeignKey(c => c.idUser)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}