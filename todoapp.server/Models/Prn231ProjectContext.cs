using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace todoapp.server.Models;

public partial class Prn231ProjectContext : DbContext
{
    public Prn231ProjectContext()
    {
    }

    public Prn231ProjectContext(DbContextOptions<Prn231ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<StickyNote> StickyNotes { get; set; }

    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagsTask> TagsTasks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0703D6E2F6");

            entity.ToTable("User");

            entity.Property(e => e.FullName).HasMaxLength(400);
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__List__3214EC0731A512E6");

            entity.ToTable("List");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Lists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__List__UserID__45F365D3");
        });

        modelBuilder.Entity<StickyNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StickyNote_pk");

            entity.ToTable("StickyNote");

            entity.HasOne(d => d.User).WithMany(p => p.StickyNotes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StickyNot__Accou__46E78A0C");
        });

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Id");

            entity.ToTable("SubTask");

            entity.Property(e => e.Status).HasDefaultValue(false);

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SubTask_Task_Id_fk");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC0782B50CF2");
        });

        modelBuilder.Entity<TagsTask>(entity =>
        {
            entity.HasKey(e => new { e.TagsId, e.TaskId }).HasName("PK__TagsTask__D3F7FF67372D1309");

            entity.ToTable("TagsTask");

            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");

            entity.HasOne(d => d.Tags).WithMany(p => p.TagsTasks)
                .HasForeignKey(d => d.TagsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TagsTask__TagsId__440B1D61");

            entity.HasOne(d => d.Task).WithMany(p => p.TagsTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__TagsTask__TaskId__44FF419A");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC07E554C5CF");

            entity.ToTable("Task");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.List).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ListId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Task_List_Id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
