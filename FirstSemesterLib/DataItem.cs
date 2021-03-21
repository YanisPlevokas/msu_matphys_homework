using System;
using System.Numerics;
using System.Runtime.Serialization;


namespace FirstSemesterLib
{
    [Serializable]
    public struct DataItem : ISerializable
    {
        public Complex electromagnet_field { get; set; }
        public Vector2 net { get; set; }
        public DataItem(Vector2 net_new, Complex electromagnet_field_new)
        {
            this.net = net_new;
            this.electromagnet_field = electromagnet_field_new;
        }
        public DataItem(SerializationInfo info, StreamingContext context)
        {
            double real = info.GetSingle("real");
            double compl = info.GetSingle("compl");
            float x = info.GetSingle("X");
            float y = info.GetSingle("Y");
            electromagnet_field = new Complex(real, compl);
            net = new Vector2(x, y);
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("real", electromagnet_field.Real);
            info.AddValue("compl", electromagnet_field.Imaginary);
            info.AddValue("X", net.X);
            info.AddValue("Y", net.Y);
        }

        public override string ToString()
        {
            return this.net.ToString() + " - " + this.electromagnet_field.ToString();
        }
        public string ToString(string format)
        {
            return this.net.ToString() + " - " + this.electromagnet_field.ToString(format) + ", " + this.electromagnet_field.Magnitude.ToString(format);
        }
    }
}