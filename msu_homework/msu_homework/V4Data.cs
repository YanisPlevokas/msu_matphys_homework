using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace msu_homework
{
    abstract class V4Data : IEnumerable<DataItem>
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyC(object source, string pname)
        {
            PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(pname));
        }

        public string measures_info { get; set; }

        public double frequency_info { get; set; }

        public V4Data(string measures, double frequency)
        {
            this.measures_info = measures;
            this.frequency_info = frequency;
        }
        public abstract Complex[] NearMax(float eps);
        public abstract override string ToString();
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        public abstract IEnumerator<DataItem> GetEnumerator();

        public string CInfo
        {
            get { return measures_info; }
            set
            {
                measures_info = value;
                PropertyC(this, "СInfo");
            }
        }
        public double CFrequency
        {
            get { return frequency_info; }
            set
            {
                frequency_info = value;
                PropertyC(this, "CFrequency");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
