using lab3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace msu_homework
{
    class Program
    {

        public static void DataC(object sourse, DataChangedEventArgs args)
        {
            Console.WriteLine();
            Console.WriteLine(args.ToString());
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            /*
            string data_path = System.IO.Directory.GetCurrentDirectory() + @"\data.txt";
            V4DataCollection new_thing = new V4DataCollection(data_path);
            Console.WriteLine(new_thing.ToLongString());
            */

            /*
            V4MainCollection new_thing = new V4MainCollection();
            new_thing.AddDefaults();

            string data_path = System.IO.Directory.GetCurrentDirectory() + @"\data.txt";
            V4DataCollection new_v4datacollect_thing = new V4DataCollection(data_path);



            Grid2D just_object = new Grid2D((float)2.1, 0, (float)2.1, 0);
            Grid2D just_object_test = new Grid2D((float)2.1, 2, (float)2.1, 2);
            V4DataOnGrid new_v4dataongrid_thing = new V4DataOnGrid("test", 12.5, just_object_test);
            new_v4dataongrid_thing.InitRandom(1, 2);
            new_thing.Add(new_v4datacollect_thing);
            new_thing.Add(new_v4dataongrid_thing);

            Console.WriteLine(new_thing);
            Console.WriteLine("MaxLength");
            Console.WriteLine(new_thing.MaxLength);
            Console.WriteLine("MaxMagn");
            Console.WriteLine(new_thing.MaxMagn.ToString());
            Console.WriteLine("Perechisl");
            Console.WriteLine(string.Join("\n", new_thing.Perechisl));
            */

            Grid2D just_object_test = new Grid2D((float)2.1, 2, (float)2.1, 2);
            V4DataOnGrid test = new V4DataOnGrid("test", 12.5, just_object_test);
            Console.WriteLine(test.ToLongString("{0}"));
            V4MainCollection test_new = new V4MainCollection();
            test_new.DataChanged += DataC;
            test_new.Add(test);
            V4DataCollection AddTest = new V4DataCollection("add2", 7.6);
            test_new.Add(AddTest);
            V4DataCollection replTest = new V4DataCollection("rep", 5.6);
            test_new[0] = replTest;
            test_new[0].CInfo = "TestChangeValue";
            test_new.Remove("TestChangeValue", 5.6);
        }
    }
}