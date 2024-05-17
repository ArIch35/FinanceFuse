using System.Collections.Generic;
using Avalonia.Media.Imaging;
using FinanceFuse.Interfaces;
using FinanceFuse.Services;

namespace FinanceFuse.Models;

public enum CategoryType
{
    Income,
    Expense,
    None
} 

public class Category(string id, CategoryType type, string name, string? logoUrl = null): IModelBase
{
    public string Id { get; init; } = id;
    public CategoryType Type { get; init; } = type;
    public string Name { get; init; } = name;
    public string? LogoUrl { get; } = logoUrl;
    public Bitmap? LogoBitmap => BitmapReader.ReadBitmapFromStringUri(LogoUrl!);
    public List<Category> SubCategories { get; init; } = null!;
}