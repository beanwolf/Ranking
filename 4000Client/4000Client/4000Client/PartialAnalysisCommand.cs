using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4000Client
{
    public partial class Ranking4000Sung
    {

        #region 명령행 처리
        // 예약어를 분석하여 명령 처리합니다. 
        public void AnalysisCommand(string Command)
        {
            //DebugWrite("AnalysisCommand = " + Command);

            // 메시지가 없다면 따져나갑니다. 
            if (Command == null || Command == "")
                return;

            // 명령 부분을 분석하기 위해 잘라냅니다. 
            string[] cmds = Command.Split(':');


            switch (cmds[0])        // 명령에 따라 구분합니다. 
            {
                case "LOGINFAIL":
                    //로그인 실패했다.
                    CmdLoginFail(Command);
                    //DebugWrite("로그인실패");
                    break;

                case "M_GAMEDATA":
                    // DebugWrite("데이타받았다:" + Command);
                    break;

                case "LOGINOK":
                    LoginInOKFlag = 1;
                    //DebugWrite("로그인성공" + LoginInOKFlag);

                    CmdLoginOK(Command);        //로그인 성공처리
                    //LoginAfter(1);
                    //Thread.Sleep(100);
                    //CmdLoginAfter(1); // < --딜리게이트로 하면 바로 접속종료되버린다.
                    break;
                // 게임 준비라면 
                case "GAMEREADY":
                    // 내용을 인자로 하여 게임 준비 메소드를 호출합니다. 
                    //CmdGameReady(cmds[1]);
                    break;

                // 서버로 부터 받은 패를 각각의 플레이어에게 할당 합니다. 
                case "PAELIST":
                    // 명령행이이의 인자들과 함께 메소드를 실행합니다. 
                    //CmdPaeList(cmds[1], cmds[2]);
                    break;

                // client : 새로운 게임을 요청합니다.
                // server : 칠 패를 생성해 클라이언트에게 보내줍니다.
                // client : 새로운 게임을 받았을때 패를 받고 세팅한후 패돌리는것을 
                //          실행하도록 한다.
                case "NEWGAME":
                    // 새 게임을 위한 메소드 호출 
                    //CmdNewGame();
                    break;

                // client : 낼 패를 보낸다.
                // server : 낸 패와 깐 패를 보낸다.
                case "PAESEND":
                    // 패를 낸 사용자의 아이디를 받습니다. 
                    //_rcvID = cmds[1];
                    // 패를 냈을때 처리를 하는 메소드 호출
                    //CmdPaeSend(cmds[1], cmds[2]);
                    //DelePaeSend(cmds[1], cmds[2]);
                    break;

                // 패를 선택했을때 전해지는 예약어
                case "SELECTPAE":
                    // 패를 낸 사용자의 아이디를 받습니다. 
                    //_rcvID = cmds[1];
                    // 먹을 패가 두개가 있어 패를 선택했을때 처리  
                    //CmdSelectPae(cmds[1], cmds[2]);
                    break;

                // client : 게임이 종료되었음을 알립니다. 
                // server : 
                //case "GAMEOVER":                            // 게임을 종료되었다는 메시지를 받았을때의 처리 
                //    CmdGameOver(cmds[1], cmds[2]);
                //    break;
                //case "GAMECLEAR":                            // 게임을 종료되었다는 메시지를 받았을때의 처리 
                //    CmdGameCLEAR(cmds[1], cmds[2]);
                //break;
                case "CHAT":                               // 채팅 내용
                    //DebugWrite("< 채팅을 받았다. (:C_R_153:[" + cmds[1] + ":" + cmds[2] + "])");          //디버거다

                    int idsize = 12 - GetSubStringEnHanAllSize(cmds[1]);
                    for (int i = 0; i < idsize; i++) cmds[1] = cmds[1] + " ";
                    string msgview = "[" + cmds[1] + "]:" + cmds[2];
                    //채팅일경우 처리해주자
                    //DebugWrite("글짜크기= 12-x="+ idsize);

                    ChatWrite(msgview);
                    break;

                case "SYSTEM":                               //시스템 내용
                    string sysmsgview = cmds[1] + ":" + cmds[2];
                    ChatWrite("[안내] " + sysmsgview);
                    break;

                case "UserListFirst":
                    //DebugWrite("진입>UserListFirst = SYSTEMJOIN:" + Command + "\r\n");

                    Command = Command.Replace("UserListFirst:", "");
                    string[] cmdL = Command.Split('|');
                    int cmdLSize = cmdL.Count();
                    for (int i = 0; i < cmdLSize; i++)
                    {
                        //DebugWrite("받았음>UserListFirst = SYSTEMJOIN:" + cmdL[i]);
                        cmdJoinServerUser("SYSTEMJOIN:" + cmdL[i]);
                    }

                    break;
                case "LogOutUser":
                    cmdLeaveServerUser(cmds[1]);
                    ChatWrite("[안내] " + cmds[2] + "님이 접속종료 하셨습니다.");
                    break;
                case "SYSTEMJOIN":                               //시스템 내용
                    //DebugWrite("SYSTEMJOIN:"+Command);
                    cmdJoinServerUser(Command);
                    ChatWrite("[안내] " + cmds[2] + "님이 접속 하셨습니다.");
                    //SYSTEMJOIN:Air:에어다~:1
                    break;
                //            BroadcastMsg(">> SYSTEMJOIN:" + nick + ":"+ nicknamd+ ":"+ p_level);     //시스템 메시지로 접속안내

                //"GAMELEVEBESTDATA:"+ GLevel+":"+ gGraed+":" + GraedBRTime.ToString() + ":" + GraedBRTimeMID);
                case "GAMELEVEBESTDATA":
                    _NowPlayGame_ResultDataString = "Lv " + cmds[1] + "-" + cmds[2] + "\r\n 베스트Time : " + cmds[3] + "초 (" + cmds[4] + ")님";
                    CmdDelGameLevelData(Command);
                    ChatWrite("== " + _NowPlayGame_ResultDataString);
                    //if (cmds[1].ToString() == "1") lbl_lv1BestResultInfo.Text = _NowPlayGame_ResultDataString; //이거 이상하게 작동함 서버에서 리피트됨 아마 델리게이트 써야함
                    break;
                default:
                    break;

            }// end switch 
        }// end AnalysisCommand
        #endregion 명령행 처리  // 예약어를 분석하여 명령 처리합니다. 

    }
}
