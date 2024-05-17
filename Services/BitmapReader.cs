using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;
using System.Reflection;

namespace FinanceFuse.Services
{
    public static class BitmapReader
    {
        private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "FinanceFuse";
        private static readonly string DefaultLogo = $"avares://{AssemblyName}/Assets/avalonia-logo.ico";
        public static Bitmap ReadBitmapFromStringUri(string uri)
        {
            try
            {
                return new Bitmap(AssetLoader.Open(CreateUri(uri)));
            }
            catch (FileNotFoundException)
            {
                return new Bitmap(AssetLoader.Open(CreateUri(DefaultLogo)));
            }
        }

        private static Uri CreateUri(string uri)
        {
            if (!(Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var result)))
            {
                return new Uri(DefaultLogo);
            }
            
            return !result.IsAbsoluteUri ? new Uri($"avares://{AssemblyName}/{uri}") : result;
        }
    }
}
