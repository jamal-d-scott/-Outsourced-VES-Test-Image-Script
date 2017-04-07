/*
 * Jamal D. Scott
 * Arthur Byra
 * Tolls VES
 * Image Script
 * Computer Aid CAIHDC
 * 4/4/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace SuperfastBlur.VES_Image_Manipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalImages = 0, currentImage = 0, q;

            Console.Write("Enter initial number for image folder names: ");
            q = Convert.ToInt32(Console.ReadLine());
            Thread.CurrentThread.IsBackground = true;
            //Get the current directory that the program is executing in.
            String directory = Directory.GetCurrentDirectory();
            directory += @"\Images";

            //Set a list of all images in the Image folder.
            String[] files = System.IO.Directory.GetFiles(directory, "*", System.IO.SearchOption.TopDirectoryOnly);
            totalImages = files.Length*5;
            //Array to hold the images.
            Bitmap[] image;
            Bitmap temp;
            int blurAmount;

            //Outer loop increments through each file path in the list.
            for (int i = 0; i < files.Length; i++)
            {
                //Creates a new instance of the array to clear out the values that were left in the image array.
                image = new Bitmap[5];

                //Creates a new instance of random that will help scramble the images
                Random r = new Random();
                bool[] visited = new bool[5];

                //Creates a new sub-directory based on the image that we're looking at.
                String newDir = directory + @"\Image" + q;
                System.IO.Directory.CreateDirectory(newDir);

                for (int j = 0; j < 5; j++)
                {
                    /*
                     * For simplicity, set the blur ammount to an already incrementing value.
                     * because each new image needs to be blurrier
                     */
                    blurAmount = j*2;

                    if (j == 0)
                    {
                        image[j] = new Bitmap(files[i]);
                        temp = new Bitmap(image[j]);
                        Console.WriteLine("\nRetrieving Image: " + files[i]);
                    }
                    else
                        //Used the previously blurred image to make the next one blurrier.
                        temp = new Bitmap(image[j - 1]);

                    //Calls the method to blur our image.
                    var blur = new GaussianBlur(temp);

                    //Stores our newly blurred image.
                    image[j] = temp;

                    //inefficiently finds a number between 0 and 4 inclusively that hasn't been already used to assign as the name of the new image
                    int rand = r.Next(0, 5);

                    //Shuffles the ordering of the elements of the array.
                    while (visited[rand] == true)
                        rand = r.Next(0, 5);
                    visited[rand] = true;

                    //Writes the image to a jpg file in its directory.
                    var result = blur.Process(blurAmount);
                    result.Save(newDir + "/img" + rand+".jpg", ImageFormat.Jpeg);

                    Console.WriteLine("\nWrote: " + newDir + "/img" + rand + ".jpg");
                    currentImage++;
                    Console.WriteLine("\nProgress: " + currentImage + "/" + totalImages  + " processed");
                }
                q++;
            }
            Console.WriteLine("\nCompleted! All files have been saved to the directory: \n" + directory);
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
