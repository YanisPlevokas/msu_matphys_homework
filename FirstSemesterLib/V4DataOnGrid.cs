using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;

namespace FirstSemesterLib
{
    [Serializable]
    public class V4DataOnGrid : V4Data, IEnumerable<DataItem>, ISerializable
    {
        public Grid2D net { get; set; }
        public Complex[,] complex_massiv { get; set; }
        public V4DataOnGrid(string measure, double frequency, Grid2D new_net) : base(measure, frequency)
        {
            this.net = new_net;
            this.complex_massiv = new Complex[net.OX_net_counter, net.OY_net_counter];
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
            double maximum = complex_massiv[0, 0].Magnitude;

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
                        mass_length *= 2;
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

        public override string ToLongString(string format)
        {
            string returned_string = this.ToString();
            int rank = complex_massiv.GetLength(0);
            int length = complex_massiv.GetLength(1);
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    returned_string = returned_string + string.Format("[{0},{1}] - coord, ", i, j) + complex_massiv[i, j].ToString(format) + " " + complex_massiv[i, j].Magnitude.ToString(format) + "\n";
                }
            }
            return returned_string;
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


        public V4DataOnGrid(SerializationInfo info, StreamingContext context)
        : base(info.GetString("base_measures"), info.GetDouble("base_frequency"))
        {
            net = (Grid2D)info.GetValue("net", typeof(Grid2D));
            complex_massiv = new Complex[net.OX_net_counter, net.OY_net_counter];
            int rank = complex_massiv.GetLength(0);
            int length = complex_massiv.GetLength(1);
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    complex_massiv[i, j] = info.GetSingle("i_" + i.ToString() + ",j_" +j.ToString());
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("base_measures", measures_info);
            info.AddValue("base_frequency", frequency_info);
            info.AddValue("net", net, typeof(Grid2D));
            int rank = complex_massiv.GetLength(0);
            int length = complex_massiv.GetLength(1);
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    info.AddValue("i_" + i.ToString() + ",j_" + j.ToString(), complex_massiv[i, j]);
                }
            }


        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            return new DataEnumerator(complex_massiv, net);
        }
        private class DataEnumerator : IEnumerator<DataItem>
        {
            private readonly Grid2D grid;
            private Complex[,] complex_massiv_enum;
            private int position_x = -1;
            private int position_y = 0;

            object IEnumerator.Current => Current;
            public DataItem Current
            {
                get
                {
                    return new DataItem(grid.GetCoord(position_x, position_y), complex_massiv_enum[position_x, position_y]);
                }
            }
            public DataEnumerator(Complex[,] values, Grid2D grid)
            {
                this.complex_massiv_enum = values;
                this.grid = grid;
            }
            public bool MoveNext()
            {
                if (position_x == complex_massiv_enum.GetLength(1) - 1)
                {
                    position_y++;
                    position_x = 0;
                }
                else
                    position_x++;
                return position_y < complex_massiv_enum.GetLength(0);
            }
            public void Reset()
            {
                position_x = -1;
                position_y = 0;
            }
            public void Dispose()
            {
                complex_massiv_enum = null;
            }
        }
    }
}
