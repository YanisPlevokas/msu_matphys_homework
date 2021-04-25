using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FirstSemesterLib;

namespace SecondSemesterLab_UI
{
    public class Second_Lab_class : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private V4MainCollection MainColl;
        private string name;
        private double minfo;
        private int count;
        private float minValue, maxValue;

        public Second_Lab_class(ref V4MainCollection mainColl) : this()
        {
            MainColl = mainColl;
        }
        public Second_Lab_class() { }
        public void SetMainCollection(V4MainCollection mainColl)
        {
            MainColl = mainColl;
            string temp = name;
            Name = "";
            Name = temp;
        }

        public void Adding()
        {
            Random rnd = new Random();
            string info = Name;
            double meas_info = MInfo;
            V4DataCollection data = new V4DataCollection(info, meas_info);
            data.InitRandom(count, MinValue, MaxValue, (double)MinValue, (double)MaxValue);
            MainColl.Add(data);
        }


        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public int Count
        {
            get => count;
            set
            {
                count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
        }
        public float MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinValue"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxValue"));
            }
        }
        public double MInfo
        {
            get => minfo;
            set
            {
                minfo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Minfo"));
            }
        }
        public float MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxValue"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinValue"));
            }
        }
      

        public string this[string columnName]
        {
            get
            {
                string msg = null;
                switch (columnName)
                {
                    case "Name":
                        if (MainColl == null)
                            break;
                        if (Name == null) 
                            msg = $"Name cannot be null";
                        else if (Name.Length < 1) 
                            msg = $"Name length must be at least 1";
                        else if (MainColl.Contains(Name)) 
                            msg = $"MainCollection is already contains \"{Name}\"";
                        break;
                    case "Count":
                        if (Count <= 1 || Count >= 5) 
                            msg = "Count must be greater than 1 and smaller than 5";
                        break;
                    case "MinValue":
                    case "MaxValue":
                        if (MinValue >= MaxValue) msg = "MinValue must be less than MaxValue";
                        break;
                    case "Minfo":
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        public string Error
        {
            get
            {
                string msg = this["Name"];
                string msg2 = this["Count"];
                string msg3 = this["MinValue"];
                if (msg == null) 
                    msg = msg2;
                else if (msg2 != null) 
                    msg += ", " + msg2;
                if (msg == null) 
                    msg = msg3;
                else if (msg3 != null) 
                    msg += ", " + msg3;
                return msg;
            }
        }

    }
}
