﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN211_project_publish.Models;

public partial class Prn211ProjectContext : DbContext
{
    public Prn211ProjectContext()
    {
    }

    public Prn211ProjectContext(DbContextOptions<Prn211ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<StickyNote> StickyNotes { get; set; }

    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =localhost; database = PRN211_project;uid=sa;pwd=123;TrustServerCertificate=True;");


    public static string GetConnectionString()
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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07C6CEB735");

            entity.ToTable("Account");

            entity.Property(e => e.FullName).HasMaxLength(400);
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__List__3214EC0745303ABA");

            entity.ToTable("List");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");

            entity.HasOne(d => d.Account).WithMany(p => p.Lists)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__List__AccountID__398D8EEE");
        });

        modelBuilder.Entity<StickyNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StickyNote_pk");

            entity.ToTable("StickyNote");

            entity.HasOne(d => d.Account).WithMany(p => p.StickyNotes)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StickyNot__Accou__5CD6CB2B");
        });

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Id");

            entity.ToTable("SubTask");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SubTask_Task_Id_fk");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC0779F8F38B");

            entity.HasMany(d => d.Tasks).WithMany(p => p.Tags)
                .UsingEntity<Dictionary<string, object>>(
                    "TagsTask",
                    r => r.HasOne<Task>().WithMany()
                        .HasForeignKey("TaskId")
                        .HasConstraintName("FK__TagsTask__TaskId__44FF419A"),
                    l => l.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__TagsTask__TagsId__440B1D61"),
                    j =>
                    {
                        j.HasKey("TagsId", "TaskId").HasName("PK__TagsTask__D3F7FF6713AD03C8");
                        j.ToTable("TagsTask");
                    });
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC07B4B6BAF0");

            entity.ToTable("Task");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("date");

            entity.HasOne(d => d.List).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ListId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Task_List_Id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
