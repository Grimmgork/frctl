using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using frctl.Commands;

namespace frctl
{
    class Program
    { 
        string[] parameterNames = new string[9] { "canvaswidth", "canvasheight", "zoom", "verticalzoomratio", "maxiterations", "maxdist", "offsetx", "offsety", "charmap" };
        string[] parameterNamesShortcut = new string[9] { "cw", "ch", "z", "vzr", "mi", "md", "ox", "oy", "cm" };

        static void Main(string[] args)
        {
            string configFilePath = Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName + "/state.json";
            AppState state = JsonConvert.DeserializeObject<AppState>(File.ReadAllText(configFilePath));//load state
            //modify state
            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(state));//save state 

            PrintState(state);

            //CliCommandBase cmd = new RootCommand();
            //Console.WriteLine(cmd.Route(args));

            Mandelbrot.Config fconfig = new Mandelbrot.Config() { 
                canvaswidth = state.canvaswidth, 
                canvasheight = state.canvasheight,
                zoom = state.zoom,
                verticalzoomratio = state.verticalzoomratio,
                maxiterations = state.maxiterations,
                maxdist = state.maxdist,
                offsetx = state.offsetx,
                offsety = state.offsety
            };

            byte[,] image = Mandelbrot.Compute(fconfig);
            PrintImageToConsole(image, state.charmap);
        }

        private static void PrintState(AppState state)
        {
            
        }

        private static void PrintImageToConsole(byte[,] image, string colorMap)
        {
		    if (colorMap == String.Empty || colorMap == null)
			    return;

            Console.WriteLine(colorMap);
            Console.WriteLine("".PadRight(colorMap.Length, '~'));

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
        
        struct AppState
        {
            public int canvaswidth { get; init; }
            public int canvasheight { get; init; }
            public int maxiterations { get; init; }
            public float zoom { get; init; }
            public float verticalzoomratio { get; init; }
            public float maxdist { get; init; }
            public float offsetx { get; init; }
            public float offsety { get; init; }

            public string charmap { get; init; }
            public string exportpath { get; init; }
            public byte[,] buffer { get; init; }
        }
    }
}
