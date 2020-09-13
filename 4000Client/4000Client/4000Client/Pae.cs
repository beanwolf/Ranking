using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4000Client
{
    public enum PaeType
    {
        /* 백(뒷면), 만(만수패), 통(통수패), 삭패, 풍패, 원패 */
        pun, man, ton, sak, bak, won
    }

    public class Pae
    {
        private int _sNo;
        private int _paeNo;
        private PaeType _paeValue;
        private string _imgName;
        private string _bigImgName;
        private string _paeCode;
        private string _ResouceImageName;       //현재 사용불가.

        //public  string _labelName;      //Label Name을 넣어준다.

        

        public Pae(int sno, int inNo, PaeType inValue)
        {
            _sNo = sno;
            _paeNo = inNo;
            _paeValue = inValue;
            string str =   inNo.ToString();
//            string str = inNo < 10 ? "0" + inNo.ToString() : inNo.ToString();
            _imgName = @"images\Pae\" + inValue.ToString() + str + "50.png";        // 50x 사이즈
            _bigImgName = @"images\Pae\" + inValue.ToString() + str + "60.png";     // 60x 사이즈
            _paeCode = str + (int)inValue;
            _ResouceImageName = inValue.ToString() + str + "50";
            //_Label_Name = lableName;                    //라벨네임넣기
        }
        // Equals 를 패에 맞게끔 오버라이드 하여 사용합니다. 
        public override bool Equals(object obj)
        {
            if (obj != null && this._paeNo == ((Pae)obj).PaeNo && this._paeValue == ((Pae)obj).PaeValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public string ResouceImageName
        {
            get { return _ResouceImageName; }
        }
        public int sNo
        {
            get { return _sNo; }
        }
        public string ImageName
        {
            get { return _imgName; }
        }
        public int PaeNo
        {
            get { return _paeNo; }
        }
        public PaeType PaeValue
        {
            get { return _paeValue; }
        }
       
        public string BigImageName
        {
            get { return _bigImgName; }
        }

        public string PaeCode
        {
            get { return _paeCode; }
        }

        //??
        //public static Pae ConvertPae(string paecode)
        //{
        //    if (paecode == null || (paecode != null && paecode.Trim().Length < 3))
        //        return null;

        //    int paeno = Int32.Parse(paecode.Substring(0, 2));
        //    return new Pae(paeno, GetPaeType(paecode.Substring(2, 1)));
        //}
        public static PaeType GetPaeType(string code)
        {
            PaeType rtnPaeType = PaeType.bak;
            switch (code)
            {
                case "0":
                    rtnPaeType = PaeType.bak;
                    break;

                case "1":
                    rtnPaeType = PaeType.man;
                    break;

                case "2":
                    rtnPaeType = PaeType.ton;
                    break;

                case "3":
                    rtnPaeType = PaeType.sak;
                    break;

                case "4":
                    rtnPaeType = PaeType.pun;
                    break;

                case "5":
                    rtnPaeType = PaeType.won;
                    break;
            }// end switch

            return rtnPaeType;
        }
    }
}
