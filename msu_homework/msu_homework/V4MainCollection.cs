using lab3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;

namespace msu_homework
{
    class V4MainCollection : IEnumerable<V4Data>
    {

        public event DataChangedEventHandler DataChanged;

        public void OnDataChanged(object source, DataChangedEventArgs args)
        {
            DataChanged?.Invoke(source, args);
        }

        public void PropertyC(object sender, PropertyChangedEventArgs args)
        {
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, num));
        }

        private List<V4Data> list;

        public int num;
        public int count
        {
            set { }
            get
            {
                return list.Count();
            }
        }
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
            get
            {
                var query = list.SelectMany(x => x).ToList();
                var query1 = from item1 in query
                             from item2 in query
                             select Vector2.Distance(item1.net, item2.net);
                var max = query1.OrderByDescending(query1 => query1).First();
                return max;
            }
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
            num = 0;
        }
        public void Add(V4Data item)
        {
            item.PropertyChanged += PropertyC;
            list.Add(item);
            num++;
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, num));
        }

        public V4Data this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                value.PropertyChanged += PropertyC;
                list[index] = value;
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace, num));
            }
        }



        public bool Remove(string id, double w)
        {
            //int flag = 0;
            //flag = list.RemoveAll(item => item.measures_info == id && item.frequency_info == w);
            List<V4Data> tmp = list.FindAll(item => (item.frequency_info == w) && (item.measures_info == id));
            int count = tmp.Count;
            if (count != 0)
            {
                tmp[0].PropertyChanged -= PropertyC;
                num -= count;
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, num));
            }

            return count > 0;
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
