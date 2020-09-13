using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4000Client
{
    class Player
    {
        // 치기위해 받은 패 (첫번째 선택한 패)
        private PaeList _hasPaeList;
        // 먹어서 가져온 패 
        private PaeList _hasGainPaes;
        // 사용자의 아이디
        private string _ID;         // 게임유저의 아이디
        private string _M_LevelCD;  //현재 레렙
        private int _jumsoo;        //현재 점수


        // 보유한 금액
        public int _baseMoney;      // 모유한 게임머니


        public Player(string inID)  // 아이디를 인자로 한 생성자 입니다. 
        {
            _hasPaeList = new PaeList();
            _hasGainPaes = new PaeList();
            _ID = inID;
            _baseMoney = 1000000;
        }
        public PaeList HasPae
        {
            set { _hasPaeList = value; }
            get { return _hasPaeList; }
        }
        public PaeList GainPae
        {
            set { _hasGainPaes = value; }
            get { return _hasGainPaes; }
        }

        public int JumSoo
        {
            set { _jumsoo = value; }
            get { return _jumsoo; }
        }
        public string ID
        {
            get { return _ID; }
        }
        public int StartMoney
        {
            set { _baseMoney = value; }
            get { return _baseMoney; }
        }
    }
}
