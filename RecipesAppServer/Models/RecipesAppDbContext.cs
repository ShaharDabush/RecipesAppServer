﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

public partial class RecipesAppDbContext : DbContext
{
    public RecipesAppDbContext()
    {
    }

    public RecipesAppDbContext(DbContextOptions<RecipesAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allergy> Allergies { get; set; }

    public virtual DbSet<Barkod> Barkods { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientRecipe> IngredientRecipes { get; set; }

    public virtual DbSet<Kind> Kinds { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=RecipesAppDB;User ID=RecipesAppAdminLogin;Password=pass;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Allergy__3214EC0703BAB108");
        });

        modelBuilder.Entity<Barkod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Barkod__3214EC07DF3D87AB");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.Barkods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Barkod__Ingredie__398D8EEE");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC074D30DD27");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__Recipe__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__UserId__4222D4EF");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC079BA4CF4A");

            entity.HasOne(d => d.Kind).WithMany(p => p.Ingredients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__KindI__2C3393D0");

            entity.HasMany(d => d.Storages).WithMany(p => p.Ingredients)
                .UsingEntity<Dictionary<string, object>>(
                    "IngredientStorage",
                    r => r.HasOne<Storage>().WithMany()
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ingredien__Stora__36B12243"),
                    l => l.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ingredien__Ingre__35BCFE0A"),
                    j =>
                    {
                        j.HasKey("IngredientId", "StorageId").HasName("PK__Ingredie__D60CF5BF1FFD1FD1");
                        j.ToTable("IngredientStorage");
                    });
        });

        modelBuilder.Entity<IngredientRecipe>(entity =>
        {
            entity.HasKey(e => new { e.IngredientId, e.RecipeId }).HasName("PK__Ingredie__A1732AD1EFCC5547");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Ingre__31EC6D26");

            entity.HasOne(d => d.Recipe).WithMany(p => p.IngredientRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Recip__32E0915F");
        });

        modelBuilder.Entity<Kind>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kind__3214EC079184EE48");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Levels__3214EC07E7884421");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Levels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Levels__RecipeId__49C3F6B7");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rating__3214EC073665F2FC");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__RecipeId__46E78A0C");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__UserId__45F365D3");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipes__3214EC070D15C3D0");

            entity.HasOne(d => d.MadeByNavigation).WithMany(p => p.Recipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recipes__MadeBy__276EDEB3");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Storage__3214EC07BE398E0B");

            entity.HasOne(d => d.ManagerNavigation).WithMany(p => p.Storages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Storage__Manager__2F10007B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E14F6080");

            entity.HasOne(d => d.Storage).WithMany(p => p.Users).HasConstraintName("FK__Users__StorageId__4AB81AF0");

            entity.HasMany(d => d.Allergies).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AllergyUser",
                    r => r.HasOne<Allergy>().WithMany()
                        .HasForeignKey("AllergyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AllergyUs__Aller__3F466844"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AllergyUs__UserI__3E52440B"),
                    j =>
                    {
                        j.HasKey("UserId", "AllergyId").HasName("PK__AllergyU__2DC127A87B6C4A91");
                        j.ToTable("AllergyUser");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
