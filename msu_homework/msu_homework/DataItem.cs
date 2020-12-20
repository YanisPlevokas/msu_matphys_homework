using System.Numerics;

namespace msu_homework
{
    struct DataItem
    {
        public Vector2 net { get; set; }
        public Complex electromagnet_field { get; set; }
        public DataItem(Vector2 net_new, Complex electromagnet_field_new)
        {
            this.net = net_new;
            this.electromagnet_field = electromagnet_field_new;
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