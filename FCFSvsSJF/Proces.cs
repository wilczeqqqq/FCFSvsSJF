using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCFSvsSJF
{
    internal class Proces
    {
        private int _ID; // ID procesu
        private int _at; // Arrival Time
        private int _bt; // Burst Time
        private int _wt; // Waiting Time
        private int _tat; // Turnaround Time
        private int _ft; // Finish Time

        public Proces(int ID, int at, int bt)
        {
            _ID = ID;
            _at = at;
            _bt = bt;
        }

        public int ID { get => _ID; set => _ID = value; }
        public int AT { get => _at; set => _at = value; }
        public int BT { get => _bt; set => _bt = value; }
        public int WT { get => _wt; set => _wt = value; }
        public int TAT { get => _tat; set => _tat = value; }
        public int FT { get => _ft; set => _ft = value; }
    }
}