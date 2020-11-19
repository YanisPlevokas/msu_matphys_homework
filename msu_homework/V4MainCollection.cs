using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msu_homework
{
    class V4MainCollection : IEnumerable<V4Data>
    {
        private List<V4Data> list;

        public DataItem MaxMagn
        {
            get
            {
                var united = list.SelectMany(x => x);
                return united.Aggregate((max, v) => v.electromagnet_field.Magnitude > max.electromagnet_field.Magnitude ? v : max);
            }
        }

        public IEnumerable<DataItem> Perechisl
        {
            get
            {
                return list.SelectMany(x => x).OrderByDescending(v => v.electromagnet_field.Magnitude);
            }
        }

        public float MaxLength
        {
            get => list.Max(x => x.Max(x => x.net.LengthSquared()) );
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

        IEnumerator<V4Data> IEnumerable<V4Data>.GetEnumerator()
        {
            return ((IEnumerable<V4Data>)list).GetEnumerator();
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

            for (int i = 0; i < 1; i++)
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
                s += item.ToString();
            }
            return s;
        }

        public string ToLongString(string format)
        {
            string s = "";
            foreach (V4Data item in list)
            {
                s = s + item.ToLongString(format);
            }
            return s;
        }


    }
}
