using System;
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

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientRecipe> IngredientRecipes { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Allergy__3214EC079264351C");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC078458295C");

            entity.HasMany(d => d.Storages).WithMany(p => p.Ingredients)
                .UsingEntity<Dictionary<string, object>>(
                    "IngredientStorage",
                    r => r.HasOne<Storage>().WithMany()
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ingredien__Stora__33D4B598"),
                    l => l.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ingredien__Ingre__32E0915F"),
                    j =>
                    {
                        j.HasKey("IngredientId", "StorageId").HasName("PK__Ingredie__D60CF5BF400BC02D");
                        j.ToTable("IngredientStorage");
                    });
        });

        modelBuilder.Entity<IngredientRecipe>(entity =>
        {
            entity.HasKey(e => new { e.IngredientId, e.RecipeId }).HasName("PK__Ingredie__A1732AD1353C07DE");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Ingre__2F10007B");

            entity.HasOne(d => d.Recipe).WithMany(p => p.IngredientRecipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Recip__300424B4");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Levels__3214EC07732C8C09");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Levels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Levels__RecipeId__403A8C7D");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rating__3214EC07BD8501F2");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__RecipeId__3D5E1FD2");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__UserId__3C69FB99");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipes__3214EC0788F9046A");

            entity.HasOne(d => d.MadeByNavigation).WithMany(p => p.Recipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recipes__MadeBy__276EDEB3");

            entity.HasMany(d => d.Allergies).WithMany(p => p.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeAllergy",
                    r => r.HasOne<Allergy>().WithMany()
                        .HasForeignKey("AllergyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RecipeAll__Aller__440B1D61"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RecipeAll__Recip__4316F928"),
                    j =>
                    {
                        j.HasKey("RecipeId", "AllergyId").HasName("PK__RecipeAl__C790635475DDA362");
                        j.ToTable("RecipeAllergies");
                    });
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Storage__3214EC0702DFE746");

            entity.HasOne(d => d.ManagerNavigation).WithMany(p => p.Storages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Storage__Manager__2C3393D0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0793B68EC6");

            entity.HasOne(d => d.Storage).WithMany(p => p.Users).HasConstraintName("FK__Users__StorageId__44FF419A");

            entity.HasMany(d => d.Allergies).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AllergyUser",
                    r => r.HasOne<Allergy>().WithMany()
                        .HasForeignKey("AllergyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AllergyUs__Aller__398D8EEE"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AllergyUs__UserI__38996AB5"),
                    j =>
                    {
                        j.HasKey("UserId", "AllergyId").HasName("PK__AllergyU__2DC127A8B288C6F6");
                        j.ToTable("AllergyUser");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
