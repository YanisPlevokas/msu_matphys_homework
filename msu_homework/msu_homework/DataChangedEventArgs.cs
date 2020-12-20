using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);
    public enum ChangeInfo
    {
        ItemChanged,
        Add,
        Remove,
        Replace
    }
    public class DataChangedEventArgs
    {
        public ChangeInfo chinf { get; set; }
        public int tparam { get; set; }
        public DataChangedEventArgs(ChangeInfo Nchinf, int Ntparam)
        {
            chinf = Nchinf;
            tparam = Ntparam;
        }
        public override string ToString()
        {
            return chinf.ToString() + "   " + tparam.ToString() + "\n";
        }
    }

}