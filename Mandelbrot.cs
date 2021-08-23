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

        public static byte[,] Compute(Config c)
        {
            byte[,] image = new byte[c.canvasheight, c.canvaswidth];
            for (int i = 0; i < c.canvaswidth; i++)
            {
                for (int j = 0; j < c.canvasheight; j++)
                {
                    float x = (c.offsetx + i - c.canvaswidth / 2) * c.zoom;
                    float y = (c.offsety + j - c.canvasheight / 2) * c.zoom * c.verticalzoomratio;
                    float startx = x;
                    float starty = y;

                    bool infinite = false;
                    int count = 0;

                    while (count < c.maxiterations)
                    {
                        if (x > c.maxdist || x < -c.maxdist)
                        {
                            infinite = true;
                            break;
                        }

                        if (y > c.maxdist || y < -c.maxdist)
                        {
                            infinite = true;
                            break;
                        }

                        float oldx = x;
                        x = x * x - y * y;
                        y = 2 * oldx * y;

                        x += startx;
                        y += starty;

                        count++;
                    }

                    if (infinite)
                    {
                        float percent = (float)count / (float)c.maxiterations;
                        byte res = (byte)(percent * 255);
                        image[j, i] = res;
                    }
                    else
                        image[j, i] = 255;
                }
            }

            return image;
        }
    }
}
