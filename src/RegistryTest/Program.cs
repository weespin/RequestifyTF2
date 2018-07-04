using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Brush = System.Windows.Media.Brush;
using Size = System.Windows.Size;

namespace RegistryTest
{
    class Program
    {
        public class ImageHelper
        {

            /// <summary>
            /// Конвертирует System.Windows.Media.Imaging.BitmapImage в System.Drawing.Bitmap.
            /// </summary>
            /// <param name="bitmapImage">Изображение для конвертирования</param>
            /// <returns></returns>
            public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
            {
                using (var outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new PngBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                    enc.Save(outStream);
                    var bitmap = new Bitmap(outStream);

                    return new Bitmap(bitmap);
                }
            }

            /// <summary>
            /// Кисть, которую нужно преобразовать в System.Windows.Media.ImageSource
            /// </summary>
            /// <param name="brush">Кисть</param>
            /// <param name="size">Размер объекта</param>
            /// <param name="margin">Отступы</param>
            /// <param name="fileName">Имя файла, в который будет сохранено изображение в формате PNG</param>
            /// <returns></returns>
            public static ImageSource BrushToImageSource(Brush brush, Size size, Thickness margin, string fileName)
            {
                var bitmap = new RenderTargetBitmap(
                    (int)(size.Width + margin.Left + margin.Right),
                    (int)(size.Height + margin.Top + margin.Bottom),
                    96, 96, PixelFormats.Pbgra32);

                var drawingVisual = new DrawingVisual();
                using (DrawingContext context = drawingVisual.RenderOpen())
                {
                    var rect = new Rect(margin.Left, margin.Top, size.Width, size.Height);
                    context.DrawRectangle(brush, null, rect);
                }

                bitmap.Render(drawingVisual);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    encoder.Save(fs);
                }

                return bitmap;
            }

            /// <summary>
            /// Кисть, которую нужно преобразовать в System.Windows.Media.ImageSource
            /// </summary>
            /// <param name="brush">Кисть</param>
            /// <param name="size">Размер объекта</param>
            /// <param name="fileName">Имя файла, в который будет сохранено изображение в формате PNG</param>
            /// <returns></returns>
            public static ImageSource BrushToImageSource(Brush brush, Size size, string fileName)
            {
                return BrushToImageSource(brush, size, new Thickness(0d), fileName);
            }

            #region Конвертер форматов (BitmapSource в BitmapImage)
            /// <summary>
            /// Конвертировать BitmapSource в BitmapImage используя PngBitmapEncoder
            /// </summary>
            /// <param name="bitmapSource">BitmapSource который нужно конвертировать</param>
            /// <returns></returns>
            public static BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
            {
                var encoder = new PngBitmapEncoder();
                using (var ms = new MemoryStream())
                {
                    var bImg = new BitmapImage();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(ms);
                    bImg.BeginInit();
                    bImg.StreamSource = new MemoryStream(ms.ToArray());
                    bImg.EndInit();
                    return bImg;
                }
            }
            #endregion
        }
        class SteamGame
        {
            public int id;
            public string photolink;
            public string Name;
            public string path;
        }
        static void Main(string[] args)
        {
            List<SteamGame> SteamIdList = new List<SteamGame>();
            var ProgramList = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\",RegistryRights.ReadKey).GetSubKeyNames();     
            var Regex = new Regex(@"Steam App (\d+)");
            foreach (var v in ProgramList)
            {

                var a = Regex.Match(v);
                if (a.Success)
                {
                   
                   
                    SteamGame game = new SteamGame();
                 game.id = Convert.ToInt32(a.Groups[1].Value) ;
                    game.path =  RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                        .OpenSubKey($"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{v}",
                            RegistryRights.ReadKey).GetValue("InstallLocation").ToString();
                  
                    var barak = $"https://store.steampowered.com/api/appdetails?appids={a.Groups[1].Value}";
                    var ass = new WebClient().DownloadString(
                      barak);
                  
                    JObject jObject = JObject.Parse(ass);
          
                    
                        var root = jObject[a.Groups[1].ToString()].Value<JObject>().ToObject<Root>();
                    if (root.success)
                    {
                        Console.WriteLine(root.data.name);
                        game.Name = root.data.name;
                        game.photolink = root.data.header_image;
                    }
                    SteamIdList.Add(game);

                }
            }

            
        }

        public class Root
        {
            public bool success { get; set; }
            public Data data { get; set; }
        }
        public class Data
        {
            public string name { get; set; }
            public string header_image { get; set; }
         
        }


    }
}

