using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace _4000Client
{
    class PaeList
    {
        private ArrayList _arrPae;
        public PaeList()
        {
            _arrPae = new ArrayList();
        }
        public Pae this[int index]
        {
            get { return (Pae)_arrPae[index]; }
        }

        public int Count
        {
            get { return _arrPae.Count; }
        }
        public void AddPae(Pae p)
        {
            _arrPae.Add(p);
        }
        public void RemovePae(object obj)
        {
            _arrPae.Remove(obj);
        }
        public void RemovePaeAt(int index)
        {
            _arrPae.RemoveAt(index);
        }
        public void RemoveAllPae()
        {
            _arrPae.Clear();
        }

        // 패 리스트를 패의 코드 리스트로 변경합니다. 
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this[0].PaeCode);

            if (Count > 1)
            {
                for (int i = 1; i < Count; i++)
                {
                    sb.Append(",");
                    sb.Append(this[i].PaeCode);
                }
            }
            return sb.ToString();
        }
        // 패의 코드 리스트를 패리스트로 변환 합니다. 
        //public static PaeList ConvertPaeList(string codelist)
        //{
        //    PaeList rtnPae = new PaeList();
        //    string[] arrPae = codelist.Split(',');
        //    for (int i = 0; i < arrPae.Length; i++)
        //        rtnPae.AddPae(Pae.ConvertPae(arrPae[i]));

        //    return rtnPae;
        //}
    }
}
