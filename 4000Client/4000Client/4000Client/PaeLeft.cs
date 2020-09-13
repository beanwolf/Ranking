using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4000Client
{

    //=========================================================================================
    //
    //      남은 패 배열.. 패없앨때마다. 어레이를 삭제해서 나중에 게임오버를 찾기위해 쓴다.
    //
    //=========================================================================================

    class PaeLeft
    {
        private int _sno;                    //시리얼 넘버
        private string _Label_Name;         // 판때기에서 자신의 라벨 이름을넣어준다.
        private string _ImgName;            //이미지를 넣어주자

        public PaeLeft(int sno, string lblname, string imgname)
        {
            _sno = sno;
            _Label_Name = lblname;
            _ImgName = imgname;
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
