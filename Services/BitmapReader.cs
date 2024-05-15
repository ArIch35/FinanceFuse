using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Reflection;

namespace FinanceFuse.Services
{
    public static class BitmapReader
    {
        private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "FinanceFuse";
        private static readonly string DefaultLogo = $"avares://{AssemblyName}/Assets/avalonia-logo.ico";
        public static Bitmap ReadBitmapFromStringUri(string uri)
        {
            return new Bitmap(AssetLoader.Open(CreateUri(uri)));
        }

        private static Uri CreateUri(string uri)
        {
            if (Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var result))
            {
                if (!result.IsAbsoluteUri)
                {
                    return new Uri($"avares://{AssemblyName}/{uri}");
                }
                return result;
            }
            return new Uri(DefaultLogo);
        }
    }
}
