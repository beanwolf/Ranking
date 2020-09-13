using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4000Client
{
    public partial class Ranking4000Sung
    {
        #region 델리게이트 선언들
        // 화면에 관련된 처리를 할때 쓰일 여러 델리게이트
        public delegate void InitDelegate(int Glevel, int gGread);                // 게임을 시작하기 위한 기본적인 처리를 할 메소드를 위임 처리할 델리게이트  
        public delegate void GameReadyDelegate(string nick);    // 게임을 시작할 수 있도록 준비하는 메소드를 위임 처리할 델리게이트  
        public delegate void PaeSendDelegate(string nick, string pae);  // 패를 냈을 경우 처리해야할 메소드를 위임 처리할 델리게이트 
        public delegate void SelectPaeDelegate(string nick, string pae);    // 먹을 패가 두개 있을때 하나를 선택했을 경우 처리해야할 메소드를 위임 처리할 델리게이트 
        public delegate void GameOverDelegate(string win, string nick); // 게임을 종료했을 경우 처리해야할 메소드를 위임 처리할 델리게이트 
        public delegate void GameClearDelegate(int GLevel, DateTime GameStrat, int gameScore, int gGraed); // 게임을 종료했을 경우 처리해야할 메소드를 위임 처리할 델리게이트 
        public delegate void JoinServerUserDelegate(string cmd);        //유저 참가 처리
        public delegate void LeaveServerUserDelegate(string cmd);       //유저 로그아웃 처리

        #region  추가로 입력한 델리게이트 선언
        public delegate void LogWriteDelegate(string msg);              // 생성한 폼 어플리케이션에 로그를 찍기위해 선언한 delegate 
        //public delegate void GameChatDelegate(string nick, string msg); //  게임내 채팅 처리를 위한 델리게이트
        public delegate void DebugWriteDelegate(string msg);      // 디버그 로그를 표시하기 위한 delegate
        public delegate void GameLogintWaiting();
        public delegate void GameLoginAfter(int sno);
        public delegate void GameLoginOKDelegate(string comd);
        public delegate void GameLevelDataDelegate(string comd);        //게임기록 받는 델리게이트
        public delegate void GameLoginFailDelegate(string cod);     //게임 실패시
        public delegate void GameSendMsg(string msg);               //서버로 전송하기 위한 델리게이트
        public delegate void GameSoundPlay1Delegate(string sname);       //게임 사운드1
        public delegate void GameSoundPlay2Delegate(string sname);       //게임 사운드1
        public delegate void GameSoundPlay3Delegate(string sname);       //게임 사운드1
        #endregion 추가선언 델리 끝
        #endregion 델리게이트 선언끝


        /// <summary>
        ///     델리게이트를 사용한 Invoke 처리 메소들
        /// </summary>
        /// 


        public void DeleGameSound1(string sname) // 게임이 종료되었을 경우 처리할 메소드를 위임 처리하기 위한 메소드 
        {
            // 게임 종료시 처리할 GameOver 메소드를 처리하기 위한 델리게이트 선언 
            GameSoundPlay2Delegate dGameSound1 = new GameSoundPlay2Delegate(GameSound1);
            // Invoke 메소드를 실행 
            this.Invoke(dGameSound1, new object[] { sname });
        }
        public void DeleGameSound2(string sname) // 게임이 종료되었을 경우 처리할 메소드를 위임 처리하기 위한 메소드 
        {
            // 게임 종료시 처리할 GameOver 메소드를 처리하기 위한 델리게이트 선언 
            GameSoundPlay2Delegate dGameSound2 = new GameSoundPlay2Delegate(GameSound2);
            // Invoke 메소드를 실행 
            this.Invoke(dGameSound2, new object[] { sname });
        }
        public void DeleGameSound3(string sname) // 게임이 종료되었을 경우 처리할 메소드를 위임 처리하기 위한 메소드 
        {
            // 게임 종료시 처리할 GameOver 메소드를 처리하기 위한 델리게이트 선언 
            GameSoundPlay3Delegate dGameSound3 = new GameSoundPlay3Delegate(GameSound3);
            // Invoke 메소드를 실행 
            this.Invoke(dGameSound3, new object[] { sname });
        }
        public void DeleJoinServerUser(string cmd)
        {
            JoinServerUserDelegate djoinserveruser = new JoinServerUserDelegate(JoinServerUser);
            this.Invoke(djoinserveruser, new object[] { cmd });

        }
        public void DeleLeaveServerUser(string cmd)
        {
            LeaveServerUserDelegate dLeaveServerUser = new LeaveServerUserDelegate(LeaveServerUser);
            this.Invoke(dLeaveServerUser, new object[] { cmd });
        }
        public void DeleGameClear(int GLevel, DateTime startTime, int gameScore, int gGraed) // 게임이 종료되었을 경우 처리할 메소드를 위임 처리하기 위한 메소드 
        {
            // 게임 종료시 처리할 GameOver 메소드를 처리하기 위한 델리게이트 선언 
            GameClearDelegate dGameClear = new GameClearDelegate(GameClear);
            // Invoke 메소드를 실행 
            this.Invoke(dGameClear, new object[] { GLevel, startTime, gameScore, gGraed });
        }
        public void DeleGameOver(string win, string nick) // 게임이 종료되었을 경우 처리할 메소드를 위임 처리하기 위한 메소드 
        {
            // 게임 종료시 처리할 GameOver 메소드를 처리하기 위한 델리게이트 선언 
            GameOverDelegate dGameOver = new GameOverDelegate(GameOver);
            // Invoke 메소드를 실행 
            this.Invoke(dGameOver, new object[] { win, nick });
        }

        public void DeleGameSoundDX1(string sname) //다이렉트사운드 효과음
        {
            // 게임 종료시 처리할 GameOver 메소드를 처리하기 위한 델리게이트 선언 
            GameSoundPlay2Delegate dGameSoundDX1 = new GameSoundPlay2Delegate(GameSoundDX1);
            // Invoke 메소드를 실행 
            this.Invoke(dGameSoundDX1, new object[] { sname });
        }

        public void DebugWrite(string msg)
        {
            //소켓에 관련된 스레드가 돌고 있으므로 application 과의 충돌을 피하기 위해 델리게이트를 이용합니다.
            DebugWriteDelegate deleDebugWrite = new DebugWriteDelegate(AppendDebug);
            //생성한 델리게이트를 이용하여 invoke 를 실행합니다.
            this.Invoke(deleDebugWrite, new object[] { msg });
        }
        public void DeleLoginFail(string comd)
        {
            GameLoginFailDelegate dLoginFail = new GameLoginFailDelegate(LoginFail);
            this.Invoke(dLoginFail, new object[] { comd });
        }


        public void DeleGameLevelData(string comd)
        {
            GameLevelDataDelegate dGameLevelData = new GameLevelDataDelegate(GamelevelDataWrite);
            this.Invoke(dGameLevelData, new object[] { comd });
        }
        public void DeleGameLoginOK(string comd)
        {
            GameLoginOKDelegate dGameLoginOk = new GameLoginOKDelegate(GameLoginOK);
            this.Invoke(dGameLoginOk, new object[] { comd });
        }
        public void ChatWrite(string msg)
        {
            //소켓에 관련된 스레드가 돌고 있으므로 application 과의 충돌을 피하기 위해 델리게이트를 이용합니다.
            LogWriteDelegate deleChatWrite = new LogWriteDelegate(MessageWrite);
            //생성한 델리게이트를 이용하여 invoke 를 실행합니다.
            this.Invoke(deleChatWrite, new object[] { msg });
        }
        public void DelePaeInit(int glv, int ggread)   // 초기화 하고 패를 돌리리는 장면을 위임하여 실행하도록 합니다. 
        {
            // 초기화 하고 패를 돌리는 장면을 연출하는 MyInitalize 메소드를 호출할 델리게이트 선언  
            InitDelegate dInit = new InitDelegate(MyInitalize);
            // Invoke 메소드를 실행  
            this.Invoke(dInit, new object[] { glv, ggread });
        }
        public void DeleGameSendMsg(string msg)   //  서버로 메시지 전송
        {
            //  서버로 메시지 전송 호출할 델리게이트 선언  
            GameSendMsg dSendMs = new GameSendMsg(SendMessage);
            // Invoke 메소드를 실행  
            this.Invoke(dSendMs, new object[] { msg });
        }

        public void deLoginAfter(int sno)
        {
            // 초기화 하고 패를 돌리는 장면을 연출하는 MyInitalize 메소드를 호출할 델리게이트 선언  
            GameLoginAfter dSendMs = new GameLoginAfter(LoginAfter);
            // Invoke 메소드를 실행  
            this.Invoke(dSendMs, sno);
        }

        //public void DelChat(string nick, string msg)    //채팅 처리를 위한 메소드위임 처리
        //{
        //    GameChatDelegate dChat = new GameChatDelegate(ChatSEND);
        //    //invke 실행
        //    this.Invoke(dChat, new object[] { nick, msg });
        //}
    }
}
