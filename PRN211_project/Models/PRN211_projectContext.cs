﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN211_test.Models
{
    public partial class PRN211_projectContext : DbContext
    {
        // singeton
        private static PRN211_projectContext ins;
        private static readonly object instanceLock = new object();
        public static PRN211_projectContext Ins 
        {
            get
            {
                lock (instanceLock)
                {
                    if (ins == null)
                    {
                        ins = new PRN211_projectContext();
                    }

                }
                return ins;
            }
        }


        private PRN211_projectContext()
        {
        }

        public PRN211_projectContext(DbContextOptions<PRN211_projectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<List> Lists { get; set; } = null!;
        public virtual DbSet<SubTask> SubTasks { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("server =localhost; database = PRN211_project;uid=sa;pwd=123;");
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

            modelBuilder.Entity<SubTask>(entity =>
            {
                entity.ToTable("SubTask");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.SubTasks)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__SubTask__TaskId__412EB0B6");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasMany(d => d.Tasks)
                    .WithMany(p => p.Tags)
                    .UsingEntity<Dictionary<string, object>>(
                        "TagsTask",
                        l => l.HasOne<Task>().WithMany().HasForeignKey("TaskId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__TagsTask__TaskId__44FF419A"),
                        r => r.HasOne<Tag>().WithMany().HasForeignKey("TagsId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__TagsTask__TagsId__440B1D61"),
                        j =>
                        {
                            j.HasKey("TagsId", "TaskId").HasName("PK__TagsTask__D3F7FF6713AD03C8");

                            j.ToTable("TagsTask");
                        });
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.List)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ListId)
                    .HasConstraintName("FK__Task__ListId__3E52440B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}