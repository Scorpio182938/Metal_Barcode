using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class UI_Config
    {
        private string config_id;
        private string machine_id;
        private string mould_index;
        private string mould_id;
        private string spare_1;
        private string spare_2;
        private string spare_3;
        private string tray1;
        private string tray2;
        private string tray3;
        private string dong;
        private string print;
        private int tar_cnt;
        private string tar_class;
        private string ptype;

        public string Config_id { get => config_id; set => config_id = value; }
        public string Machine_id { get => machine_id; set => machine_id = value; }
        public string Mould_index { get => mould_index; set => mould_index = value; }
        public string Mould_id { get => mould_id; set => mould_id = value; }
        public string Spare_1 { get => spare_1; set => spare_1 = value; }
        public string Spare_2 { get => spare_2; set => spare_2 = value; }
        public string Spare_3 { get => spare_3; set => spare_3 = value; }
        public string Tray1 { get => tray1; set => tray1 = value; }
        public string Tray2 { get => tray2; set => tray2 = value; }
        public string Tray3 { get => tray3; set => tray3 = value; }
        public string Dong { get => dong; set => dong = value; }
        public string Print { get => print; set => print = value; }
        public int Tar_cnt { get => tar_cnt; set => tar_cnt = value; }
        public string Tar_class { get => tar_class; set => tar_class = value; }
        public string Ptype { get => ptype; set => ptype = value; }
    }
}
