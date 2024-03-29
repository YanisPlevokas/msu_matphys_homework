﻿using lab3;
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
    public class V4MainCollection : IEnumerable<V4Data>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private List<V4Data> list;

        [field: NonSerialized]
        public event DataChangedEventHandler DataChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool IfChangedCollection = false;


        private void OnChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            IfChangedCollection = true;
            //MaxMagnNew = MaxMagn;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxMagn"));

        }
        public void OnDataChanged(object source, DataChangedEventArgs args)
        {
            DataChanged?.Invoke(source, args);

        }
        public void PropertyC(object sender, PropertyChangedEventArgs args)
        {
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, num));

        }

        //private DataItem MaxMagnNew;
        public DataItem MaxMagn
        {
            get
            {
                var united = list.SelectMany(x => x);
                if (united != null && united.Any())
                    {
                    return united.Aggregate((max, v) => v.electromagnet_field.Magnitude > max.electromagnet_field.Magnitude ? v : max);
                }
                return new DataItem(new Vector2(0, 0), new Complex(0, 0));
            }
            set
            {
                MaxMagn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxMagn"));
            }
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
                IfChangedCollection = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Save Problem" + ex.Message);

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
                IfChangedCollection = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load Problem" + ex.Message);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        public void Add(V4Data item)
        {
            if (Contains(item.MInfo))
            {
                throw new Exception("Object already in dataset, input some distinct params\n");
                return;
            }
            item.PropertyChanged += PropertyC;
            list.Add(item);
            num++;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, num));

        }
        public bool Remove(string id, double w)
        {
            bool flag = false;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((list[i].MInfo == id) && (list[i].FInfo == w))
                {
                    list[i].PropertyChanged -= PropertyC;
                    list.RemoveAt(i);
                    flag = true;
                    num--;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
            if (flag)
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, num));
            return flag;
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
            Random rnd = new Random();
            Grid2D just_object = new Grid2D((float)2.1, 4, (float)2.1, 4);
            V4DataOnGrid onGrid_object = new V4DataOnGrid("grid" + rnd.Next().ToString(), 2.3, just_object);
            V4DataCollection collection_object = new V4DataCollection("collection" + rnd.Next().ToString(), 2.3);
            int number_of_new_objects = 5;
            double minVal = 12.0;
            double maxVal = 24.0;
            for (int i = 0; i < 1; i++)
            {
                onGrid_object.InitRandom(minVal, maxVal);
                collection_object.InitRandom(number_of_new_objects, (float)rnd.NextDouble(), (float)rnd.NextDouble(), minVal, maxVal);
                list.Add(onGrid_object);
                list.Add(collection_object);
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public bool Contains(string id)
        {
            foreach (V4Data data in list)
                if (data.MInfo == id)
                    return true;
            return false;
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
