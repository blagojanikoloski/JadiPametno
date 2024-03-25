using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Xml;
using JadiPametno.Models;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<Recipe> Recipe { get; set; }
    public DbSet<Ingredient> Ingredient { get; set; }
    public DbSet<RecipeHasIngredient> RecipeHasIngredient { get; set; }

}
