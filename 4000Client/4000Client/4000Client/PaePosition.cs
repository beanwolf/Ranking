using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace _4000Client
{

    class PaePosition
    {
        private ArrayList _arrPaePosition;
        public PaePosition()
        {
            _arrPaePosition = new ArrayList();
        }


        public PaePo this[int index]
        {
            get { return (PaePo)_arrPaePosition[index]; }
        }
        public int Count
        {
            get { return _arrPaePosition.Count; }
        }
        public void RemoveAllPaePosition()
        {
            _arrPaePosition.Clear();
        }
        public void RemovePaePositionAt(int index)
        {
            _arrPaePosition.RemoveAt(index);
        }
        public void AddPaePosition(PaePo p)
        {
            _arrPaePosition.Add(p);
        }
    }
}
