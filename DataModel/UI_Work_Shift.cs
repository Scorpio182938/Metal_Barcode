using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class UI_Work_Shift
    {
        private string _name;
        private int _uph;
        private DateTime _start_Time;
        private DateTime _end_Time;

        public string Name { get => _name; set => _name = value; }
        public int Uph { get => _uph; set => _uph = value; }
        public DateTime Start_Time { get => _start_Time; set => _start_Time = value; }
        public DateTime End_Time { get => _end_Time; set => _end_Time = value; }
    }
}
