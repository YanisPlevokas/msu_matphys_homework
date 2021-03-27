using lab3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FirstSemesterLib
{
    public class V4MainCollection : IEnumerable<V4Data>, INotifyCollectionChanged
    {
        private List<V4Data> list;


        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public bool HasUnsavedChanges { get; private set; }

        private void OnChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            HasUnsavedChanges = true;
        }

        public int num;
        public int count
        {
            set { }
            get
            {
                return list.Count();
            }
        }

        public V4MainCollection()
        {
            list = new List<V4Data>();
            CollectionChanged += OnChange;
            num = 0;
        }

        public void Save(string filename)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Open(filename, FileMode.OpenOrCreate);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(fileStream, list);
                HasUnsavedChanges = false;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        public void Load(string filename)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter serializer = new BinaryFormatter();
                list = (List<V4Data>)serializer.Deserialize(fileStream);
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                HasUnsavedChanges = false;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        public void Add(V4Data item)
        {
            list.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public bool Remove(string id, double w)
        {
            bool flag = false;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((list[i].MInfo == id) && (list[i].FInfo == w))
                {
                    list.RemoveAt(i);
                    flag = true;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
            return flag;
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

        public void AddDefaults()
        {
            Grid2D just_object = new Grid2D((float)2.1, 4, (float)2.1, 4);
            V4DataOnGrid onGrid_object = new V4DataOnGrid("hello", 2.3, just_object);
            V4DataCollection collection_object = new V4DataCollection("hello_new", 2.3);
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
