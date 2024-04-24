using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace rdp_products_pictures
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Current directory path: " +
                      Directory.GetCurrentDirectory());
            string kiindulasifajlut = "C:\\kepkuldes";
            string celfajlbasic = "C:\\DNN\\Portals\\0\\Hotcakes\\Data\\products\\";
            string[] kepek = Directory.GetFiles(kiindulasifajlut);
            for (int i = 0; i < kepek.Length; i++)
            {
                try
                {
                    string kepnev = kepek[i].Split(';')[0].Replace(kiindulasifajlut + "\\", "") + '.' + kepek[i].Split('.').Last();
                    string mappanev = kepek[i].Split(';').Last().Split('.')[0].ToLower();
                    string celmappa = celfajlbasic + "\\" + mappanev;
                    if (Directory.Exists(celmappa))
                    {
                        Directory.Delete(celmappa, true);
                    }
                    Directory.CreateDirectory(celmappa);

                    string smallmappa = celmappa + "\\small";
                    Directory.CreateDirectory(smallmappa);

                    string mediummappa = celmappa + "\\medium";
                    Directory.CreateDirectory(mediummappa);

                    string teljeskeput = celmappa + "\\" + kepnev;
                    System.IO.File.Move(kepek[i], teljeskeput);

                    Image eredetikep = Image.FromFile(teljeskeput);
                    int magassag = eredetikep.Height;
                    int szelesseg = eredetikep.Width;
                    Double arany = 0;
                    if (szelesseg > magassag)
                    {
                        arany = Convert.ToDouble(szelesseg) / 440;

                    }
                    else
                    {
                        arany = Convert.ToDouble(magassag) / 440;
                    }

                    int ujmagassag = Convert.ToInt32(magassag / arany);
                    int ujszelesseg = Convert.ToInt32((szelesseg / arany));
                    Image kozepeskep = ResizeImage(eredetikep, ujszelesseg, ujmagassag);


                    kozepeskep.Save(mediummappa + "\\" + kepnev);



                    Double arany2 = arany * 2;
                    int ujmagassag2 = Convert.ToInt32((magassag / arany2));
                    int ujszelesseg2 = Convert.ToInt32((szelesseg / arany2));
                    Image kiskep = ResizeImage(eredetikep, ujszelesseg2, ujmagassag2);


                    kiskep.Save(smallmappa + "\\" + kepnev);

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message + " at " + kepek[i]);
                }

            }

            

        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }



    }



}
