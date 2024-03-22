using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN211_project.Models
{
    public partial class PRN211_projectContext : DbContext
    {
        public PRN211_projectContext()
        {
        }

        public PRN211_projectContext(DbContextOptions<PRN211_projectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<List> Lists { get; set; } = null!;
        public virtual DbSet<StickyNote> StickyNotes { get; set; } = null!;
        public virtual DbSet<SubTask> SubTasks { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<TagsTask> TagsTasks { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionStrings:Default"];
            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.FullName).HasMaxLength(400);
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.ToTable("List");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Lists)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__List__AccountID__398D8EEE");
            });

            modelBuilder.Entity<StickyNote>(entity =>
            {
                entity.ToTable("StickyNote");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.StickyNotes)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StickyNot__Accou__5CD6CB2B");
            });

            modelBuilder.Entity<SubTask>(entity =>
            {
                entity.ToTable("SubTask");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.SubTasks)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SubTask_Task_Id_fk");
            });

            modelBuilder.Entity<TagsTask>(entity =>
            {
                entity.HasKey(e => new { e.TagsId, e.TaskId })
                    .HasName("PK__TagsTask__D3F7FF6713AD03C8");

                entity.ToTable("TagsTask");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Tags)
                    .WithMany(p => p.TagsTasks)
                    .HasForeignKey(d => d.TagsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TagsTask__TagsId__440B1D61");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TagsTasks)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__TagsTask__TaskId__44FF419A");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Task_List_Id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
