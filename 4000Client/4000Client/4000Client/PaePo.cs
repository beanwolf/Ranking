using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4000Client
{
    class PaePo
    {
       
            private int _sno;                    //시리얼 넘버
            private string _Label_Name;         // 판때기에서 자신의 라벨 이름을넣어준다.
            private string _ImgName;            //이미지를 넣어주자
        private int _visible = 0;


        public void Cvisibl()
        {
            this._visible = 1;
        }
        public int visible
        {
            get { return _visible; }
        }
        public PaePo(int sno, string lblname, string imgname)
        {
            _sno = sno;
            _Label_Name = lblname;
            _ImgName = imgname;
            _visible = 0;
        }
        public int sNo
        {
            get { return _sno; }
        }
        public string ImageName
        {
            get { return _ImgName; }
        }
        public string Label_Name
        {
            get { return _Label_Name; }
        }
        

    }
}
