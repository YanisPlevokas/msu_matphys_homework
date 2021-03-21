using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace FirstSemesterLib
{
    [Serializable]

    public class V4DataCollection : V4Data, IEnumerable<DataItem>
    {
        public Dictionary<Vector2, Complex> dict;

        public V4DataCollection(string measure, double frequency) : base(measure, frequency)
        {
            this.dict = new Dictionary<Vector2, Complex>();
        }
        public V4DataCollection(string filename)  : base("", 0)
        {
            FileStream fs = null;
            V4DataCollection dataSet = null;
            string[] vectInfo;
            Dictionary<Vector2, Complex> dict_new = new Dictionary<Vector2, Complex>();
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                StreamReader istream = new StreamReader(fs);
                string parsingArg = istream.ReadLine();
                if (parsingArg == null)
                    throw new Exception("no measure\n");
                string measures = parsingArg;
                parsingArg = istream.ReadLine();
                if (parsingArg == null)
                    throw new Exception("no freq info\n");
                double frequency = Convert.ToDouble(parsingArg);
                while ((parsingArg = istream.ReadLine()) != null)
                {
                    vectInfo = parsingArg.Split(' ');
                    if (vectInfo.Length != 4)
                        throw new Exception("length problem\n");
                    dict_new.Add(new Vector2(Convert.ToSingle(vectInfo[0]), Convert.ToSingle(vectInfo[1])), new Complex(Convert.ToDouble(vectInfo[2]), Convert.ToDouble(vectInfo[3])));
                }
                measures_info = measures;
                frequency_info = frequency;
            }
            catch (Exception e)
            {
                dataSet = null;
                System.Console.WriteLine("Parse error");
                System.Console.WriteLine(e.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            dict = dict_new;
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

        public override string ToLongString(string format)
        {
            string s = this.ToString();
            foreach (KeyValuePair<Vector2, Complex> kvp in dict)
            {
                s = s + kvp.Key.X + " - x coord, " + kvp.Key.Y + " - y coord, " + kvp.Value.Magnitude.ToString(format) + " - value.\n";
            }

            return s;
        }

        public override string ToString()
        {
            return "type - V4DataCollection, " + measures_info + " - meausers info, " + frequency_info + " - frequency info, " + dict.Count + " - number of elems.\n";
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            return new DataEnumerator(dict);
        }
        private class DataEnumerator : IEnumerator<DataItem>
        {
            private Dictionary<Vector2, Complex> dict_new;
            public int position = -1;
            object IEnumerator.Current => Current;
            public DataItem Current
            {
                get
                {
                    return new DataItem(dict_new.Keys.ElementAt(position), dict_new.Values.ElementAt(position));
                }
            }
            public DataEnumerator(Dictionary<Vector2, Complex> dict)
            {
                dict_new = dict;
            }
            public bool MoveNext()
            {
                position += 1;
                return position < dict_new.Count;
            }
            public void Reset()
            {
                position = -1;
            }
            public void Dispose()
            {
                dict_new = null;
            }
        }
    }
}
