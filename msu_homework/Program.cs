using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace msu_homework
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string data_path = @"C:\Users\white\source\repos\msu_homework\msu_homework\data.txt";
            V4DataCollection new_thing = new V4DataCollection(data_path);
            Console.WriteLine(new_thing.ToLongString());
            */
            V4MainCollection new_thing = new V4MainCollection();
            new_thing.AddDefaults();

            string data_path = @"C:\Users\white\source\repos\msu_homework\msu_homework\data.txt";
            V4DataCollection new_v4datacollect_thing = new V4DataCollection(data_path);
            Grid2D just_object = new Grid2D((float)2.1, 0, (float)2.1, 0);
            Grid2D just_object_test = new Grid2D((float)2.1, 2, (float)2.1, 2);
            V4DataOnGrid new_v4dataongrid_thing = new V4DataOnGrid("test", 12.5, just_object_test);
            new_v4dataongrid_thing.InitRandom(1, 2);
            new_thing.Add(new_v4datacollect_thing);
            new_thing.Add(new_v4dataongrid_thing);

            Console.WriteLine(new_thing);
            Console.WriteLine(new_thing.MaxLength);
            Console.WriteLine(new_thing.MaxMagn);
            Console.WriteLine(string.Join("\n", new_thing.Perechisl));

        }
    }
}