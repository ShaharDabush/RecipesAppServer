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

    public virtual DbSet<AllergyUser> AllergyUsers { get; set; }

    public virtual DbSet<Barkod> Barkods { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientRecipe> IngredientRecipes { get; set; }

    public virtual DbSet<IngredientStorage> IngredientStorages { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Allergy__3214EC07ECE3A987");
        });

        modelBuilder.Entity<AllergyUser>(entity =>
        {
            entity.HasOne(d => d.Allergy).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AllergyUs__Aller__3C69FB99");

            entity.HasOne(d => d.User).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AllergyUs__UserI__3B75D760");
        });

        modelBuilder.Entity<Barkod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Barkod__3214EC07E3E62CFA");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.Barkods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Barkod__Ingredie__37A5467C");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC0740FBC6A7");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__Recipe__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__UserId__3F466844");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC0762FB8309");

            entity.HasOne(d => d.Kind).WithMany(p => p.Ingredients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__KindI__2C3393D0");
        });

        modelBuilder.Entity<IngredientRecipe>(entity =>
        {
            entity.HasOne(d => d.Ingredient).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Ingre__30F848ED");

            entity.HasOne(d => d.Storage).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Stora__31EC6D26");
        });

        modelBuilder.Entity<IngredientStorage>(entity =>
        {
            entity.HasOne(d => d.Ingredient).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Ingre__33D4B598");

            entity.HasOne(d => d.Recipe).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Recip__34C8D9D1");
        });

        modelBuilder.Entity<Kind>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kind__3214EC0708D36660");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Levels__3214EC07A4D018AD");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Levels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Levels__RecipeId__46E78A0C");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rating__3214EC07E62C0AB2");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__RecipeId__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__UserId__4316F928");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipes__3214EC07DB91A127");

            entity.HasOne(d => d.MadeByNavigation).WithMany(p => p.Recipes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recipes__MadeBy__276EDEB3");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Storage__3214EC076EB33FAF");

            entity.HasOne(d => d.ManagerNavigation).WithMany(p => p.Storages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Storage__Manager__2F10007B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC071717C3D4");

            entity.HasOne(d => d.Storage).WithMany(p => p.Users).HasConstraintName("FK__Users__StorageId__47DBAE45");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
