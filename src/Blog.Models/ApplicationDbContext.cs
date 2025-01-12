using Blog.Models.Posts;
using Blog.Models.UserModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;

namespace Blog.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; } // Add UserRole DbSet here

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            builder.Entity<Post>(entity =>
            {
                entity.ToTable("Posts");

                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Author)
                    .WithMany() 
                    .HasForeignKey(p => p.UserId) 
                    .OnDelete(DeleteBehavior.Cascade); 

  
                entity.Property(p => p.Title)
                    .IsRequired() 
                    .HasMaxLength(200); 

                entity.Property(p => p.Body)
                    .IsRequired() 
                    .HasMaxLength(1000); 
            });
            builder.Entity<UserRole>(entity =>
            {
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles) 
                      .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId);
            });
            builder.Entity<Like>(entity =>
            {
                entity.HasOne(l => l.Post) 
                      .WithMany(p => p.Likes) 
                      .HasForeignKey(l => l.PostId) 
                      .OnDelete(DeleteBehavior.NoAction); 
            });
            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(x => x.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
