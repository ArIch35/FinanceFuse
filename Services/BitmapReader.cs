using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFuse.Services
{
    public static class BitmapReader
    {
        private static readonly string _assemblyName = Assembly.GetExecutingAssembly()?.GetName()?.Name ?? "FinanceFuse";
        public static readonly string DEFAULT_LOGO = $"avares://{_assemblyName}/Assets/avalonia-logo.ico";
        public static Bitmap ReadBitmapFromStringURI(string uri)
        {
            return new Bitmap(AssetLoader.Open(CreateURI(uri)));
        }

        private static Uri CreateURI(string uri)
        {
            if (Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var result))
            {
                if (!result.IsAbsoluteUri)
                {
                    return new Uri($"avares://{_assemblyName}/{uri}");
                }
                return result;
            }
            return new Uri(DEFAULT_LOGO);
        }
    }
}
