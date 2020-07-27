using System.Collections.Generic;
using System.ComponentModel;

namespace MyFitTexodeTest.Model
{
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string UserName { get; set; }
        public int AvgStep { get; set; }
        public int MaxStep { get; set; }
        public int MinStep { get; set; }
        public int Rank { get; set; }
        public string Status { get; set; }

        public List<int> Steps { get; set; }

        public string ColorSet
        {
            get
            {
                if ((MaxStep - (AvgStep * 0.2)) > AvgStep)
                {
                    return "DarkSeaGreen";
                }

                if ((MinStep + (AvgStep * 0.2)) < AvgStep)
                {
                    return "PaleVioletRed";
                }

                return "White";
            }
        }
    }
}
    