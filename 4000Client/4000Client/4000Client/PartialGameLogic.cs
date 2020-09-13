using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace _4000Client
{
    public partial class  Ranking4000Sung
    {
        //====================================================================
        //  움직일수 있는 패인지 체크 하는 로직    CheckChoicePassblePaeHEXA
        //====================================================================
        private bool CheckChoicePassblePaeHEXA(Pae thisPae, Label thisLabelPae)     // //움직일수 있는 패인지 체크한다.
        {
            bool rtnbool = false;
            string thisLabeName = thisLabelPae.Name;
            //DebugWrite("myPae_Click(lv:"+ _NowPlayGameLevel + ")>  :" + thisLabeName + "\r\n");
            int thisP = thisPae.sNo;

            string rePlString = "lblGamePae";
            if (_NowPlayGameLevel == 2) rePlString = "lblGamePa2";
            else if (_NowPlayGameLevel == 3) rePlString = "lblGamePa3";
            else if (_NowPlayGameLevel == 4) rePlString = "lblGamePa4";
            else if (_NowPlayGameLevel == 5) rePlString = "lblGamePa5";

            string thisPaePocode = thisLabeName;
            int thisPaePoCodeINTX = int.Parse(thisPaePocode.Replace(rePlString, "").Substring(2, 2));      //라벨에서 위치코드만 추출해 INT로 저장
            int thisPaePoCodeINTY = int.Parse(thisPaePocode.Replace(rePlString, "").Substring(0, 2));      //라벨에서 위치코드만 추출해 INT로 저장

            string Left_thisLabeName = "";
            string Right_thisLabelName = "";
            int nowlblNum = int.Parse(thisLabeName.Substring(12, 2).ToString());            //열단위
            int nowlblNum100 = int.Parse(thisLabeName.Substring(11, 1).ToString());        //줄단위
            int left_lblNum = nowlblNum - 1;
            int right_lblNum = nowlblNum + 1;

            if (left_lblNum < 10) Left_thisLabeName = thisLabeName.Substring(0, 12) + "0" + left_lblNum.ToString();
            else Left_thisLabeName = thisLabeName.Substring(0, 12) + left_lblNum.ToString();

            if (right_lblNum < 10) Right_thisLabelName = thisLabeName.Substring(0, 12) + "0" + right_lblNum.ToString();
            else Right_thisLabelName = thisLabeName.Substring(0, 12) + right_lblNum.ToString();

            //_arr_Clear_PaeList : 없앤 패 리스트

            //게임패 배치스타일 분기
            switch (_PaeCenterPanArrangePositionStyle)
            {
                case 1: //벌집형일경우
                    string Left_thisLabeName2 = "자신 왼쪽2";
                    string Right_thisLabelName2 = "자신 오른쪽2";

                    if ((nowlblNum100 == 1) && (nowlblNum % 2 == 1))  //홀수이면서 1번이면 -1은 없다.(첫줄의 홀수다)
                    {
                        DebugWrite("1번라인의 홀수):" + thisLabeName + " > " + Right_thisLabelName + "\r\n");
                        if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0) || (_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0)) rtnbool = true;
                        else
                        {
                            // 없앤 패리스트엔 없지만 판때기에 없을수 있으므로 검사한다.
                            //nowlblNum = 내패넘버 2자리
                            if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1) || (_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1))
                            {
                                rtnbool = true;
                            }
                        }
                    }
                    else
                    {
                        // 조건 (CL1 || ML1) && (CL2 || ML3) == true
                        bool CL1 = false;
                        bool CL2 = false;
                        bool ML1 = false;
                        bool ML2 = false;
                        bool CR1 = false;
                        bool CR2 = false;
                        bool MR1 = false;
                        bool MR2 = false;

                        //홀수일경우 left_lblNum =  nowlblNum - 1; 했지만 짝수일 경우는 반대다.
                        //앞자리 0 처리
                        int leftMinerNum = 1;   //내패에서 빼서 찾을 100단위 넘버 //홀수는 나자신보다 -1, 짝수면 +1
                        if (nowlblNum % 2 != 1) leftMinerNum = -1;

                        if (left_lblNum < 10) Left_thisLabeName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + left_lblNum.ToString();
                        else Left_thisLabeName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + left_lblNum.ToString();
                        //앞자리 0 처리
                        if (right_lblNum < 10) Right_thisLabelName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + right_lblNum.ToString();
                        else Right_thisLabelName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + right_lblNum.ToString();

                        if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0)) CL1 = true;
                        if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName2) > 0)) CL2 = true;
                        if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0)) CR1 = true;
                        if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName2) > 0)) CR2 = true;

                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1)) ML1 = true;
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName2) < 1)) ML2 = true;
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1)) MR1 = true;
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName2) < 1)) MR2 = true;

                        DebugWrite("패정보 : ((" + CL1 + " || " + ML1 + ") && (" + CL2 + " || " + ML2 + "))");
                        DebugWrite("패정보 : ((" + CR1 + " || " + MR1 + ") && (" + CR2 + " || " + MR2 + "))");
                        if ((CL1 || ML1) && (CL2 || ML2)) rtnbool = true;
                        else if ((CR1 || MR1) && (CR2 || MR2)) rtnbool = true;
                    }
                    break;
                default:    //바둑판 게임 방식 일경우
                    if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0) || (_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0))
                    {
                        rtnbool = true;
                    }
                    else
                    {
                        // 없앤 패리스트엔 없지만 판때기에 없을수 있으므로 검사한다.
                        //nowlblNum = 내패넘버 2자리
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1) || (_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1))
                        {
                            //만들패드 모든 리스트에서 오른쪽패가 없다. 고로 오른쪽이 비었다.
                            DebugWrite("만든패 모든리스트에서 왼쪽 또는 오른쪽패가 없다.. :" + thisLabeName + " > " + Right_thisLabelName + "\r\n");
                            rtnbool = true;
                        }
                    }
                    break;      //패 배치스타일 바둑판스타일 끝
            }
            return rtnbool;
        }

        
        //=======================================================
        //패선택 처리해준다.    SelectBoardPae
        //패를 선택했을대 이미지를 좀 크게 보여준다.
        //=======================================================
        private void SelectBoardPae(string thisLabeName, Pae pae, Label paeLabel, int paeindex)
        {
            //======================================================
            //  패를 클릭했을때 , 두번째. 등등.
            //======================================================
            DebugWrite(">SelectBoardPae 패선택] " + _firstChoicePaeName + "==" + thisLabeName + "\r\n");


            if (_firstChoicePaeName == thisLabeName)  //첫번째 선택패랑 지금 선택한 패랑 같다면(처음이면 없겠지)
            {
                _firstChoicePaeName = "";
                paeLabel.Size = new Size(50, 68);   //선택한 패의 사이즈 키우기 50 -> 60, 86->96
                Point beforLocation = new Point(paeLabel.Location.X + 3, paeLabel.Location.Y + 3);
                paeLabel.Location = beforLocation;        //살짝 위로 좌로 보내낸거 다시 원위치 정렬해주자

                paeLabel.BringToFront();            //aosdkvdmfh rkwudhsek.
                CmdSoundPlay2(_GSound_SenchoicePae);    //효과음 주자
                                                        //paepoindex = _arrPaePosition[paeindex].sNo;
                DebugWrite(">다시선택해서  선택 해제] thisLabeName:" + thisLabeName + ", paeLabel.name" + paeLabel.Name + ", _firstChoicePaeName:" + _firstChoicePaeName);
                _fristChoiceLabl = null;            //라벨선택한걸 할당해제한다.
                _fristChoiceINDEXsNo = 999;
                //_firstChoicePaePo = paepo;
                paeLabel.Refresh();     //다시그려준다.

            }
            else
            {
                if (_firstChoicePaeName == "")  //첫선택이다.
                {
                    //GameSound1("KbdKeyTap.wav");    //효과음 주자
                    CmdSoundPlay2(_GSound_SenchoicePae);    //효과음 주자

                    _firstChoicePaeName = thisLabeName;
                    _fristChoiceLabl = paeLabel;
                    _fristChoiceINDEXsNo = paeindex;
                    //_firstChoicePaePo = _arrPaePosition[paeindex];
                    paeLabel.Size = new Size(56, 74);   //선택한 패의 사이즈 키우기 50 -> 60, 86->96
                    Point beforLocation = new Point(paeLabel.Location.X - 3, paeLabel.Location.Y - 3);
                    paeLabel.Location = beforLocation;        //살짝 위로 좌로 보내서 정렬해주자
                    paeLabel.BringToFront();        //맨앞으로 보내주자
                    paeLabel.Refresh();     //다시그려준다.
                    //DebugWrite(">첫번째 선택 : 비교중 = "+ thisLabeName + " | "+ paeLabel.Name + " | " + paeindex);
                }
                else
                {
                    //패를 2개째 선택이다.
                    // 첫번째 선택했던 패와 지금 패를 비교해서 처리한다.

                    //1. 첫번째 패와 두번째선택패가 같은 그림인지 비교
                    //2. 첫번재패와 두번째패를 화면에서 지워준다.
                    //3. 패어레이에서 맞춘(두패 다) 거로 표시해준다.
                    //4. 점수를 올려준다.
                    DebugWrite("두개째선택 : 1(" + _firstChoicePaeName + ") = 2(" + thisLabeName + ")\r\n");
                    //string fimgstring = _fristChoiceLabl.Name;
                    string fimgstring = _arrPaePosition[_fristChoiceINDEXsNo].ImageName;
                    string thisimgstring = _arrPaePosition[paeindex].ImageName;

                    if (fimgstring == thisimgstring)
                    {

                        //DebugWrite("> 두번째패 선택완료 : 이미지가 같다." + _arrPaePosition[_fristChoiceINDEXsNo] + "  [=]  " + thisimgstring);


                        _fristChoiceLabl.Location = new Point(_fristChoiceLabl.Location.X + 3, _fristChoiceLabl.Location.Y + 3);                //원래 위치로 돌려주자
                        _fristChoiceLabl.Visible = false;       //같이니까 없애자
                        paeLabel.Visible = false;


                        //1. PaePosition에서 패를 없앤거로 표시해주거나 삭제하자
                        //2. 점수를 주자
                        //3. 성공 사운드를 넣어주자
                        //4. 없앤 패 어레이에 추가시키자...

                        //_NowPlayGameLevel = 2;
                        //_NowPlayGameGrade = 1;
                        //시간 계산해서 점수 추가해주자.
                        TimeSpan NextRemoveTime = DateTime.Now - _LastRemovePaeStopWatch;
                        if (NextRemoveTime.TotalSeconds < 2)
                        {
                            _Game_RemovePoint_NOW = _Game_RemovePoint_NOW + (3 * _NowPlayGameLevel * _NowPlayGameGrade);
                            _ClearPaeSungCode = _ClearPaeSungCode + 1;
                        }
                        else if (NextRemoveTime.TotalSeconds < 3)
                        {
                            _Game_RemovePoint_NOW = _Game_RemovePoint_NOW + (2 * _NowPlayGameLevel * _NowPlayGameGrade);
                            _ClearPaeSungCode = _ClearPaeSungCode + 1;
                        }
                        else if (NextRemoveTime.TotalSeconds < 4)
                        {
                            _Game_RemovePoint_NOW = _Game_RemovePoint_NOW + (1 * _NowPlayGameLevel * _NowPlayGameGrade);
                            _ClearPaeSungCode = _ClearPaeSungCode + 1;
                        }
                        else
                        {
                            _Game_RemovePoint_NOW = 10;
                            _ClearPaeSungCode = 1;
                        }

                        DebugWrite(">점수 : " + _myGameScore + " + " + _Game_RemovePoint_NOW + " = " + NextRemoveTime.TotalSeconds.ToString("#,##0") + "초");
                        _myGameScore = _myGameScore + _Game_RemovePoint_NOW;
                        _LastRemovePaeStopWatch = DateTime.Now;     //패를 없애 시간을 넣어주자

                        lblGameNowPoint.Text = _myGameScore.ToString("#,##0");
                        //_firstChoicePaePo.Cvisibl();
                        _arr_Clear_PaeList = _arr_Clear_PaeList + "|" + thisLabeName + "|" + _firstChoicePaeName;
                        //DebugWrite("현재없앨 패리스트: thisLabeName[" + thisLabeName + "]  _firstChoicePaeName[" + _firstChoicePaeName + "]\r\n");
                        //DebugWrite("현재없앤패리스트: " + _arr_Clear_PaeList + "\r\n");
                        // 아래코드는 어레이에서 인덱스를 찾아 삭제하면 인덱스가 밀리니 사용 불가능함
                        //_arrWholePae.RemovePaeAt(paeindex);               // 패포지션 어레이에서 패를 삭제한다.
                        //_arrWholePae.RemovePaeAt(_fristChoiceINDEXsNo);   // 패포지션 어레이에서 패를 삭제한다.
                        //DebugWrite("없앨 패 인덱스1 : " + paeindex + " \r\n");
                        //DebugWrite("없앨 패 인덱스2 : " + _fristChoiceINDEXsNo + " \r\n");
                        _GAme_LeftPae_Count = _GAme_LeftPae_Count - 2;
                        //DebugWrite("남은패 갯수 : " + _GAme_LeftPae_Count + " 개\r\n");

                        //paepostiton 에서 paeList의 인덱스 값으로 sno검색해서 인덱스 찾아 삭제하자
                        for (int i = 0; i < _arrPaeLeft.Count; i++)
                        {
                            if (_arrPaeLeft[i].sNo == paeindex)
                            {
                                DebugWrite("1:없앤다:" + thisLabeName + "(" + thisimgstring + ")\r\n");
                                _arrPaeLeft.RemovePaeLeftAt(i);
                                break;
                            }
                        }
                        for (int i = 0; i < _arrPaeLeft.Count; i++)
                        {
                            if (_arrPaeLeft[i].sNo == _fristChoiceINDEXsNo)
                            {
                                DebugWrite("2:없앤다:" + _firstChoicePaeName + "(" + fimgstring + ") \r\n");
                                _arrPaeLeft.RemovePaeLeftAt(i);
                                break;
                            }
                        }
                        //DebugWrite("남은 어레이 갯수 : " + _arrPaeLeft.Count + " 개\r\n");
                        //DebugWrite("없앤패릿트 : " + _arr_Clear_PaeList + " \r\n");

                        _fristChoiceLabl = null;            //없앴으니 첫번째  라벨선택한걸 할당해제한다.
                        _fristChoiceINDEXsNo = 999;         //없앴으니 첫번째 인덱스 날리자.
                        _firstChoicePaeName = "";           //없앴으니 첫번째 선택패 없애 주자.

                        if (_ClearPaeSungCode > 8) _ClearPaeSungCode = 8;
                        CmdSoundPlay3(_ClearPaeSungCode);    //효과음 주자

                        lbl_PaePosition.Text = _NowPlayGame_ResultDataString;// 최고점수& 유저를 를 계속 표시해주게 된다.

                        if (_GAme_LeftPae_Count <= GameEndLeftPae)     //패남은갯수가 0이면 게임완료
                        {
                            CmdGameCLEAR(_NowPlayGameLevel, _GameStartTime, _myGameScore, _NowPlayGameGrade);    //게임클리어로 보내자
                        }
                        else if (!LeftPaeMoveCheck())             //더이상 게임에 제거할수 있는패가 없는지 검사해서 게임을 종료시킨다.(실패)
                        {
                            //DebugWrite(">>  더 없앨수 있는 패가 없다!!! .\r\n");
                            DeleGameOver("1", "더 없앨수 있는 패가 없습니다. !");
                        }
                    }
                    else
                    {
                        DebugWrite("> 같은 패가 아닙니다." + fimgstring + "  [=]  " + thisimgstring);
                        //_fristChoiceLabl.Size = new Size(50, 68);   // 처음 선택한 패의 사이즈 원래 대로
                        // 실패 사운드 넣어주자
                        CmdSoundPlay2(_GSound_FailClickPae);    //효과음 주자
                    }
                }
            }
        }

        //=======================================================
        //  옮길수 있는 패가 남아있느지 확인한다.(첫번째 패)    LeftPaeMoveCheck
        //=======================================================
        private bool LeftPaeMoveCheck()
        {
            //패갯수를 수정했으므로 작은 사이즈에서 없앨수 잇는 패의 갯수를 계산다시 하자
            for (int i = 0; i < _arrPaeLeft.Count; i++)
            {
                string rePlString = "lblGamePae";
                if (_NowPlayGameLevel == 2) rePlString = "lblGamePa2";
                else if (_NowPlayGameLevel == 3) rePlString = "lblGamePa3";
                else if (_NowPlayGameLevel == 4) rePlString = "lblGamePa4";
                else if (_NowPlayGameLevel == 5) rePlString = "lblGamePa5";

                int thisPaePoCodeINTX = int.Parse(_arrPaeLeft[i].Label_Name.Replace(rePlString, "").Substring(2, 2));      //라벨에서 위치코드만 추출해 INT로 저장
                int thisPaePoCodeINTY = int.Parse(_arrPaeLeft[i].Label_Name.Replace(rePlString, "").Substring(0, 2));      //라벨에서 

                string Left_thisLabeName = "";
                string Right_thisLabelName = "";
                string Left_thisLabeName2 = "자신 왼쪽2";
                string Right_thisLabelName2 = "자신 오른쪽2";


                int nowlblNum = int.Parse(_arrPaeLeft[i].Label_Name.Substring(12, 2).ToString());           //칸 번호
                int nowlblNum100 = int.Parse(_arrPaeLeft[i].Label_Name.Substring(11, 1).ToString());        //줄 번호 단위

                int left_lblNum = nowlblNum - 1;
                int right_lblNum = nowlblNum + 1;

                if (left_lblNum < 10) Left_thisLabeName = _arrPaeLeft[i].Label_Name.Substring(0, 12) + "0" + left_lblNum.ToString();
                else Left_thisLabeName = _arrPaeLeft[i].Label_Name.Substring(0, 12) + left_lblNum.ToString();

                if (right_lblNum < 10) Right_thisLabelName = _arrPaeLeft[i].Label_Name.Substring(0, 12) + "0" + right_lblNum.ToString();
                else Right_thisLabelName = _arrPaeLeft[i].Label_Name.Substring(0, 12) + right_lblNum.ToString();

                if (_PaeCenterPanArrangePositionStyle == 1) //헥사타잎 타잎 게임이면
                {
                    bool CL1 = false;
                    bool CL2 = false;
                    bool ML1 = false;
                    bool ML2 = false;
                    bool CR1 = false;
                    bool CR2 = false;
                    bool MR1 = false;
                    bool MR2 = false;

                    //홀수일경우 left_lblNum =  nowlblNum - 1; 했지만 짝수일 경우는 반대다.
                    //앞자리 0 처리
                    int leftMinerNum = 1;   //내패에서 빼서 찾을 100단위 넘버 //홀수는 나자신보다 -1, 짝수면 +1
                    if (nowlblNum % 2 != 1) leftMinerNum = -1;

                    if (left_lblNum < 10) Left_thisLabeName2 = _arrPaeLeft[i].Label_Name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + left_lblNum.ToString();
                    else Left_thisLabeName2 = _arrPaeLeft[i].Label_Name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + left_lblNum.ToString();
                    //앞자리 0 처리
                    if (right_lblNum < 10) Right_thisLabelName2 = _arrPaeLeft[i].Label_Name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + right_lblNum.ToString();
                    else Right_thisLabelName2 = _arrPaeLeft[i].Label_Name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + right_lblNum.ToString();

                    if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0)) CL1 = true;
                    if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName2) > 0)) CL2 = true;
                    if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0)) CR1 = true;
                    if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName2) > 0)) CR2 = true;

                    if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1)) ML1 = true;
                    if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName2) < 1)) ML2 = true;
                    if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1)) MR1 = true;
                    if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName2) < 1)) MR2 = true;

                    if ((CL1 || ML1) && (CL2 || ML2))
                    {

                        if (LeftPaeMatchCheck(_arrPaeLeft[i].ImageName, i))
                        {
                            lbl_4_FirstChoicePaeDisplay.Image = Image.FromFile(_arrPaeLeft[i].ImageName);
                            lbl_firstchoecePaename.Text = _arrPaeLeft[i].ImageName.Replace("images\\Pae\\", "") + "\r\n" + _arrPaeLeft[i].Label_Name.Replace("lblGamePa4", "");
                            return true;
                        }
                    }
                    else if ((CR1 || MR1) && (CR2 || MR2))
                    {
                        if (LeftPaeMatchCheck(_arrPaeLeft[i].ImageName, i))
                        {
                            lbl_4_FirstChoicePaeDisplay.Image = Image.FromFile(_arrPaeLeft[i].ImageName);
                            lbl_firstchoecePaename.Text = _arrPaeLeft[i].ImageName.Replace("images\\Pae\\", "") + "\r\n" + _arrPaeLeft[i].Label_Name.Replace("lblGamePa4", "");
                            return true;
                        }
                    }
                }
                else //(_PaeCenterPanArrangePositionStyle == 0) 바둑판이 게임이면
                {
                    if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0) || (_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0))
                    {
                        if (LeftPaeMatchCheck(_arrPaeLeft[i].ImageName, i)) return true;
                    }
                    else
                    {
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1) || (_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1))
                        {
                            //만들패드 모든 리스트에서 오른쪽패가 없다. 고로 오른쪽이 비었다.
                            if (LeftPaeMatchCheck(_arrPaeLeft[i].ImageName, i)) return true;
                        }
                    }
                }
            }
            return false;
        }
        //움직일수있는 패검사 두번째 선택패 확인( 다음꺼)  확인

        private bool LeftPaeMatchCheck(string imgName, int myIndex)
        {
            //myindex = 내인덱스 번호(이인덱스는 비교대상에서 제외)
            // imgName = 내 이미지
            for (int i = 0; i < _arrPaeLeft.Count; i++)
            {
                if (i != myIndex)
                {
                    //내인덱스가 아닌것들중에서
                    if (imgName == _arrPaeLeft[i].ImageName)
                    {
                        //같은 이미지 일경우
                        string rePlString = "lblGamePae";
                        if (_NowPlayGameLevel == 2) rePlString = "lblGamePa2";
                        else if (_NowPlayGameLevel == 3) rePlString = "lblGamePa3";
                        else if (_NowPlayGameLevel == 4) rePlString = "lblGamePa4";
                        else if (_NowPlayGameLevel == 5) rePlString = "lblGamePa5";

                        string NowLabel_name = _arrPaeLeft[i].Label_Name;


                        int thisPaePoCodeINTX = int.Parse(NowLabel_name.Replace(rePlString, "").Substring(2, 2));      //라벨에서 위치코드만 추출해 INT로 저장
                        int thisPaePoCodeINTY = int.Parse(NowLabel_name.Replace(rePlString, "").Substring(0, 2));      //라벨에서 


                        // 이미지가 같은 것중에서 없앨수 있는 위치에 있는지 확인한다.

                        string Left_thisLabeName = "";
                        string Right_thisLabelName = "";
                        int nowlblNum = int.Parse(NowLabel_name.Substring(12, 2).ToString());
                        int nowlblNum100 = int.Parse(NowLabel_name.Substring(11, 1).ToString());        //줄단위

                        int left_lblNum = nowlblNum - 1;
                        int right_lblNum = nowlblNum + 1;

                        if (left_lblNum < 10) Left_thisLabeName = NowLabel_name.Substring(0, 12) + "0" + left_lblNum.ToString();
                        else Left_thisLabeName = NowLabel_name.Substring(0, 12) + left_lblNum.ToString();

                        if (right_lblNum < 10) Right_thisLabelName = NowLabel_name.Substring(0, 12) + "0" + right_lblNum.ToString();
                        else Right_thisLabelName = NowLabel_name.Substring(0, 12) + right_lblNum.ToString();


                        if (_PaeCenterPanArrangePositionStyle == 1) //헥사타잎 타잎 게임이면
                        {
                            string Left_thisLabeName2 = "자신 왼쪽2";
                            string Right_thisLabelName2 = "자신 오른쪽2";

                            bool CL1 = false;
                            bool CL2 = false;
                            bool ML1 = false;
                            bool ML2 = false;
                            bool CR1 = false;
                            bool CR2 = false;
                            bool MR1 = false;
                            bool MR2 = false;

                            //홀수일경우 left_lblNum =  nowlblNum - 1; 했지만 짝수일 경우는 반대다.
                            //앞자리 0 처리
                            int leftMinerNum = 1;   //내패에서 빼서 찾을 100단위 넘버 //홀수는 나자신보다 -1, 짝수면 +1
                            if (nowlblNum % 2 != 1) leftMinerNum = -1;      //짝수면?

                            if (left_lblNum < 10) Left_thisLabeName2 = NowLabel_name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + left_lblNum.ToString();
                            else Left_thisLabeName2 = NowLabel_name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + left_lblNum.ToString();
                            //앞자리 0 처리
                            if (right_lblNum < 10) Right_thisLabelName2 = NowLabel_name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + right_lblNum.ToString();
                            else Right_thisLabelName2 = NowLabel_name.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + right_lblNum.ToString();

                            if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0)) CL1 = true;
                            if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName2) > 0)) CL2 = true;
                            if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0)) CR1 = true;
                            if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName2) > 0)) CR2 = true;

                            if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1)) ML1 = true;
                            if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName2) < 1)) ML2 = true;
                            if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1)) MR1 = true;
                            if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName2) < 1)) MR2 = true;
                            if ((CL1 || ML1) && (CL2 || ML2))
                            {
                                DebugWrite(_arrPaeLeft[i].ImageName + "두번째 매칭  왼쪽빔 찾음");
                                lbl_4_NextChoicePaeDisplay.Image = Image.FromFile(_arrPaeLeft[i].ImageName);
                                lbl_NextchoecePaename.Text = _arrPaeLeft[i].ImageName + "\r\n" + _arrPaeLeft[i].Label_Name;


                                return true;
                            }
                            else if ((CR1 || MR1) && (CR2 || MR2))
                            {
                                DebugWrite(_arrPaeLeft[i].ImageName + "두번째 매칭  오른쪽빔 찾음");
                                lbl_4_NextChoicePaeDisplay.Image = Image.FromFile(_arrPaeLeft[i].ImageName);
                                lbl_NextchoecePaename.Text = _arrPaeLeft[i].ImageName + "\r\n" + _arrPaeLeft[i].Label_Name;


                                return true;
                            }
                        }
                        else
                        {
                            // 바둑판형일경우
                            if (_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0) return true;
                            else if (_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0) return true;
                            else if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1) || (_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1)) return true;
                        }

                    }
                }
            }
            return false;
        }


        //게임 실패
        //==========================================================================================
        //게임 보드에 깔린 패를 선택했을경우   myPae_Click
        //==========================================================================================
        private void myPae_Click(object sender, EventArgs e)
        {
            // 패를 치기 위해 선택한 라벨로 부터 패정보를 가져옮니다. 
            Pae thisPae = ((Pae)((Label)sender).Tag);
            //PaePo thisPaePo = ((PaePo)((Label)sender).Tag);
            Label thisLabelPae = ((Label)sender);
            string thisLabeName = thisLabelPae.Name;
            //DebugWrite("myPae_Click(lv:"+ _NowPlayGameLevel + ")>  :" + thisLabeName + "\r\n");
            int thisP = thisPae.sNo;

            string rePlString = "lblGamePae";
            if (_NowPlayGameLevel == 2) rePlString = "lblGamePa2";
            else if (_NowPlayGameLevel == 3) rePlString = "lblGamePa3";
            else if (_NowPlayGameLevel == 4) rePlString = "lblGamePa4";
            else if (_NowPlayGameLevel == 5) rePlString = "lblGamePa5";

            string thisPaePocode = thisLabeName;
            int thisPaePoCodeINTX = int.Parse(thisPaePocode.Replace(rePlString, "").Substring(2, 2));      //라벨에서 위치코드만 추출해 INT로 저장
            int thisPaePoCodeINTY = int.Parse(thisPaePocode.Replace(rePlString, "").Substring(0, 2));      //라벨에서 위치코드만 추출해 INT로 저장
                                                                                                           //DebugWrite(">패선택 : "+ thisLabeName + " | "+ thisPaePoCodeINTX );

            DebugWrite("(Lv." + _NowPlayGameLevel + ") " + _PaeCenterPanArrangePositionStyle + " 라벨이름검사 :" + thisLabeName + "\r\n");

            // lblGamePae0614
            // 01234567890123
            // 123456789012
            string Left_thisLabeName = "";
            string Right_thisLabelName = "";
            int nowlblNum = int.Parse(thisLabeName.Substring(12, 2).ToString());            //열단위
            int nowlblNum100 = int.Parse(thisLabeName.Substring(11, 1).ToString());        //줄단위
            int left_lblNum = nowlblNum - 1;
            int right_lblNum = nowlblNum + 1;

            if (left_lblNum < 10) Left_thisLabeName = thisLabeName.Substring(0, 12) + "0" + left_lblNum.ToString();
            else Left_thisLabeName = thisLabeName.Substring(0, 12) + left_lblNum.ToString();

            if (right_lblNum < 10) Right_thisLabelName = thisLabeName.Substring(0, 12) + "0" + right_lblNum.ToString();
            else Right_thisLabelName = thisLabeName.Substring(0, 12) + right_lblNum.ToString();

            //_arr_Clear_PaeList : 없앤 패 리스트

            //게임패 배치스타일 분기
            switch (_PaeCenterPanArrangePositionStyle)
            {
                case 1: //벌집형일경우
                        //뒤에 2자리가 홀수일경우 위쪽이다. 고로 (좌는=  (자신의번호에서 같은10단위 에서 1단위 -1 && ,-10단위 1단위-1) 우는 x,1)
                        //뒤에 2자리가 짝수일 경우 (좌는 자신번호
                    string Left_thisLabeName2 = "자신 왼쪽2";
                    string Right_thisLabelName2 = "자신 오른쪽2";

                    if ((nowlblNum100 == 1) && (nowlblNum % 2 == 1))  //홀수이면서 1번이면 -1은 없다.(첫줄의 홀수다)
                    {
                        DebugWrite("1번라인의 홀수):" + thisLabeName + " > " + Right_thisLabelName + "\r\n");
                        if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0) || (_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0))
                        {
                            SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP);
                        }
                        else
                        {
                            // 없앤 패리스트엔 없지만 판때기에 없을수 있으므로 검사한다.
                            //nowlblNum = 내패넘버 2자리
                            if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1) || (_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1))
                            {
                                //만들패드 모든 리스트에서 오른쪽패가 없다. 고로 오른쪽이 비었다.
                                //DebugWrite("만든패 모든리스트에서 왼쪽 또는 오른쪽패가 없다.. :" + thisLabeName + " > " + Right_thisLabelName + "\r\n");
                                SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP);
                            }
                        }
                    }
                    else
                    {
                        // 조건 (CL1 || ML1) && (CL2 || ML3) == true
                        bool CL1 = false;
                        bool CL2 = false;
                        bool ML1 = false;
                        bool ML2 = false;
                        bool CR1 = false;
                        bool CR2 = false;
                        bool MR1 = false;
                        bool MR2 = false;

                        //홀수일경우 left_lblNum =  nowlblNum - 1; 했지만 짝수일 경우는 반대다.
                        //앞자리 0 처리
                        int leftMinerNum = 1;   //내패에서 빼서 찾을 100단위 넘버 //홀수는 나자신보다 -1, 짝수면 +1
                        if (nowlblNum % 2 != 1) leftMinerNum = -1;

                        if (left_lblNum < 10) Left_thisLabeName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + left_lblNum.ToString();
                        else Left_thisLabeName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + left_lblNum.ToString();
                        //앞자리 0 처리
                        if (right_lblNum < 10) Right_thisLabelName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + "0" + right_lblNum.ToString();
                        else Right_thisLabelName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - leftMinerNum) + right_lblNum.ToString();

                        //조건 조합 시작 
                        //조건 C = 클리어패 리스트
                        //조건 L1 왼쪽1, L2왼쪽2
                        //조건 (CL1 || ML1) && (CL2 || ML2) == true

                        if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0)) CL1 = true;
                        if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName2) > 0)) CL2 = true;
                        if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0)) CR1 = true;
                        if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName2) > 0)) CR2 = true;

                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1)) ML1 = true;
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName2) < 1)) ML2 = true;
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1)) MR1 = true;
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName2) < 1)) MR2 = true;

                        // TRUE 조건들 (CL,CR, ML, ML)
                        // 조건 1 : 좌측 둘다 없앤패에 있다.
                        // 조건 2 : 우측 둘다 없앤패에 있따.
                        // 조건 1 : 좌측 둘다 만든패에 없다.
                        // 조건 1 : 우측 둘다 만든패에 없다.
                        // 조건 2 : 홀수이면서1 이고 없앤패에 있다.        //완
                        // 조건 2 : 홀수이면서1 이고 만든패에 없다.        //완
                        // 조건 :  (L1 true && L2 true) = true
                        // 조건 :  (R1 true && R2 true) = true
                        //조건 ===========
                        // 조건 (CL1 || ML1) && (CL2 || ML3) == true

                        DebugWrite("패정보 : ((" + CL1 + " || " + ML1 + ") && (" + CL2 + " || " + ML2 + "))");
                        DebugWrite("패정보 : ((" + CR1 + " || " + MR1 + ") && (" + CR2 + " || " + MR2 + "))");
                        if ((CL1 || ML1) && (CL2 || ML2)) SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP);
                        else if ((CR1 || MR1) && (CR2 || MR2)) SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP); ;
                    }
                    //else//짝수면
                    //{
                    //    //앞자리 0 처리
                    //    if (left_lblNum < 10) Left_thisLabeName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - 1) + "0" + left_lblNum.ToString();
                    //    else Left_thisLabeName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - 1) + left_lblNum.ToString();
                    //    //앞자리 0 처리
                    //    if (right_lblNum < 10) Right_thisLabelName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - 1) + "0" + right_lblNum.ToString();
                    //    else Right_thisLabelName2 = thisLabeName.Substring(0, 11) + (nowlblNum100 - 1) + right_lblNum.ToString();


                    //    if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0) && (_arr_Clear_PaeList.IndexOf(Left_thisLabeName2) > 0))
                    //    {
                    //        DebugWrite("좌측 둘다 업앤패중에 있다 고로 선택가능한패다..\r\n");
                    //        SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP);
                    //    }
                    //    else if ((_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0) && (_arr_Clear_PaeList.IndexOf(Right_thisLabelName2) > 0))
                    //    {
                    //        DebugWrite("우측 둘다 업앤패중에 있다 고로 선택가능한패다..\r\n");
                    //        SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP);
                    //    }
                    //}
                    break;
                default:    //바둑판 게임 방식 일경우
                    if ((_arr_Clear_PaeList.IndexOf(Left_thisLabeName) > 0) || (_arr_Clear_PaeList.IndexOf(Right_thisLabelName) > 0))
                    {
                        //DebugWrite("라벨 오른쪽 또는 왼쪽 없앤거 있다. :" + thisLabeName + " > " + Right_thisLabelName + "\r\n");
                        SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP);
                    }
                    else
                    {
                        // 없앤 패리스트엔 없지만 판때기에 없을수 있으므로 검사한다.
                        //nowlblNum = 내패넘버 2자리
                        if ((_arr_Clear_PaeList_MakePaeList.IndexOf(Left_thisLabeName) < 1) || (_arr_Clear_PaeList_MakePaeList.IndexOf(Right_thisLabelName) < 1))
                        {
                            //만들패드 모든 리스트에서 오른쪽패가 없다. 고로 오른쪽이 비었다.
                            DebugWrite("만든패 모든리스트에서 왼쪽 또는 오른쪽패가 없다.. :" + thisLabeName + " > " + Right_thisLabelName + "\r\n");
                            SelectBoardPae(thisLabeName, thisPae, thisLabelPae, thisP);
                        }
                    }
                    break;      //패 배치스타일 바둑판스타일 끝
            }

            //// 게이머의 정보를 얻기위해 라벨이 속한 패널을 찾아 냅니다. 
            //Panel pn = (Panel)((Label)sender).Parent.Parent; // 패널을 가져오고 
            //// 패널로 부터 게이머의 정보를 얻어 냅니다. 
            //Player per = (Player)pn.Tag;

            //// 칠 순서의 사용자라면 
            //if (_myID == per.ID && per.ID == _nextPlayer.ID)
            //{
            //    // 치기 위해 낸 패를 모든 게이머에게 전송합니다. 
            //    SendMessage("PAESEND:" + thisPae.PaeCode);
            //}
        }
    }
}
