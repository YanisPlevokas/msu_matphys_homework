using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace FirstSemesterLib
{
    [Serializable]

    public abstract class V4Data : IEnumerable<DataItem>, INotifyPropertyChanged
    {
        [field: NonSerialized]

        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyC(string pname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pname));
        }
        protected string measures_info;
        protected double frequency_info;
        public string MInfo 
        {
            get 
            { 
                return measures_info; 
            }
            set 
            { 
                measures_info = value;
                PropertyC("MInfo");
            } 
        }

        public double FInfo 
        { 
            get
            {
                return frequency_info;
            }
            set
            {
                frequency_info = value;
                PropertyC("FInfo");
            }
        }

        public V4Data(string measures, double frequency)
        {
            MInfo = measures;
            FInfo = frequency;
        }
        public abstract Complex[] NearMax(float eps);
        public abstract override string ToString();
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
