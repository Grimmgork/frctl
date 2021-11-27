using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;

namespace frctl
{
    class Program
    {
        static AppState state;
        string[] parameterNames = new string[9] { "canvaswidth", "canvasheight", "zoom", "verticalzoomratio", "maxiterations", "maxdist", "offsetx", "offsety", "charmap" };
        string[] parameterNamesShortcut = new string[9] { "cw", "ch", "z", "vzr", "mi", "md", "ox", "oy", "cm" };

        static void Main(string[] args)
        {
            string configFilePath = Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName + "/state.json";
            if(File.Exists(configFilePath))
            {
                state = JsonConvert.DeserializeObject<AppState>(File.ReadAllText(configFilePath));//load state
                File.WriteAllText(configFilePath, JsonConvert.SerializeObject(state));
            }
            else
            {
                state = new AppState();
                File.WriteAllText(configFilePath, JsonConvert.SerializeObject(state));
            }

            PrintImageToConsole(Mandelbrot.ComputeImage(state.fractalConfig), state.charmap);
        }

        private static void PrintImageToConsole(byte[,] image, string colorMap)
        {
		    if(colorMap == String.Empty || colorMap == null)
			    return;

            for (int i = 0; i < image.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < image.GetLength(1); j++)
                {
                  	byte imageValue = image[i, j];
                    int color = Utilities.MapValue(imageValue, 0, 255, 0, colorMap.Length-1);
                    line += colorMap[color];
                }
                Console.WriteLine(line);
		    }
	    }

        private static void SaveImageToFile(byte[,] image, string path)
        {
            int width = image.GetLength(1);
            int height = image.GetLength(0);

            Bitmap bmp = new Bitmap(width, height);
            for(int j = 0; j < height; j++)
            {
                for(int i = 0; i < width; i++)
                {
                    byte val = image[j, i];
                    bmp.SetPixel(i, j, Color.FromArgb(val,val,val));
                }
            }

            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }
       
        [System.Serializable]
        class AppState
        {
            public AppState()
            {
                fractalConfig = new Mandelbrot.Config()
                {
                    canvaswidth = 64,
                    canvasheight = 32,
                    maxiterations = 280,
                    zoom = 0.048f,
                    verticalzoomratio = 2f,
                    maxdist = 6f,
                    offsetx = -8f,
                    offsety = 0.9f
                };

                charmap = "  .'^\",:;Il!i><~+_-?\\,],[}{1)(|/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
                exportpath = "c:/users/users/eric/desktop";
                buffer = null;
            }
            
            public Mandelbrot.Config fractalConfig { get; init; }

            public string charmap { get; init; }
            public string exportpath { get; init; }
            public byte[,] buffer { get; init; }
        }
    }
}
