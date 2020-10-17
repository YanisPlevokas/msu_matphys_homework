using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace msu_homework
{
    public struct Grid2D
    {
        public float OX_step { get; set; }
        public int OX_net_counter { get; set; }
        public float OY_step { get; set; }
        public int OY_net_counter { get; set; }

        public Grid2D(float step_x, int net_x, float step_y, int net_y)
        {
            this.OX_step = step_x;
            this.OX_net_counter = net_x;
            this.OY_step = step_y;
            this.OY_net_counter = net_y;
        }
        public override string ToString()
        {
            return OX_step.ToString() + " - OX_step, " + OX_net_counter.ToString() + " - OX_net_counter, " + 
                OY_step.ToString() + " - OY_step, " + OY_net_counter.ToString() + " - OY_net_counter."; 
        }

    }

    public abstract class V4Data
    { 
    
        public string measures_info { get; set; }

        public double frequency_info { get; set; }

        public V4Data(string measures, double frequency)
        {
            this.measures_info = measures;
            this.frequency_info = frequency;
        }
        public abstract Complex[] NearMax(float eps);
        public abstract string ToLongString();
        public abstract override string ToString();
    }

    public class V4DataOnGrid : V4Data
    {
        public Grid2D net { get; set; }
        public Complex[,] complex_massiv { get; set; }

        public V4DataOnGrid(string measure, double frequency, Grid2D new_net) : base(measure, frequency)
        {
            this.net = new_net;
            this.complex_massiv = new Complex[net.OX_net_counter,net.OY_net_counter];
        }
        public void InitRandom(double minValue, double maxValue)
        {
            Random rnd = new Random();
            double real_part;
            double imag_part;
            int rank = complex_massiv.GetLength(0);
            int length = complex_massiv.GetLength(1);
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    real_part = rnd.NextDouble() * (maxValue - minValue) + minValue;
                    imag_part = rnd.NextDouble() * (maxValue - minValue) + minValue;
                    complex_massiv[i, j] = new Complex(real_part, imag_part);
                }
            }
        }

        
        public override Complex[] NearMax(float eps)
        {
            double maximum = complex_massiv[0,0].Magnitude;

            int counter = 0;

            int rank = complex_massiv.GetLength(0);
            int length = complex_massiv.GetLength(1);

            Complex[] NearMaxReturnable = new Complex[10];

            int mass_length = 10;

            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    maximum = Math.Max(maximum, complex_massiv[i, j].Magnitude);
                }
            }

            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (Math.Abs(complex_massiv[i, j].Magnitude - maximum) <= eps)
                    {
                        NearMaxReturnable[counter++] = complex_massiv[i, j];
                    }

                    if (counter == mass_length - 2) // Если мы подходим к границе нашего массива для хранения значений, то увеличиваем размер массива в два раза
                    {
                        mass_length = mass_length * 2;
                        Array.Resize(ref NearMaxReturnable, mass_length); // Меняем размер нашего массива
                    }
                }
            }
            Array.Resize(ref NearMaxReturnable, counter); // Меняем размер нашего массива точно под количество элементов, которые удовлетворяют нашим свойствам

            return NearMaxReturnable;
        }

        public override string ToLongString()
        {
            string returned_string = this.ToString();
            int rank = complex_massiv.GetLength(0);
            int length = complex_massiv.GetLength(1);
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    returned_string = returned_string + string.Format("[{0},{1}] - coord, {2} - value.\n", i, j, complex_massiv[i, j]);
                }
            }
            return returned_string;
        }

        public override string ToString()
        {
            return "type - V4DataOnGrid, " + measures_info + " - meausers info, " + frequency_info + " - frequency info, " + net.OX_step + " - OX_step, " +
                net.OX_net_counter + " - OX net counter, " + net.OY_step + " - OY_step, " + net.OY_net_counter + " - OY net counter.\n";
        }

        public static explicit operator V4DataCollection(V4DataOnGrid obj)
        {
            V4DataCollection ret_obj = new V4DataCollection(obj.measures_info, obj.frequency_info);
            ret_obj.dict = new Dictionary<System.Numerics.Vector2, System.Numerics.Complex>();

            int rank = obj.complex_massiv.GetLength(0);
            int length = obj.complex_massiv.GetLength(1);
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Vector2 new_vector = new Vector2((float)i * obj.net.OX_step, (float)j * obj.net.OY_step);
                    Complex new_complex = obj.complex_massiv[i, j];
                    ret_obj.dict.Add(new_vector, new_complex);
                }
            }

            return ret_obj;
        }
    }


    public class V4DataCollection : V4Data
    {
        public Dictionary<System.Numerics.Vector2, System.Numerics.Complex> dict;

        public V4DataCollection(string measure, double frequency) : base(measure, frequency) 
        {
            this.dict = new Dictionary<System.Numerics.Vector2, System.Numerics.Complex>();
        }

        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {
            Random rnd = new Random();
            double x_curr;
            double y_curr;
            double field_value_real;
            double field_value_imag;
            for (int i = 0; i < nItems; i++)
            {
                x_curr = rnd.NextDouble() * (double)xmax;
                y_curr = rnd.NextDouble() * (double)ymax;
                field_value_real = rnd.NextDouble() * (maxValue - minValue) + minValue;
                field_value_imag = rnd.NextDouble() * (maxValue - minValue) + minValue;
                Vector2 new_vector = new Vector2((float)x_curr, (float)y_curr);
                Complex new_complex = new Complex(field_value_real, field_value_imag);
                dict.Add(new_vector, new_complex);
            }



        }
        public override Complex[] NearMax(float eps)
        {
            int counter = 0;

            double maximum = dict.First().Value.Magnitude;

            Complex[] NearMaxReturnable = new Complex[10];

            int mass_length = 10;

            foreach (Complex complex_value in dict.Values)
            {
                maximum = Math.Max(complex_value.Magnitude, maximum);
            }

            foreach (Complex complex_value in dict.Values)
            {

                if (Math.Abs(complex_value.Magnitude - maximum) <= eps)
                {
                    NearMaxReturnable[counter++] = complex_value;
                }
                if (counter == mass_length - 3) // Если мы подходим к границе нашего массива для хранения значений, то увеличиваем размер массива в два раза
                {
                    mass_length = mass_length * 2;
                    Array.Resize(ref NearMaxReturnable, mass_length); // Меняем размер нашего массива
                }
            }

            Array.Resize(ref NearMaxReturnable, counter); // Меняем размер нашего массива точно под количество элементов, которые удовлетворяют нашим свойствам

            return NearMaxReturnable;


        }
        public override string ToLongString()
        {
            string s = this.ToString();
            foreach (KeyValuePair<Vector2, Complex> kvp in dict)
            {
                s = s + kvp.Key.X + " - x coord, " + kvp.Key.Y + " - y coord, " + kvp.Value.Magnitude + " - value.\n";
            }

            return s;
        }

        public override string ToString()
        {
            return "type - V4DataCollection, " + measures_info + " - meausers info, " + frequency_info + " - frequency info, " + dict.Count + " - number of elems.\n";
        }

        


    }

    public class V4MainCollection
    {
        private List<V4Data> list;

        public interface IEnumerable
        {
            IEnumerator GetEnumerator();
        }
        public IEnumerator<V4Data> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public V4MainCollection()
        {
            this.list = new List<V4Data>();
        }
        public int count
        {
            set { }
            get 
            {
                return list.Count;
            }
        }
        public void Add(V4Data item)
        {
             list.Add(item);
        }

        public bool Remove(string id, double w)
        {
            int flag = 0;
            flag = list.RemoveAll(item => item.measures_info == id && item.frequency_info == w);
            return (flag > 0);
        }

        public void AddDefaults()
        {
            int n = 0;

            Grid2D just_object = new Grid2D((float)2.1, 4, (float)2.1, 4);
            V4DataOnGrid onGrid_object = new V4DataOnGrid("hello", 2.3, just_object);
            V4DataCollection collection_object = new V4DataCollection("hello", 2.3);
            int number_of_new_objects = 5;
            double minVal = 12.0;
            double maxVal = 24.0;
            Random rnd = new Random();

            for (int i = 0; i < 3 + n; i++)
            {
                onGrid_object.InitRandom(minVal, maxVal);
                collection_object.InitRandom(number_of_new_objects, (float)rnd.NextDouble(), (float)rnd.NextDouble(), minVal, maxVal);
                list.Add(onGrid_object);
                list.Add(collection_object);
            }
        }
        public override string ToString()
        {
            string s = "";
            foreach (V4Data item in list)
            {
                s = s + item.ToString();
            }
            return s;
        }
        

    }



    class Program
    {
        static void Main(string[] args)
        {
                                                                        /*
            Grid2D just_object = new Grid2D((float)2.1, 5, (float)4.2, 5);
            V4DataOnGrid grid_object = new V4DataOnGrid("hello", 2.4, just_object);
            grid_object.InitRandom(2.5, 10);
            Console.WriteLine(grid_object.ToLongString());
     
                                                                        

            V4DataCollection new_object = (V4DataCollection)grid_object;
            Console.WriteLine("\n");
            Console.WriteLine(new_object.ToLongString());
            Console.WriteLine("\n");
                                                                    */

                                                                    /*
            V4MainCollection new_main_obj = new V4MainCollection();
            new_main_obj.AddDefaults();
            Console.WriteLine(new_main_obj.ToString());

            foreach (var item in new_main_obj)
            {
                Complex[] obj = item.NearMax((float)0.001);
                for (int i = 0; i < obj.Length; i++)
                {
                    Console.WriteLine(obj[i].ToString());
                }
            }
                                                                    */


                                                                    /* REMOVE TEST
                                            //Grid2D just_object = new Grid2D((float)2.1, 5, (float)4.2, 5);
            V4MainCollection new_main_obj_1 = new V4MainCollection();
            V4DataOnGrid grid_object_0 = new V4DataOnGrid("hi", 2.4, just_object);
            V4DataOnGrid grid_object_1 = new V4DataOnGrid("hello", 2.4, just_object);
            V4DataOnGrid grid_object_2 = new V4DataOnGrid("hehe", 2.4, just_object);
            V4DataOnGrid grid_object_3 = new V4DataOnGrid("hello", 2.4, just_object);
            new_main_obj.Add(grid_object_0);
            new_main_obj.Add(grid_object_1);
            new_main_obj.Add(grid_object_2);
            new_main_obj.Add(grid_object_3);
            bool a = new_main_obj_1.Remove("hello", 2.4);
                                                                    */

        }
    }
}
