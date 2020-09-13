using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace _4000Client
{
    //=========================================================================================
    //
    //      남은 패 배열.. 패없앨때마다. 어레이를 삭제해서 나중에 게임오버를 찾기위해 쓴다.
    //
    //=========================================================================================

    class PaeLeftList
    {
        private ArrayList _arrPaeLeft;
        public PaeLeftList()
        {
            _arrPaeLeft = new ArrayList();
        }


        public PaeLeft this[int index]
        {
            get { return (PaeLeft)_arrPaeLeft[index]; }
        }
        public int Count
        {
            get { return _arrPaeLeft.Count; }
        }
        public void RemoveAllPaeLeft()
        {
            _arrPaeLeft.Clear();
        }
        public void RemovePaeLeftAt(int index)
        {
            _arrPaeLeft.RemoveAt(index);
        }
        public void AddPaeLeft(PaeLeft p)
        {
            _arrPaeLeft.Add(p);
        }
    }
}
