using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frctl
{
    public static class Mandelbrot
    {
        public struct Config
        {
            public int canvaswidth { get; init; }
            public int canvasheight { get; init; }
            public int maxiterations { get; init; }

            public float zoom { get; init; }
            public float verticalzoomratio { get; init; }
            public float maxdist { get; init; }
            public float offsetx { get; init; }
            public float offsety { get; init; }
        }

        public static byte[,] ComputeImage(Config c)
        {
            byte[,] image = new byte[c.canvasheight, c.canvaswidth];

            for (int i = 0; i < c.canvaswidth; i++)
            {
                for (int j = 0; j < c.canvasheight; j++)
                {
                    float x = (c.offsetx + i - c.canvaswidth / 2) * c.zoom;
                    float y = (c.offsety + j - c.canvasheight / 2) * c.zoom * c.verticalzoomratio;

                    image[j, i] = ComputeColor(x, y, c.maxiterations, c.maxdist);
                }
            }

            return image;
        }

        private static byte ComputeColor(float x, float y, int maxiterations, float maxdistance)
        {
            float startx = x;
            float starty = y;

            int count = 0;

            while (count < maxiterations)
            {
                float oldx = x;
                x = x * x - y * y;
                y = 2 * oldx * y;

                x += startx;
                y += starty;

                if (PointDistance(startx, starty, x, y) > maxdistance)
                {
                    float percent = (float)count / (float)maxiterations;
                    return (byte)(percent * 255);
                }

                count++;
            }
            
            return 255;
        }

        private static double PointDistance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt((Math.Pow(x1 - y1, 2) + Math.Pow(x2 - y2, 2)));
		}
    }
}
