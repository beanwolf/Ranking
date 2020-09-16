using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Media;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System.Deployment.Application;

namespace _4000Client
{
    public partial class Ranking4000Sung : Form
    {
        /*=====================================================================
        *  랭킹사천성 v0.0.1            2019 02 22      by 뮤
        *              환경변수 panel
        *
        ** =====================================================================
        ** ===================================================
         *          <<<  실행순서  >>>
         * ===================================================
         *  1.  Ranking4000Sung()
         *  1-1.      InitializeComponent();
         *          btnConn_Click(                           //Login btn
         *  2-1.    LoginAfter() 
         *  3.  btn_EasyStart_Click
         *  3-0       Lv001_Start();
         *          Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);*  
         *  3-1.      GameReady()
         *  3-1-1           MakePae();      //패 어레이를 생성한다.
         *  3-1-2           MyInitalize();
         *  4       PaeClick
         *  
         *  
         *  427 : End ServerSend_cmdGameEnd)(
         *  GameFinishBoardClear()          //      게임 처음 상태로 돌리기
         */
        //=================================================
        //    디버그용 정식배포전 점검해야함
        //=================================================
        //==============================================================
        //true : 개발용, false : 배포용
      
#if true    //개발용
        private int LocalIPUse = 1;                         // 로컬 OR 서버       어디접속할건지 1:로컬 2:서버
        public int _AutoLogin_Air = 1;  //자동로그인시킬건가:1, 0:아니오


        private int _svrPort = 9212;                        // 포트
        private int DebugPanelVisible = 0;                  // 디버거 출력        ON/OFF 0:off / 1:On
        public string GameVer = "1001";                     // 클라이언트 버전      (서버에서 받아오거나 기록하게해야 한다)
        public string mx = "999999";
        public string mi = "Air";
        private int GameEndLeftPae = 0;                   // 남은완료패갯수     게임클리어 시킬 패갯수 0:기본 120:1번성공
#else   //배포용
        private int LocalIPUse = 2;                         // 로컬 OR 서버       어디접속할건지 1:로컬 2:서버
        private int _svrPort = 9211;                        // 포트
        private int DebugPanelVisible = 0;                  // 디버거 출력        ON/OFF 0:off / 1:On
        private int GameEndLeftPae = 0;                   // 남은완료패갯수     게임클리어 시킬 패갯수 0:기본 120:1번성공
        public string GameVer = "1001";                     // 클라이언트 버전      (서버에서 받아오거나 기록하게해야 한다)
        public int _AutoLogin_Air = 0;  //자동로그인시킬건가     0:아니오
        public string mx = "";
        public string mi = "";
#endif
        //==============================================================
        private string RankingServerIP = "";                // 서버 IP            입력하면 IP로 접속합니다.

        
        #region 변수선언
        private int LoginInOKFlag = 0;                      //로그인에 성공해야 1이된다.
        private string _svrIP = "127.0.0.1";                // 기본 아이피
        private string _myID = "";                          // 나의 별칭 아이디를 가지고 있습니다. 
        private string _myPWD = "";                         // 나의 패스워드를 가지고있습니다.
        private string _myNick = "";                        // 나의 닉네임 변수
        private int _myLevel = 1;                           // 나의 레벨
        private int _myGameScore = 0;                       // 현재게임 점수
        private int _NowPlayGameLevel = 1;                  //현재 플레이 중인 게임 레벨
        private int _NowPlayGameGrade = 1;                  //현재 플레이 중인 게임 레벨의 난이도
        private string _NowPlayGame_ResultDataString = "";  //현재 플레이 중인 게임의 베스트타임 & 유저
        private int _myP_all = 0;
        private double _myP_cash = 0;
        public static string _UserInfoID = "";                     //유저 정보 볼 아이디 넣어준다.
        #region
        //=====================================================
        //
        //  배포전에 지워야함
        //
        //=====================================================
        #endregion

        #region 게임중 선택패에 대한 내용들
        //==============================================
        //      게임 시작시에 필요한 변수들 -시작
        //==============================================

        private int _PaeCenterPanArrangePositionStyle = 0; //패배치방식 0:바둑판(default), 1:벌집

        private string _firstChoicePaeName = "";            //첫번째로 선택한 패의 라벨 네임입력
        private Label _fristChoiceLabl = null;                     //첫번째 선택한 라벨
        private int _fristChoiceINDEXsNo;                     //첫번째 선택한 라벨의 인덱스
        int PaeSameCount = 0;                               //게임내에서 같은패 갯수( 기본값 8 중요!)

        DateTime _GameStartTime = DateTime.Now;           //게임 시작 시간
        DateTime _LastRemovePaeStopWatch;                   //마지막으로 패를 없애서 점수받은시간
        private int _Game_RemovePoint_NOW = 100;                      //    없앨때마다 기본으로 주는 포인트
        private int _Game_RemovePoint_OneSecINRate = 200;                 //  1초내로 없애면 주는 포인트 배율
        private int _Game_RemovePoint_TwoSecInRate = 100;                 // 2초내로 업애면 주는 퐁니트 배율
        private int _Game_RemovePoint_ThreeSecInRate = 50;                 // 2초내로 업애면 주는 퐁니트 배율

        private int _ClearPaeSungCode = 0;                  //클리어단계에 따른 사운드 코드 0기본, 도레미파솔라시도

        //private int _Game_AllPae_Count = 0;                     //그판의 패의 갯수
        private int _GAme_LeftPae_Count = 999;                  //남은 패의 갯수

        private string _arr_Clear_PaeList = "";                       // 없앤 패 라벨 이름 붙여서 계속 넣어주자
        private string _arr_Clear_PaeList_MakePaeList = "";             //패만들때 이름붙여서 계속 넣어주자.
        //private int paepoindex;
        //private PaePo _firstChoicePaePo;                       
        //private string _NexttChoicePaeName = "";            //다음으로 선택한 패의 라벨 네임입력
        //==============================================
        //      게임 시작시에 필요한 변수들 - 끝
        //==============================================
        #endregion

        //=========================
        //  LV 1
        //=========================
        private Size _panGameBoardLv1Size_endgame = new Size(311, 72);       //게임시작전에 레벨게임보드를 버튼모양으로 만들어준다.
        private Size _panGameBoardLv1Size_Startgame = new Size(1006, 615);       //게임시작전에 레벨게임보드를 버튼모양으로 만들어준다.
        private Size _panGameBoardGameing = new Size(1003, 615);            //플레이 됬을때 게임판 사이즈
        private Point _panGameBoardLv1Loc_endGame = new Point(5, 32);        //LV1 게임시작전 위치
        private Point _panGameBoardLv2Loc_endGame = new Point(5, 122);        //Lv2 게임시작전 위치
        private Point _panGameBoardLv3Loc_endGame = new Point(5, 212);        //Lv2 게임시작전 위치
        private Point _panGameBoardLv4Loc_endGame = new Point(5, 302);        //Lv2 게임시작전 위치

        private Point _panGameBoardLv1Loc_StartGame = new Point(5, 32);        //Lv1 게임시작후 위치
        private Point _panGameBoardLv2Loc_StartGame = new Point(5, 32);        //Lv2 게임시작후 위치

        private Size _panCenterPanLv1Size_Startgame = new Size(818, 550);      //centerpan 게임판 사이즈
        private Point _panCenterPanLv1Location_Startgame = new Point(95, 51);      //centerpan 게임판 위치
        private Point _webBrowser_Login_Location = new Point(380, 10);                  //브라우저위치
        private Point _ChatBox_Gaming_Locattion = new Point(1015, 218);                  //게임중 채팅창 위치

        private Size _panLoginWindowSize = new Size(725, 271);     //로그인창 사이즈
        //=========================
        //  LV 2
        //=========================

        public int PaeNUMTEMP = 1;                  //임시넘버링

        private TcpClient _client = null;               // 클라이언트 소켓 객체 생성 
        private Thread _thread = null;                  // 쓰레드 객체를 생성합니다. 
        private NetworkStream _netStream = null;        // 네크워크 스트림  
        private StreamReader _stmReader = null;     // 읽기 스트림 
        private StreamWriter _stmWriter = null;     // 쓰기 스트림 
        private bool _isStop = false;                   // 중지에 관한 플래그 

        private System.Threading.Timer Timers = null;   //서버통신용 타이머
        private System.Threading.Timer Timer = null;    //게임에 시간표시용 타이머

        SoundPlayer x1 = new SoundPlayer();
        SoundPlayer x2 = new SoundPlayer();
        SoundPlayer x3 = new SoundPlayer();         //패연속 리므브용이다.

        //다이이렉트 사운드  -------------- 현재 실행안되고 있어 제외

        private Device dsDevice = null;
        private SecondaryBuffer buffer = null;
        private Device dsDevice2 = null;
        private SecondaryBuffer buffer2 = null;


        ListViewItem _Listview1 = new ListViewItem();


        private PaeList _arrOpenPae;        //바닥에 깔려진 패 리스트
        private Player _player1;                    //플레이어1
        private string _selectPae = "";     // 선택한 패에 대한 정보를 나타낼때 쓰일 변수 입니다. 

        #region 게임 사운드 변수
        public string _GSound_GameResult = "result.wav";
        public string _GSound_GameClear = "win.wav";
        public string _GSound_GameClearSpecial = "winSpecial.wav";
        public string _GSound_GameFail = "drop.wav";
        public string _GSound_FirchoicePae = "Unlock.wav";
        public string _GSound_SenchoicePae = "KbdSwipeGesture.wav";
        public string _GSound_FailClickPae = "KbdKeyTap.wav";

        #endregion
        #endregion 변수선언끝

       

        //============================================================================================
        // ↑↑↑↑↑↑↑
        // 이상 선언문들
        //============================================================================================
        //
        // 게임플레이 클래스 시작
        //
        //============================================================================================
        public Ranking4000Sung()
        {
            InitializeComponent();

            //실해인자에서 값 받아오기
            // 랭킹 서버리스트 클릭시 받아오는 소스들에 들어있는경우
            //실해인자에서 값 받아오기

            if (System.Environment.GetCommandLineArgs().Length > 1)
            {
                this.Text = System.Environment.GetCommandLineArgs().Length.ToString();
                txtIP.Text = System.Environment.GetCommandLineArgs()[1];
                txtNick.Text = System.Environment.GetCommandLineArgs()[1];
                txt_PWD.Text = System.Environment.GetCommandLineArgs()[1];
            }
            //실해인자에서 값 받아오기 =---끝
            dsDevice = new Device();                    //DxSound
            dsDevice2 = new Device();   //DxSound
            dsDevice.SetCooperativeLevel(this, CooperativeLevel.Normal);    //DxSound
            dsDevice2.SetCooperativeLevel(this, CooperativeLevel.Normal);   //DxSound

            pan_Login.Size = _panLoginWindowSize;        //로그인창 사이즈
            pan_Login.Location = new Point(328, 174);   //록인창 위치
            //lbl_LoginMessage.Text = "업데이트 확인중...";


            //로그인창에 웹브라우져에서 IP얻어오기
            webBrowser_Login.Location = _webBrowser_Login_Location; //브라우저의 위치 확보하기
            webBrowser_Login.Navigate("https://www.ranking.co.kr/game/4000ServerIP");

            if (LocalIPUse == 2)
            {
                txtIP.Text = RankingServerIP;
                txtIP.Visible = false;
            }
            else
            {
                // 현재 컴퓨터의 아이피를 초기값으로 정합니다. 
                IPHostEntry localHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                // 현재 컴퓨터에서 얻은 IP 값중 첫번째 값을 기본IP 값으로 세팅해 줍니다. 
                txtIP.Text = localHostEntry.AddressList[0].ToString();
            }


            lbl_GameVer.Text = " 2019 Munet MuseMiMu,  Ver : " + GameVer;
            lbl_GameVer.ForeColor = Color.Gray;
            //lbl_LoginMessage.Text = "";

            if (DebugPanelVisible == 1) txt_ErrLog.Visible = true;     //디버거 출력창
            //게임패널 초기위치 5,32, 사이즈 1006,615

            #region 자동업데이트 검사

            //======================================
            //자동업데이트 검사
            //======================================
            UpdateCheckInfo info;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("새로운버전을 다운로드 할수 없습니다. \r\n인터넷 연결을 확인후 다시 시도 해 주세요\r\n" + dde.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (InvalidDeploymentException Ide)
                {
                    MessageBox.Show("업데이트를 확인 할수 없습니다.\r\n" + Ide.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("업데이트를 할수 없습니다." + ioe.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (info.UpdateAvailable)
                {
                    //if (MessageBox.Show("새로운 게임 버전이 있습니다. 다운로드 하겠습니까?", "랭킹게임", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    //그냥 무조건 업데이트 하게 만들자
                        try
                        {
                            ad.Update();
                            Application.Restart();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw;
                        }
                    //}
                    //else
                    //{
                    //    this.Close();
                    //}

                }
                else
                {
                    lbl_LoginMessage.Text = "";
                    lbl_GameVer.Text = " 2019 Munet MuseMiMu,  Ver :"+GameVer+" (" + ad.CurrentVersion+")";
                }
            }
            //======================================
            //자동업데이트 검사   ------끝
            //======================================
            #endregion


            //이미지 캐싱용
            // @"images\Pae\" + inValue.ToString() + str + "50.png"
            lbl_img01.Image = Image.FromFile(@"images\Pae\man150.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img02.Image = Image.FromFile(@"images\pae\man250.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img03.Image = Image.FromFile(@"images\pae\man350.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img04.Image = Image.FromFile(@"images\pae\man450.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img05.Image = Image.FromFile(@"images\pae\man550.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img06.Image = Image.FromFile(@"images\pae\man650.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img07.Image = Image.FromFile(@"images\pae\man750.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img08.Image = Image.FromFile(@"images\pae\man850.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img09.Image = Image.FromFile(@"images\pae\man950.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
            lbl_img10.Image = Image.FromFile(@"images\pae\pun150.png");   // 라벨에 보여질 이미지 정보를 보여줍니다. 
        }




        private void LogintWaiting()
        {
            lbl_LoginMessage.Visible = true;
            lbl_LoginMessage.ForeColor = Color.Blue;
            lbl_LoginMessage.Text = "로그인중...";
            for (int i = 1; i < 40; i++)
            {
                lbl_LoginMessage.Text = "";
                Thread.Sleep(100);
                if (LoginInOKFlag == 1)
                {
                    btnConn.Enabled = false;
                    btnConn.Visible = false;
                    lbl_LoginMessage.Text = "로그인 성공";
                    //LoginAfter(LoginInOKFlag);
                    break;
                }
                else if(i ==39 && LoginInOKFlag !=1)
                {
                    lbl_LoginMessage.Text = "로그인 실패(아이디 패스워드 확인바람)";
                    lbl_LoginMessage.ForeColor = Color.Red;
                    btnConn.Enabled = true;
                    btnConn.Visible = true;

                    break;
                }else
                {
                    lbl_LoginMessage.ForeColor = Color.Blue;
                    lbl_LoginMessage.Text = "로그인중...";
                }
            }
            //로그인을 기다립니다.
        }
     
        private void LoginAfter(int sno)                   //로그인 후에 처리해야 될것들 모음
        {
            pan_Login.Visible = false;
            //webBrowser_Login.Visible = false;   //공지사항 웹브라우저를 닫는다.
            pnl_CashPaeImage.Visible = false;// 패이미지 캐싱용을 닫는다.
            //배경이미지를 넣어준다.
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ranking4000Sung));
            pan_GAMEBoard1.Size = _panGameBoardLv1Size_endgame;
            Pan_UserList.Visible = true;    //좌측 아바타및 유저리스트 패널 보이게한다.

            UriBuilder builder = new UriBuilder("https://www.ranking.co.kr/Account/AvataDisplay?myid=" + _myID);
            string url = builder.ToString();
            webBrowser_MyAVATA.Navigate(url); //내아바타불러온다.
            lbl_MyidDisplay.Text = _myID;   //내아이디 표시해주자
            btn_EasyStart.Visible = true;   //바로시작 버튼 보여주자
            Point Cpos = new Point(10, 448);
            exitBin.Text = "종료";  //글씨를 없애서 배경이미지 버튼이 보이게 한다.


            //txt_ChatWindows.Visible = true; //채팅창을 보여준다.
            //txt_ChatWindows.Location = Cpos;        //채팅윈도위 위치
            //txt_ChatWindows.Size = new Size(998, 192);  //채팅 윈도우 사이즈
            richtxt_ChatWindows.Visible = true; //채팅창을 보여준다.
            richtxt_ChatWindows.Location = Cpos;        //채팅윈도위 위치
            richtxt_ChatWindows.Size = new Size(998, 192);  //채팅 윈도우 사이즈
            richtxt_ChatWindows.ForeColor = Color.Black;
            
            txtSend.Visible = false;         // 채팅 입력창을 보여준다.

            pan_GAMEBoard1.Visible = true;   //1레벨 패널을 보여준다.
            pan_GAMEBoard1.Location = _panGameBoardLv1Loc_endGame;      //2랩 보드 작은사이즈로 원위치
            pan_GAMEBoard1.Size = _panGameBoardLv1Size_endgame;         //2랩 보드 작은사이즈로

            pan_GAMEBoard2.Visible = true;   //2레벨 패널을 보여준다.
            pan_GAMEBoard2.Location = _panGameBoardLv2Loc_endGame;      //2랩 보드 작은사이즈로 원위치
            pan_GAMEBoard2.Size = _panGameBoardLv1Size_endgame;         //2랩 보드 작은사이즈로

            pan_GAMEBoard3.Visible = true;   //3레벨 패널을 보여준다.
            pan_GAMEBoard3.Location = _panGameBoardLv3Loc_endGame;      //2랩 보드 작은사이즈로 원위치
            pan_GAMEBoard3.Size = _panGameBoardLv1Size_endgame;         //2랩 보드 작은사이즈로

            pan_GAMEBoard4.Visible = true;   //4레벨 패널을 보여준다.
            pan_GAMEBoard4.Location = _panGameBoardLv4Loc_endGame;      //2랩 보드 작은사이즈로 원위치
            pan_GAMEBoard4.Size = _panGameBoardLv1Size_endgame;         //2랩 보드 작은사이즈로

            btn_Lavel1Start.Visible = true;         //레벨 스타트 버튼 보이게
            btn_Lv1Nor_Start.Visible = true;       //렙레2 스타트 버튼 보이게
            btn_Lv1Hard_Start.Visible = true;       //렙레3 스타트 버튼 보이게
            btn_Lv2Easy_Start.Visible = true;     //레벨2 스타트 버튼 보이게
            btn_Lv2Nor_Start.Visible = true;       //렙레2 스타트 버튼 보이게
            btn_Lv2Hard_Start.Visible = true;       //렙레3 스타트 버튼 보이게
            btn_Lv3Easy_Start.Visible = true;     //레벨2 스타트 버튼 보이게
            btn_Lv3Nor_Start.Visible = true;       //렙레2 스타트 버튼 보이게
            btn_Lv3Hard_Start.Visible = true;       //렙레3 스타트 버튼 보이게
            btn_Lv4Easy_Start.Visible = true;     //레벨2 스타트 버튼 보이게
            btn_Lv4Nor_Start.Visible = true;       //렙레2 스타트 버튼 보이게
            btn_Lv4Hard_Start.Visible = true;       //렙레3 스타트 버튼 보이게

            _myGameScore = 0;                   //점수를 0으로 만들어준다.
            DebugWrite("LoginAfter 완료.");

        }



        #region     델리게이트를 사용한 Invoke 처리 메소들
     
        public void CmdLoginFail(string msg)
        {
            DeleLoginFail(msg);
        }
        public void LoginFail(string command) {
            string[] cmds = command.Split(':');
            if (cmds.Length == 3)
            {
                lbl_LoginMessage.Text = cmds[1] + "는 " + cmds[2];
            }
            else
            {
                lbl_LoginMessage.Text = cmds[1];
            }
            //lbl_LoginMessage.Text = cmds.Length.ToString();
            lbl_LoginMessage.ForeColor = Color.Red;
            pan_Login.Visible = true;
            pan_GAMEBoard1.Visible = false;
            btnConn.Visible = true;
            btnConn.Enabled = true;
            //DebugWrite("아이디 또는 패스워드가 틀렸단다.");
        }

    

        #endregion     델리게이트를 사용한 Invoke 처리 메소들 끝====================================================


        public void GameLoginOK(string command)
        {
            string[] cmds = command.Split(':');
            //pnl_AllPaeImagesList.Visible = false;   //패이미지 로딩버퍼용 패널

            LoginAfter(1);
            //게임데이타 뿌려주자
            //_GameDbData = double.Parse(redern["p_cash"].ToString()) + ":" + redern["p_all"].ToString() + ":" + redern["p_win"].ToString() + ":" + redern["p_level"].ToString() + ":" + redern["p_record"].ToString() + ":" + redern["p_gameclear_level"].ToString();
            //"LOGINOK:" + mid + ":로그인 성공:"+ _GameDbData);

            // 0: "LOGINOK", 1:mid, 2:로그인 성공, 3:p_cash, 4:p_all, 5:p_win, 6: p_level, 7
            _myP_all = int.Parse(cmds[4].ToString());
            _myP_cash = double.Parse(cmds[3].ToString());
            ChatWrite("\r\n[" + cmds[1] + "님 접속을 환영합니다.]\r\n>> 현재 개발버전이며 정식서비스중이 아닙니다.\r\n>> 아직 버그나 문제점이 많이 있습니다. 불편하시더라도 양해부탁드립니다.\r\n>> 채팅은 엔터후에 글을 적고 다시 엔터를 누르시면 됩니다.\r\n");

            lbl_MyPcashlDisplay.Text = _myP_cash.ToString("#,##0") +" 랭";
            lbl_MyPallDisplay.Text = _myP_all.ToString("#,##0") + " 전";

            listViewUserList.View = View.Details;
            listViewUserList.Columns.Add("ID", 100, HorizontalAlignment.Left);
            listViewUserList.Columns.Add("Nick", 90, HorizontalAlignment.Left);

        }

        public void GamelevelDataWrite(string command)
        {
            //게임 기록 갱신용
            
            string[] cmds = command.Split(':');
            if (cmds[1] == "1") lbl_lv1BestResultInfo.Text = "Lv " + cmds[1] + "-" + cmds[2] + " " + double.Parse(cmds[3].ToString()).ToString("#,##0") + "초 (" + cmds[4] + " 님)";
            else if (cmds[1] == "2") lbl_lv2BestResultInfo.Text = "Lv " + cmds[1] + "-" + cmds[2] + " " + double.Parse(cmds[3].ToString()).ToString("#,##0") + "초 (" + cmds[4] + " 님)";
            else if (cmds[1] == "3") lbl_lv3BestResultInfo.Text = "Lv " + cmds[1] + "-" + cmds[2] + " " + double.Parse(cmds[3].ToString()).ToString("#,##0") + "초 (" + cmds[4] + " 님)";
            else if (cmds[1] == "4") lbl_lv4BestResultInfo.Text = "Lv " + cmds[1] + "-" + cmds[2] + " " + double.Parse(cmds[3].ToString()).ToString("#,##0") + "초 (" + cmds[4] + " 님)";
        }



        public void LeaveServerUser(string cmd) //로그아웃한 사람 리스트뷰에서 삭제
        {
            for (int i = 0; i < listViewUserList.Items.Count; i++)
            {
                if (listViewUserList.Items[i].Text == cmd)  listViewUserList.Items.RemoveAt(i);
            }
        }
        public void JoinServerUser(string cmd)
        {
            //처음 접속했을때 먼저온사람이 있으면 그거 받아서 뿌려줘야 한다.
            string[] cmds = cmd.Split(':');
            // 1: id, 2:닉네임, 3:레벨
            DebugWrite("JoinServerUser" + cmd[0] + cmd[1] + cmd[2]);
            //리스트뷰에 유저 추가시키자
            ListViewItem item1 = new ListViewItem(cmds[1], int.Parse(cmds[3].ToString()));
            //            item1.SubItems.Add(lvno.ToString());
            item1.SubItems.Add(cmds[2]);
            listViewUserList.Items.Add(item1);
             

        }
        public void GameClear(int GLevel, DateTime StartTime, int gameScore, int GGraed)  //게임 종료시 결과및 대기실로 보내자
        {
            //================================================================
            //  게임클리어
            //================================================================
            TimeSpan gametime = DateTime.Now - _GameStartTime;
            pnl_GameEnd.Visible = true;     //게임종료 결과판 보이기
            pnl_GameEnd.BringToFront();     //게임종료 결과판 앞으로

            

            _myP_cash = _myP_cash + gameScore;      //게임 머니 정산
            lbl_MyPcashlDisplay.Text = _myP_cash.ToString("#,##0") + " 랭";      //게임 머니 표시


            lbl_GameEnd_TitleText.BringToFront();   //게임종료 결과 타이틀 앞으로
            lbl_GameEnd_TitleText.Text = "GAME CLEAR !";
            lbl_GameEnd_TitleText.ForeColor = Color.Yellow;
            lbl_GameEnd_TitleText.Visible = true;   //게임클리어 타이틀 보이기
            lbl_GameResult_Text.Visible = true;
            lbl_GameResult_Text.BringToFront();     //게임 결과 텍스트 앞으로
            lbl_GameResult_Text.Text = "레벨 " + GLevel + "-" + GGraed + " 클리어!! " + gametime.TotalSeconds.ToString("#,##0") + "초";
            lbl_GameResult_Text.ForeColor = Color.Yellow;

            // 서버로 게임결과를 받아와서 기록에 관한걸 받아오자
            ServerSend_cmdGameEnd(GLevel, gametime.TotalSeconds.ToString(), gameScore, GGraed);
            //MessageBox.Show("=============\r\n   GAME CLEAR \r\n=============");       //게임 클리어 창띄우기
            CmdSoundPlay1("result.wav"); //완료 소리내자
            Thread.Sleep(1000);     // 잠시 멈추게 하자
            AppendMsg("============= GAME CLEAR !!!! =============\r\n= 클리어 타임 : " + gametime.TotalSeconds.ToString("#,##0") + "초\r\n= 게임 점수 : " + _myGameScore.ToString("#,##0")+ "\r\n=======================================");
            btn_Gameclear_CloseButton.Visible = true;
            btn_Gameclear_CloseButton.Enabled = true;



            //1. 게임판 안보이게 하기
            //2. 대기실 화면으로 돌려보내기
            //3. 게임리스트 업댓하기
            //4. 다시 게임시작 버튼 활성화
        }//게임 종료시 결과및 대기실로 보내자
        public void ServerSend_cmdGameEnd(int GLevel, string GameStartTime, int _GameScore, int gGraed)
        {
            //게임 ㅣ완료시 데이타를 서버로 전송하기 위한.
            CmdSendMessage("GAMERESULT:" + _myNick + ":" + GLevel + ":" + GameStartTime.ToString() + ":" + _GameScore + ":" + gGraed);
            //SendMessage("GAMERESULT:"+_myNick+":"+GLevel + ":" +GameStartTime.ToString() + ":" +_GameScore);
            //SendMessage("NICK:" + txtNick.Text + ":" + txt_PWD.Text);         // 별칭과 패스워드를 전송합니다. 

        }
        public void ServerSend_CmdGameStart(int GLevel, int gGraed)
        {
            SendMessage("GAMERSTART:" + _myNick + ":" + GLevel + ":" + gGraed);
            //CmdSendMessage("GAMERSTART:" + _myNick + ":" + GLevel + ":" + gGraed);    //델리게이트로 한게 에러?

        }
        public void GameOver(string S1, string S2)  //게임 종료시 결과및 대기실로 보내자
        {
            //================================================================
            //  게임 실패 (게임오버)
            //================================================================
            pnl_GameEnd.Visible = true;     //게임종료 결과판 보이기
            pnl_GameEnd.BringToFront();     //게임종료 결과판 앞으로
            lbl_GameEnd_TitleText.BringToFront();   //게임종료 결과 타이틀 앞으로
            lbl_GameEnd_TitleText.Text = "GAME OVER !";
            lbl_GameEnd_TitleText.ForeColor = Color.Red;
            lbl_GameResult_Text.Visible = true;
            lbl_GameResult_Text.Text = S2.ToString();
            lbl_GameResult_Text.ForeColor = Color.White;
            CmdSoundPlay1(_GSound_GameFail);    // 게임 실패 소리내자
            //MessageBox.Show("GAME OVER");       //게임 실패 창띄우기

            //1. 게임판 안보이게 하기
            //2. 대기실 화면으로 돌려보내기
            //3. 게임리스트 업댓하기
            //4. 다시 게임시작 버튼 활성화
        }//게임 종료시 결과및 대기실로 보내자



        private void btnConn_Click(object sender, EventArgs e)
        {
            //================================================================
            //  로그인 버튼
            //================================================================
            // 서버로 부터 로그인을 확인받아옵니다.
            lbl_LoginMessage.Text = "로그인중....";
            lbl_LoginMessage.ForeColor = Color.Blue;
            listViewUserList.Clear();   //리스트뷰 리셋?  재로그인 하는 경우  때문에
            int filltxt = 0;
            // 자신이 사용할 별명을 저장합니다. 
            if (_AutoLogin_Air == 1)
            {
                _myID = mi;       //아이디
                _myNick = mi;     //닉네임   (서버에서 받아와야 한다)
                _myPWD = mx;
                filltxt = 1;
                // 접속 버튼을 한번만 누르도록 합니다. 
                btnConn.Enabled = false;
                lbl_LoginMessage.Text = "로그인중....";
                lbl_LoginMessage.ForeColor = Color.Blue;

                startClient();              // 클라이언트로서 서버에 접속하고 통신할수 있도록 쓰레드를 가동합니다.
            }
            else
            {
                if (txtNick.Text == "")
                {
                    filltxt = 0;
                    lbl_LoginMessage.Text = "아이디를 입력해 주세요";
                    lbl_LoginMessage.ForeColor = Color.Red;

                }
                else if (txt_PWD.Text == "")
                {
                    filltxt = 0;
                    lbl_LoginMessage.Text = "패스워를 입력해 주세요";
                    lbl_LoginMessage.ForeColor = Color.Red;
                }
                else
                {
                    _myID = txtNick.Text;       //아이디
                    _myNick = txtNick.Text;     //닉네임   (서버에서 받아와야 한다)
                    _myPWD = txt_PWD.Text;
                    // 접속 버튼을 한번만 누르도록 합니다. 
                    btnConn.Enabled = false;
                    lbl_LoginMessage.Text = "로그인중....";
                    lbl_LoginMessage.ForeColor = Color.Blue;
                    startClient();              // 클라이언트로서 서버에 접속하고 통신할수 있도록 쓰레드를 가동합니다.
                }
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        #region Network관련 모듈
        private void startClient()
        {
            if (LocalIPUse == 1)        //로컬 접속이라면
            {
                if (txtIP.Text.Trim() != "") _svrIP = txtIP.Text;       // 입력한 접속할 아이피의 정보로 받습니다. 
            }
            else _svrIP = RankingServerIP;               //상단에 적은 IP(랭킹서버) 로 접속합니다.

            _thread = new Thread(new ThreadStart(ClientReceive));       // 서버와 통신할 쓰레드를 생성합니다. 
            _thread.IsBackground = true;                // 백그라운드 모드로 설정합니다. 
            _thread.Start();                            // 쓰레드를 시작합니다. 

            Thread.Sleep(300);
            //btnConn.Enabled = true;
            ////LoginAfter();
            LogintWaiting();        //로그인  기다리는중 후에 처리해야 될것들 모음
                                    //LoginAfter(1);
                                    //로그인 완료 메세지를 기다리게 둔다.
                                    //if(dxx)
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("로그인 하지 못했습니다.\n서버에서 응답이 없습니다.");
                                    //    btnConn.Enabled = true;     // 버튼 다시보여줘야 재로그인하지
                                    //}
        }
        public void SendMessage(string msg)          // 메세지를 전송합니다. 
        {
            if (_stmWriter != null)
            {
                if (_netStream != null)
                {
                    if (_client != null)
                    {
                        try
                        {
                            //여기쯤 검사해야 한다. 접속중인지.
                            _stmWriter.WriteLine(msg);
                            _stmWriter.Flush();            // 메세지를 방출합니다. 
                        }
                        catch
                        {
                            //접속이 끊어졌다.
                            pan_Login.Visible = true;
                            pan_Login.BringToFront();
                            return;
                        }
                    }
                    else
                    {
                        //아직 보완해야 겠다. 어덯게 도니는지 모른다.
                        //접속이 끊어졌다.
                        pan_Login.Visible = true;
                        pan_Login.BringToFront();
                    }
                }
                else
                {
                    //아직 보완해야 겠다. 어덯게 도니는지 모른다.
                    //접속이 끊어졌다.
                    pan_Login.Visible = true;
                    pan_Login.BringToFront();
                }
            } else
            {
                //아직 보완해야 겠다. 어덯게 도니는지 모른다.
                //접속이 끊어졌다.
                pan_Login.Visible = true;
                pan_Login.BringToFront();
            }

        }

        // 서버로부터 온 메시지를 받는 부분 입니다. 
        private void ClientReceive()
        {
            try
            {
                _client = new TcpClient(_svrIP, _svrPort);                      // 클라이 언트 객체를 생성함과 동시에 접속합니다. 
                _netStream = _client.GetStream();                               // 생성된 네트워크 스트림 객체를 얻습니다. 
                _stmReader = new StreamReader(_netStream);                      // 네트워크 스트림으로 부터 읽기 스트림 객체를 생성합니다. 
                _stmWriter = new StreamWriter(_netStream);                      // 네트워크 스트림으로 부터 쓰기 스트림 객체를 생성합니다. 



                SendMessage("NICK:" + _myNick + ":" + _myPWD);         // 별칭과 패스워드를 전송합니다. 
                while (!_isStop)                            // 통신을 멈출때까지 
                {
                    string rcvMsg = _stmReader.ReadLine();              // 스트림으로 전송된 메시지를 읽습니다. 
                    AnalysisCommand(rcvMsg);                        // 메시지를 분석하여 명령을 구분해 냅니다.
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                if (_stmReader != null)
                {

                    _stmReader.Close();                 // 읽기 스트림을 닫습니다. 
                    _stmWriter.Close();                 // 쓰기 스트림을 닫습니다. 
                    _netStream.Close();
                    _client.Close();                // 클라이언트를 종료합니다. 
                }
                else
                {
                    DebugWrite("서버에서 응답이 없습니다.");
                }
            }
        }
        #endregion  Network관련 모듈 끝



        #region 이벤트 처리 모듈 마우스 이동,호버 사운드 (panTitle_MouseMove, panTitle_MouseDown,btn_mouse_hoverSound)
        // 전역 변수로 포인터를 선언 합니다. 
        public Point ptRect = new Point(0, 0);
        public Point ptRect2 = new Point(0, 0);
        // MouseMove 이벤트의 구현 부분입니다.
        private void panTitle_MouseMove(object sender, MouseEventArgs e)
        {
            // 타이틀에서 마우스의 왼쪽 버튼을 클릭 했을때의 구현입니다.
            if (e.Button == MouseButtons.Left)
            {
                // 현재 위치로 부터 이벤트가 발생한 위치까지를 계산합니다.
                Point pt = new Point(this.Location.X + e.X - ptRect.X, this.Location.Y + e.Y - ptRect.Y);
                // 마우스에 의해 이동된 위치까지 이동 시킵니다. 
                this.Location = pt;
            }
        }

        // MouseDown 이벤트의 구현 부분입니다.
        private void panTitle_MouseDown(object sender, MouseEventArgs e)
        {
            // 마우스를 이동하기 시작한 위치를 구합니다. 
            ptRect.X = e.X;
            ptRect.Y = e.Y;
        }

        private void btn_mouse_hoverSound(object sender, EventArgs e)       //마우스 호버 사운드
        {
            //마우스 호버시에 사운드 나오게 하련다.
            CmdSoundPlay1("buttonon.wav");
        }

        private void CmdSoundPlay1(string sname)
        {
            DeleGameSound1(sname);
            //GameSoundDxs1(sname);       //다이렉트 사운드
        }
        private void CmdSoundPlay2(string sname)
        {
            DeleGameSound2(sname);
            //GameSoundDX1(sname);       //다이렉트 사운드
            //DeleGameSoundDX1(sname); //다이렉트 사운드 딜리게이트

        }
        private void CmdSoundPlay3(int sname)
        {
            //연속으로 없앨때 음계를 위한 것이다.
            //DeleGameSound3("piano" + sname + ".wav");
            GameSoundDxs1("piano" + sname + ".wav");        //다이렉트 사운드
        }

        private void GameSound2(string sname)
        {
            x2.SoundLocation = Application.StartupPath + @"\Sound\" + sname;
            x2.Play();
        }
        private void GameSound1(string sname)
        {
            x1.SoundLocation = Application.StartupPath + @"\Sound\" + sname;
            x1.Play();

        }
        private void GameSound3(string sname)
        {
            x3.SoundLocation = Application.StartupPath + @"\Sound\" + sname;
            x3.Play();
        }
        
        private void GameSoundDX1(string sname)
        {
            SecondaryBuffer buffer2 = new SecondaryBuffer(Application.StartupPath + @"\Sound\" + sname, dsDevice);
            if (dsDevice2 != null) buffer2.Play(0, BufferPlayFlags.Default);
        }

        private void GameSoundDxs1(string sname)
        {
            //Ds1.Stop();
            SecondaryBuffer buffer = new SecondaryBuffer(Application.StartupPath + @"\Sound\" + sname, dsDevice);
            if (dsDevice != null) buffer.Play(0, BufferPlayFlags.Default);
        }
        private void GameSoundDxs2(string sname)
        {
            SecondaryBuffer buffer2 = new SecondaryBuffer(Application.StartupPath + @"\Sound\" + sname, dsDevice);
            if (dsDevice2 != null) buffer2.Play(0, BufferPlayFlags.Default);
        }
        private void GameSoundDxs3(string sname)
        {
            SecondaryBuffer buffer = new SecondaryBuffer(Application.StartupPath + @"\Sound\" + sname, dsDevice);
            if (dsDevice != null) buffer.Play(0, BufferPlayFlags.Default);
        }



        #endregion



        // 디버그 에러로그 창에 메세지를 출력합니다.
        public void AppendDebug(string msg)     //디버그용 로그
        {
            try
            {
                //메시지를 추가하고 개행을 합니다.
                txt_ErrLog.AppendText("[D] " + msg + "\r\n");
                txt_ErrLog.Focus();                          //로그상자에 포커스를 설정합니다.
                txt_ErrLog.ScrollToCaret();                  //추가로 인해 늘어난 라인까지 보여지도록 합니다.
            }
            catch (Exception ex)
            {
                txt_ErrLog.AppendText("[X]" + ex.ToString());
            }
        }

        // 화면의 대화창에 메세지를 출력합니다. 
        public void AppendMsg(string msg)
        {
            //txt_ChatWindows.AppendText(msg + "\r\n");       // 메세지 추가와 함께 개행되도록 한다. 
            //txt_ChatWindows.Focus();                        // 메세지창에 포커스를 줌 
            //txt_ChatWindows.ScrollToCaret();                // 메세지 추가된 부분까지 스크롤시켜 보여줌 
            richtxt_ChatWindows.AppendText(msg + "\r\n");       // 메세지 추가와 함께 개행되도록 한다. 
            richtxt_ChatWindows.Focus();                        // 메세지창에 포커스를 줌 
            richtxt_ChatWindows.ScrollToCaret();                // 메세지 추가된 부분까지 스크롤시켜 보여줌 
            
            txtSend.Focus();                                // 다시 입력할수 있도록 메세지 입력 상자에 포커스
        }



        // 쓰기 스트림으로 메세지를 전송합니다. 


        // 채팅
        //
        private void txtSend_KeyPress(object sender, KeyPressEventArgs e)   // 엔터를 치면 메세지를 전송합니다. 
        {
            if (e.KeyChar == '\r')                  // 엔터를 입력하였다면 
            {
                e.Handled = true;   //소리 제거?
                if (txtSend.Visible == false)
                {

                    txtSend.Focus();            //입력상자에 커서를 줍니다.
                } else
                {
                    string msg = txtSend.Text;
                    if (msg == "" || msg == null)
                    {
                        //그냥 엔터만 친거일수 있으니 보내지 않는다.
                    }
                    else
                    {
                        string msg2 = "CHATIN:" + _myID + ":" + msg;
                        CmdSendMessage(msg2);       // 서버로 메세지를 전송합니다. 
                        txtSend.Clear();            // 입력상자의 내용을 지웁니다.
                        txtSend.Focus();            //입력상자에 커서를 줍니다.
                    }
                    txtSend.Visible = false;
                }
            }
            if (e.KeyChar == '')      //컨트롤 디를 입력하면
            {
                if (txt_ErrLog.Visible == true)
                {
                    txt_ErrLog.Location = _ChatBox_Gaming_Locattion;
                    txt_ErrLog.Size = new Size(228, 400);
                    txt_ErrLog.Visible = false;
                    lbl_4_NextChoicePaeDisplay.Visible = false;// 힌트창
                }
                else
                {
                    txt_ErrLog.Visible = true;
                    txt_ErrLog.BringToFront();
                    lbl_4_NextChoicePaeDisplay.Visible = true;     // 힌트창
                }
            }
        }


        // 어플리 케이션의 쓰레드에 포함하기 위해 델리게이트 이용
        public void MessageWrite(string msg)
        {
            //받은메시지를 채팅창에 뿌려준다.
            DebugWrite("Chat:"+ msg);
            //// 소켓 쓰레드와 어플리케이션 쓰레드와 충돌되지 않도록 데리게이트 이용 
            //LogWriteDelegate deleLogWirte = new LogWriteDelegate(AppendMsg);
            //// 어플리케이션의 Invoke를 사용하여 델리게이트를 실행 
            //this.Invoke(deleLogWirte, new object[] { msg });


            //txt_ChatWindows.AppendText(msg + "\r\n");        //개행
            //txt_ChatWindows.Focus();
            //txt_ChatWindows.ScrollToCaret();
            //richtxt_ChatWindows.ForeColor = Color.Blue;
            richtxt_ChatWindows.AppendText(msg + "\r\n");
            //richtxt_ChatWindows.SelectionStart = (richtxt_ChatWindows.Text.Length - (msg.Length));
            //richtxt_ChatWindows.SelectionLength = richtxt_ChatWindows.Text.Length;
            //richtxt_ChatWindows.SelectionColor = Color.DarkGray;
            richtxt_ChatWindows.Focus();
            richtxt_ChatWindows.ScrollToCaret();
            txtSend.Focus();
        }


        /*=======================================================================
         * 
         *  게임 명령 처리 하는 부분 Cmd 모음
         * 
         *======================================================================= 
         */
         public void cmdJoinServerUser(string mid)
        {
            DeleJoinServerUser(mid);
        }
        public void cmdLeaveServerUser(string mid)
        {
            DeleLeaveServerUser(mid);
        }
        public void CmdLoginAfter(int sno)
        {
            deLoginAfter(sno);
        }
        public void CmdDelGameLevelData(string command) {
            DeleGameLevelData(command);
        }
        public void CmdLoginOK(string Command)
        {
            DeleGameLoginOK(Command);
        }
        private void CmdSendMessage(string msg2)
        {
            ///DeleGameSendMsg(msg2);// 딜리게이트 이용
            SendMessage(msg2);      //그냥보내기
        }
        public void CmdGameCLEAR(int GLevel, DateTime StartTime, int GameScore, int gGraed)
        {
            DeleGameClear(GLevel, StartTime, GameScore, gGraed);    //1. 게임시작시간에서 현재시간빼서 얼마 걸렸는지 표시하자
        }
        public void CmdChat(string nick, string msg)                //채팅일경우
        {
            // 읽기 스트림으로 부터 메세지를 읽어드림
            // 로그창에 서버로부터 받은 메세지를 추가합니다. 
            string msg2 = nick + " : " + msg;
            MessageWrite(msg2);
        }


        #region MyInitalize: 델리게이트에서 호출하는 게임을 초기화하는 실제 모듈

        private void MyInitalize(int GLevel, int gGread)
        {
            lblGameNowPoint.Text = "0";     // 점수 레이블
                                            //ClearPanel(userPan1);       // 플레이어 1이 칠 패를 보여주는 패널을 초기화 합니다. 

            // 가운데 패널(깔려진 패가 보여질)의 패가 놓여질 모든 라벨을 찾습니다.  

            //if ( 레벨1 일경우 centerPanLv1)
            // ㄹ벨 다른 경우 고쳐줘야 한다.
            //TimerGameBoardDisplay();    //타이머?

            

            if (GLevel == 1)
            {
                for (int i = 0; i < centerPanLv1.Controls.Count; i++)
                {
                    ((Label)centerPanLv1.Controls[i]).Visible = false; // 라벨이 보이지 않도록 합니다. 
                    ((Label)centerPanLv1.Controls[i]).Tag = null;  // 라벨의 Tag값을 모두 초기화 합니다.  
                    ((Label)centerPanLv1.Controls[i]).Image = null;    // 라벨의 이미지를 모두 초기화 합니다. 
                    ((Label)centerPanLv1.Controls[i]).Size = new Size(50, 68);    // 라벨의 이미지 크기를 모두 초기화 합니다. 
                }
                ShowDevidePae(centerPanLv1, _arrWholePae, "", centerPanLv1);     //오픈 패를 그린다.
                centerPanLv1.Refresh();    //패널을 다시 그립니다.
            } else if (GLevel == 2)
            {
                for (int i = 0; i < centerPanLv2.Controls.Count; i++)
                {
                    ((Label)centerPanLv2.Controls[i]).Tag = null;  // 라벨의 Tag값을 모두 초기화 합니다.  
                    ((Label)centerPanLv2.Controls[i]).Image = null;    // 라벨의 이미지를 모두 초기화 합니다. 
                    ((Label)centerPanLv2.Controls[i]).Size = new Size(50, 68);    // 라벨의 이미지 크기를 모두 초기화 합니다. 
                    ((Label)centerPanLv2.Controls[i]).Visible = false; // 라벨이 보이지 않도록 합니다. 
                }
                ShowDevidePae(centerPanLv2, _arrWholePae, "", centerPanLv2);     //오픈 패를 그린다.
                centerPanLv2.Refresh();    //패널을 다시 그립니다.
            }
            else if (GLevel == 3)
            {
                for (int i = 0; i < centerPanLv3.Controls.Count; i++)
                {
                    ((Label)centerPanLv3.Controls[i]).Tag = null;  // 라벨의 Tag값을 모두 초기화 합니다.  
                    ((Label)centerPanLv3.Controls[i]).Image = null;    // 라벨의 이미지를 모두 초기화 합니다. 
                    //((Label)centerPanLv3.Controls[i]).Click = null;    // 라벨의 이미지를 모두 초기화 합니다. 
                    ((Label)centerPanLv3.Controls[i]).Size = new Size(50, 68);    // 라벨의 이미지 크기를 모두 초기화 합니다. 
                    ((Label)centerPanLv3.Controls[i]).Visible = false; // 라벨이 보이지 않도록 합니다. 
                }
                ShowDevidePae(centerPanLv3, _arrWholePae, "", centerPanLv3);     //오픈 패를 그린다.
                centerPanLv3.Refresh();    //패널을 다시 그립니다.
            }
            else if (GLevel == 4)
            {
                for (int i = 0; i < centerPanLv4.Controls.Count; i++)
                {

                    ((Label)centerPanLv4.Controls[i]).Tag = null;  // 라벨의 Tag값을 모두 초기화 합니다.  
                    ((Label)centerPanLv4.Controls[i]).Image = null;    // 라벨의 이미지를 모두 초기화 합니다. 
                    ((Label)centerPanLv4.Controls[i]).Size = new Size(50, 68);    // 라벨의 이미지 크기를 모두 초기화 합니다. 
                    ((Label)centerPanLv4.Controls[i]).Visible = false; // 라벨이 보이지 않도록 합니다. 

                }
                ShowDevidePae(centerPanLv4, _arrWholePae, "", centerPanLv4);     //오픈 패를 그린다.
                centerPanLv4.Refresh();    //패널을 다시 그립니다.
            }


            if (!LeftPaeMoveCheck())
            {
                DeleGameOver("1", "더 없앨수 있는 패가 없습니다 !");
            }




            // 선택된 패를 나눠주는 장면을 연출합니다.
            // 가운데 판에 깔려질 패들을 깝니다. 
        }// end MyInitalize : 패돌리기 


        // 패에 이미지를 그려줍니다.
        private void ShowDevidePae(Panel inPan, PaeList paeList, string id, Panel centerPanelName)
        {


            //DebugWrite("> ShowDevidePae" + paeList.Count + "개");
            for (int i = 0; i < paeList.Count; i++)
            {
                Label lblOpen = GetRandomLabel(centerPanelName);// 중앙의 깔려질 패정보중 하나를 선택해 가져온다. 
                lblOpen.Tag = (Pae)paeList[i];      // 라벨의 Tag에 패 객체를 넣습니다. 
                //_arrPaePosition[i].Label_Name = lblOpen.Name;

                


                //global::_4000Client.Properties.Resources.sak250;

                //lblOpen.Image = global::_4000Client.Properties.Resources. .paeList[i].ResouceImageName;
                lblOpen.Image = Image.FromFile(paeList[i].ImageName);   // 라벨에 보여질 이미지 정보를 보여줍니다. 
                lblOpen.Click -= new System.EventHandler(this.myPae_Click);  //초기화 방법을 몰라서 일단 전에 넣었을거 같으니 없애준다.
                lblOpen.Click += new System.EventHandler(this.myPae_Click);  //그리고 다시 넣어준다.(2번들어가서 두번 클릭 되더라)
                //lblOpen.MouseHover -= new System.EventHandler(this.lbl_MouseOver_ChoiceCheck);    //작동실패
                //lblOpen.MouseHover += new System.EventHandler(this.lbl_MouseOver_ChoiceCheck);    //작동실패
                //lblOpen.MouseLeave -= new System.EventHandler(this.lbl_MouseOver_ChoiceCheckLeave);    //작동실패
                //lblOpen.MouseLeave += new System.EventHandler(this.lbl_MouseOver_ChoiceCheckLeave);    //작동실패
                //DebugWrite("> ShowDevidePae" + paeList[i].ImageName.ToString() + " <<");


                _arrPaePosition.AddPaePosition(new PaePo(paeList[i].sNo, lblOpen.Name, paeList[i].ImageName));
                _arrPaeLeft.AddPaeLeft(new PaeLeft(paeList[i].sNo, lblOpen.Name, paeList[i].ImageName));  //남은패 어레이에도 넣어준다.
                _arr_Clear_PaeList_MakePaeList = _arr_Clear_PaeList_MakePaeList + "|" + lblOpen.Name.ToString();//만든패 리스트에도 넣어준다. 
                //DebugWrite("paelist추가 => imgname: "+ paeList[i].ImageName);
                lblOpen.Visible = true;             // 패를 보여지도록 한다. 
                lblOpen.Refresh();                  // 다시 그려 잘 보지도록 합니다. 
                //Thread.Sleep(5);                   // 약간의 간격을 줍니다. 
            }
        }
        #region GetRandomLabel: 바닥에 깔린 먹을 패를 랜덤하게 선택해 하나의 레이블을 반환합니다.

        private Label GetRandomLabel(Panel inPanel)
        {
            Label lbl;  // 반환할 라벨을 선언합니다. 
            while (true)
            {
                Random r1 = new Random();       // 랜덤 객체를 생성합니다. 
                // 주어진 패널이 포함하고 있는 컨트롤의 갯수 안에서 임의의 수를 구합니다. 
                int cnt = r1.Next(inPanel.Controls.Count);
                // 구한 값을 인덱스로 하는 라벨의 Tag 값과 Name을 체크하여 사용하지 않는 라벨을 찾아 냅니다. 
                if (((Label)inPanel.Controls[cnt]).Tag == null && ((Label)inPanel.Controls[cnt]).Name != "")
                {
                    lbl = ((Label)inPanel.Controls[cnt]);       // 아직 사용되지 않은 라벨을 찾았다면 라벨 객체를 리턴합니다. 
                    return lbl;
                }
            }
        }
        #endregion

        #endregion
        #region GetCenter: 지정된 두 지점간 길이의 중앙점을 구해 시작된 위치를 더해 중앙점을 찾습니다.
        private Point GetCenter(Point front, Point rear)
        {
            // 중간 지점을 나타낼 포인트 선언 
            Point rp = rear;
            // X 좌표간의 중간 위치값을 구합니다. 
            rp.X = Convert.ToInt32((rp.X - front.X) / 2) + front.X;
            // Y 좌표간의 중간 위치값을 구합니다. 
            rp.Y = Convert.ToInt32((rp.Y - front.Y) / 2) + front.Y;
            // 중간 지점을 반환합니다. 
            return rp;
        }
        #endregion
        #region ShowDevidePae: 패를 돌리는 장면을 연출 합니다.


        #endregion





        //==================================
        //      게임 처음 상태로 돌리기
        //==================================
        private void GameFinishBoardClear()          //      게임 처음 상태로 돌리기
        {
            //닫기 버튼누르면

            pnl_GameEnd.Visible = false;            //게임 완료 창 숨기자
            lblGameNowPoint.Visible = false;        //게임점수판 숨긴다.

            pan_GAMEBoard1.Size = _panGameBoardLv1Size_endgame;
            pan_GAMEBoard1.Location = _panGameBoardLv1Loc_endGame;
            pan_GAMEBoard1.Visible = true; ;

            pan_GAMEBoard2.Size = _panGameBoardLv1Size_endgame;
            pan_GAMEBoard2.Location = _panGameBoardLv2Loc_endGame;
            pan_GAMEBoard2.Visible = true;
            pan_GAMEBoard3.Size = _panGameBoardLv1Size_endgame;
            pan_GAMEBoard3.Location = _panGameBoardLv3Loc_endGame;
            pan_GAMEBoard3.Visible = true;
            pan_GAMEBoard4.Size = _panGameBoardLv1Size_endgame;
            pan_GAMEBoard4.Location = _panGameBoardLv4Loc_endGame;
            pan_GAMEBoard4.Visible = true;

            //Level 1
            btn_Lavel1Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lavel1Start.Enabled = true;
            btn_EasyStart.Visible = true;      //바로시작 버튼
            btn_EasyStart.Enabled = true;      //바로시작 버튼
            btn_Lv2Easy_Start.Visible = true;
            btn_Lv2Easy_Start.Enabled = true;
            btn_Lv3Easy_Start.Visible = true;
            btn_Lv3Easy_Start.Enabled = true;
            btn_Lv4Easy_Start.Visible = true;
            btn_Lv4Easy_Start.Enabled = true;


            btn_Lv1Nor_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv1Nor_Start.Enabled = true;
            btn_Lv2Nor_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv2Nor_Start.Enabled = true;
            btn_Lv3Nor_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv3Nor_Start.Enabled = true;
            btn_Lv4Nor_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv4Nor_Start.Enabled = true;

            btn_Lv1Hard_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv1Hard_Start.Enabled = true;
            btn_Lv2Hard_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv2Hard_Start.Enabled = true;
            btn_Lv3Hard_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv3Hard_Start.Enabled = true;
            btn_Lv4Hard_Start.Visible = true;            //레벨 시작버튼 보인다..
            btn_Lv4Hard_Start.Enabled = true;

            centerPanLv1.Visible = false;            //ㄱ ㅔ임 판때기 보인다.
            centerPanLv2.Visible = false;            //ㄱ ㅔ임 판때기 보인다.
            centerPanLv3.Visible = false;            //ㄱ ㅔ임 판때기 보인다.
            centerPanLv4.Visible = false;            //ㄱ ㅔ임 판때기 보인다.



            //txt_ChatWindows.Location = new Point(10, 448);        //채팅윈도위 위치
            richtxt_ChatWindows.Location = new Point(10, 448);        //채팅윈도위 위치
            //txt_ChatWindows.Size = new Size(998, 192);  //채팅 윈도운 사이즈
            richtxt_ChatWindows.Size = new Size(998, 192);  //채팅 윈도운 사이즈

        }
        private void btn_EasyStart_Click(object sender, EventArgs e)
        {
            //바로시작 버튼 누르면
            //btn_EasyStart
            Lv001_Start(1, 1);
        }

        private void Lv001_Start(int lvClass, int lvGrade)
        {
            int LvClass = lvClass;      //1 쉬움, 중간, 어려움
            btn_Lavel1Start.Enabled = false;
            btn_Lv1Nor_Start.Enabled = false;
            btn_Lv1Hard_Start.Enabled = false;
            switch (lvGrade)
            {
                case 1:
                    btn_Lavel1Start.Visible = true;
                    btn_Lv2Easy_Start.Visible = true;     //레벨2 스타트 버튼 보이게
                    btn_Lv3Easy_Start.Visible = true;     //레벨2 스타트 버튼 보이게
                    btn_Lv4Easy_Start.Visible = true;     //레벨2 스타트 버튼 보이게

                    btn_Lv1Nor_Start.Visible = false;
                    btn_Lv1Hard_Start.Visible = false;
                    btn_Lv2Nor_Start.Visible = false;       //렙레2 스타트 버튼 보이게
                    btn_Lv2Hard_Start.Visible = false;       //렙레3 스타트 버튼 보이게
                    btn_Lv3Nor_Start.Visible = false;       //렙레2 스타트 버튼 보이게
                    btn_Lv3Hard_Start.Visible = false;       //렙레3 스타트 버튼 보이게
                    btn_Lv4Nor_Start.Visible = false;       //렙레2 스타트 버튼 보이게
                    btn_Lv4Hard_Start.Visible = false;       //렙레3 스타트 버튼 보이게

                    btn_Lavel1Start.Enabled = false;
                    btn_Lv2Easy_Start.Enabled = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv3Easy_Start.Enabled = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv4Easy_Start.Enabled = false;     //레벨2 스타트 버튼 보이게

                    break;
                case 2:
                    btn_Lavel1Start.Visible = false;
                    btn_Lv1Nor_Start.Visible = true;
                    btn_Lv1Hard_Start.Visible = false;
                    btn_Lv2Easy_Start.Visible = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv2Nor_Start.Visible = true;       //렙레2 스타트 버튼 보이게
                    btn_Lv2Hard_Start.Visible = false;       //렙레3 스타트 버튼 보이게
                    btn_Lv3Easy_Start.Visible = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv3Nor_Start.Visible = true;       //렙레2 스타트 버튼 보이게
                    btn_Lv3Hard_Start.Visible = false;       //렙레3 스타트 버튼 보이게
                    btn_Lv4Easy_Start.Visible = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv4Nor_Start.Visible = true;       //렙레2 스타트 버튼 보이게
                    btn_Lv4Hard_Start.Visible = false;       //렙레3 스타트 버튼 보이게

                    btn_Lv1Nor_Start.Enabled = false;
                    btn_Lv2Nor_Start.Enabled = false;
                    btn_Lv3Nor_Start.Enabled = false;
                    btn_Lv4Nor_Start.Enabled = false;

                    break;
                default:
                    btn_Lavel1Start.Visible = false;
                    btn_Lv1Nor_Start.Visible = false;
                    btn_Lv1Hard_Start.Visible = true;
                    btn_Lv2Easy_Start.Visible = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv2Nor_Start.Visible = false;       //렙레2 스타트 버튼 보이게
                    btn_Lv2Hard_Start.Visible = true;       //렙레3 스타트 버튼 보이게
                    btn_Lv3Easy_Start.Visible = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv3Nor_Start.Visible = false;       //렙레2 스타트 버튼 보이게
                    btn_Lv3Hard_Start.Visible = true;       //렙레3 스타트 버튼 보이게
                    btn_Lv4Easy_Start.Visible = false;     //레벨2 스타트 버튼 보이게
                    btn_Lv4Nor_Start.Visible = false;       //렙레2 스타트 버튼 보이게
                    btn_Lv4Hard_Start.Visible = true;       //렙레3 스타트 버튼 보이게

                    btn_Lv1Hard_Start.Enabled = false;       //렙레3 스타트 버튼 보이게
                    btn_Lv2Hard_Start.Enabled = false;       //렙레3 스타트 버튼 보이게
                    btn_Lv3Hard_Start.Enabled = false;       //렙레3 스타트 버튼 보이게
                    btn_Lv4Hard_Start.Enabled = false;       //렙레3 스타트 버튼 보이게
                    break;

            }

            if (LvClass == 1)
            {
                pan_GAMEBoard1.Location = _panGameBoardLv1Loc_StartGame;     //게임중일때 판위치로
                pan_GAMEBoard1.Size = _panGameBoardGameing;        //게임중일때 보드 사이즈
                pan_GAMEBoard1.Visible = true;
                pan_GAMEBoard1.BringToFront();

                //1레벨 판때기 일경우
                lbl_lv1BestResultInfo.Visible = true;       //최고점수 보여주는 라벨
                lbl_lv1BestResultInfo.Enabled = true;
                lbl_lv1BestResultInfo.BringToFront();
                //lbl_lv1BestResultInfo.Text = "레벨 기록 데이타 불러오는중..."; //게임 베스트 표시해주기
                centerPanLv1.Visible = true;            //ㄱ ㅔ임 판때기 보인다.


                //나머지 레벨의 패널들을 숨긴다.
                pan_GAMEBoard2.Visible = false;     //레벨2 패널
                pan_GAMEBoard3.Visible = false;     //레벨3 패널
                pan_GAMEBoard4.Visible = false;     //레벨3 패널
                pnl_GameEnd.Visible = false;     //게임종료 결과판 보이기

                centerPanLv1.Size = _panCenterPanLv1Size_Startgame;        //센타 게임플레이판 위치시킨다.
                centerPanLv1.Location = _panCenterPanLv1Location_Startgame; //센타 게임판 위치 수정
                button_GameClose.Visible = true;    //게임 종료 버튼 보여주자

            }
            else if (lvClass == 2)
            {
                pan_GAMEBoard2.Location = _panGameBoardLv1Loc_StartGame;    //게임중일때 판위치로
                pan_GAMEBoard2.Size = _panGameBoardGameing;         //게임중일때 보드 사이즈
                pan_GAMEBoard2.Visible = true;
                pan_GAMEBoard2.BringToFront();

                lbl_lv2BestResultInfo.Visible = true;       //최고점수 보여주는 라벨
                lbl_lv2BestResultInfo.Enabled = true;
                lbl_lv2BestResultInfo.BringToFront();
                centerPanLv2.Visible = true;            //ㄱ ㅔ임 판때기 보인다.

                //나머지 레벨의 패널들을 숨긴다.
                pan_GAMEBoard1.Visible = false;
                pan_GAMEBoard3.Visible = false;     //레벨2 패널
                pan_GAMEBoard4.Visible = false;     //레벨2 패널
                button_GameClose2.Visible = true;    //게임 종료 버튼 보여주자

            }
            else if (lvClass == 3)
            {
                pan_GAMEBoard3.Location = _panGameBoardLv1Loc_StartGame;    //게임중일때 판위치로
                pan_GAMEBoard3.Size = _panGameBoardGameing;         //게임중일때 보드 사이즈
                pan_GAMEBoard3.Visible = true;
                pan_GAMEBoard3.BringToFront();

                lbl_lv3BestResultInfo.Visible = true;       //최고점수 보여주는 라벨
                lbl_lv3BestResultInfo.Enabled = true;
                lbl_lv3BestResultInfo.BringToFront();

                centerPanLv3.Visible = true;            //ㄱ ㅔ임 판때기 보인다.
                //나머지 레벨의 패널들을 숨긴다.
                pan_GAMEBoard1.Visible = false;
                pan_GAMEBoard2.Visible = false;     //레벨2 패널
                pan_GAMEBoard4.Visible = false;     //레벨2 패널
                button_GameClose3.Visible = true;    //게임 종료 버튼 보여주자


            }
            else if (lvClass == 4)
            {
                pan_GAMEBoard4.Location = _panGameBoardLv1Loc_StartGame;    //게임중일때 판위치로
                pan_GAMEBoard4.Size = _panGameBoardGameing;         //게임중일때 보드 사이즈
                pan_GAMEBoard4.Visible = true;
                pan_GAMEBoard4.BringToFront();

                lbl_lv4BestResultInfo.Visible = true;       //최고점수 보여주는 라벨
                lbl_lv4BestResultInfo.Enabled = true;
                lbl_lv4BestResultInfo.BringToFront();

                centerPanLv4.Visible = true;            //ㄱ ㅔ임 판때기 보인다.
                //나머지 레벨의 패널들을 숨긴다.
                pan_GAMEBoard1.Visible = false;
                pan_GAMEBoard2.Visible = false;     //레벨2 패널
                pan_GAMEBoard3.Visible = false;     //레벨2 패널
                button_GameClose4.Visible = true;    //게임 종료 버튼 보여주자

            }
            lblGameNowPoint.Visible = true;     //게임점수판
            lblGameNowPoint.Text = "0";         //게임점수판
            lblGameNowPoint.BringToFront();     //게임점수판
            lblGameNowPoint.Refresh();          //게임 점ㅁ수판 (배경묻어나와서 먼저 그려보자)


            //게임패널 초기위치 5,32, 사이즈 1006,615
            btn_EasyStart.Visible = false;      //바로시작 버튼 안보여주기
            //txt_ChatWindows.Location = _ChatBox_Gaming_Locattion;        //채팅윈도위 위치
            //txt_ChatWindows.Size = new Size(232, 378);  //채팅 윈도운 사이즈
            //txt_ChatWindows.BringToFront();

            richtxt_ChatWindows.Location = _ChatBox_Gaming_Locattion;        //채팅윈도위 위치
            richtxt_ChatWindows.Size = new Size(232, 378);  //채팅 윈도운 사이즈
            richtxt_ChatWindows.BringToFront();
            
                
            GameReady(lvClass, lvGrade);    //게임을 준비해준다.

            lbl_PaePosition.Visible = false;        //디버깅 표시 라벨을 안보이게 한다.

            AppendMsg("================= GAME START !!!! =================");
            AppendMsg("=  게임머니  : -" + ((lvClass * 100) * lvGrade) + "랭 ");
            AppendMsg("===================================================");
            _GameStartTime = DateTime.Now;          //시작시간을 넣어준다.

            _myP_cash = _myP_cash  - ((lvClass * 100) * lvGrade);       //게임시작비용 차감
            lbl_MyPcashlDisplay.Text = _myP_cash.ToString("#,##0") +" 랭";//게임시작비용 차감 표시
            _myP_all = _myP_all + 1;//전적 올려주기
            lbl_MyPallDisplay.Text = _myP_all.ToString("#,##0") + " 전";       //전적 올려주기 표시

        }


        private void button3_Click(object sender, EventArgs e)  //게이미 종료 버튼 누르면
        {
            GameFinishBoardClear();     //게임판 정리하자
        }

        // 전체 패 리스트를 관리 합니다. 
        PaeList _arrWholePae;
        PaePosition _arrPaePosition;
        PaeLeftList _arrPaeLeft;

        public void GameReady(int GLevel, int GGrade) //게임을 준비한다.
        {
            _GameStartTime = DateTime.Now;              //시간을 리셋한다.
            _LastRemovePaeStopWatch = DateTime.Now;
            _myGameScore = 0;                   //점수를 0으로 만들어준다.
            _arrWholePae = new PaeList();
            _arrWholePae.RemoveAllPae();
            _arrPaeLeft = new PaeLeftList();
            _arrPaeLeft.RemoveAllPaeLeft();
            _arrPaePosition = new PaePosition();
            _arrPaePosition.RemoveAllPaePosition();
            ServerSend_CmdGameStart(GLevel, GGrade);            //게임 시작을 서버에 알린다.
            MakePae(GLevel, GGrade);      //패 어레이를 생성한다.
            DelePaeInit(GLevel, GGrade);    //MyInitalize(GLevel, GGrade) 의 딜레겡트버전
            //MyInitalize(GLevel, GGrade);
        }

        #region 모든패 생성    
        private void MakePae(int GLevel, int GGrade) //패를 생성한다.
        {
            _arr_Clear_PaeList = "";        //없앤패 리스트를 초기화한다.
            _arr_Clear_PaeList_MakePaeList = "";    //만든패 리스트를 초기화한다.
                                                    //선택한 패도 초기화하자
            _firstChoicePaeName = "";            //첫번째로 선택한 패의 라벨 네임입력
            _fristChoiceLabl = null;                     //첫번째 선택한 라벨
            _fristChoiceINDEXsNo = 0;                     //첫번째 선택한 라벨의 인덱스

            switch (GGrade)
            {
                case 1:
                    PaeSameCount = 8;
                    break;
                case 2:
                    PaeSameCount = 6;
                    break;
                case 3:
                    PaeSameCount = 4;
                    break;
                default:
                    PaeSameCount = 8;
                    break;
            }
            //  if (_PaeCenterPanArrangePositionStyle == 1) PaeSameCount = PaeSameCount + 2;    //난이도 조종용

            //PaeNUMTEMP = 1;
            //판때끼에 있는 라벨갯수를 가져오자
            //centerPanLv1 에서 라벨 갯수
            int PaeCount = 0;
            if (GLevel == 1) PaeCount = centerPanLv1.Controls.Count; //(12x6=72) 센타판에 있는 패의 갯수를 가져온다.
            else if (GLevel == 2) PaeCount = centerPanLv2.Controls.Count;
            else if (GLevel == 3) PaeCount = centerPanLv3.Controls.Count;
            else if (GLevel == 4) PaeCount = centerPanLv4.Controls.Count;
            //else if (GLevel == 3) PaeCount = centerPanLv3.Controls.Count;
            //else if (GLevel == 3) PaeCount = centerPanLv3.Controls.Count;
            //else if (GLevel == 3) PaeCount = centerPanLv3.Controls.Count;

            // else..... 레벨 갯수만큼


            //8 * 16;      //판대기에 있는 패갯수 70 + 42 = 112 / 2 = 56 /2 = 28 ( 28 *4 = 112)
            int PaeFor4 = PaeCount / PaeSameCount; //8x16 = 128 / 6 = 
            int nowpae9Count = 1;
            int paetype = 1;
            int ExtraPaeAdd = 0;
            int PaedModCount = PaeCount % PaeSameCount;  //나머지 갯수를 찾자
            _GAme_LeftPae_Count = PaeCount;     //패갯수 넣어주자

            DebugWrite("만들패갯수는 : " + PaeCount);
            DebugWrite("몇개씩? : " + PaeSameCount);
            DebugWrite("나머지는? : " + PaedModCount);

            for (int i = 1; i <= PaeFor4; i++)
            {
                MakeSamepae(i, nowpae9Count, paetype, PaeSameCount); //같은패를 여러개 만들어준다.
                if (nowpae9Count == 9) {
                    nowpae9Count = 1;
                    paetype++;
                }
                else nowpae9Count++;            //만든패갯수 1+
            }
            //패갯수가 4,6,8의 배수에서 6일경우 갯수가 2개 남아서. 이렇게 했다.
            if (PaedModCount != 0)
            {
                // PaedModCount 만큼 추가 패를 만들어 줘야 한다.
                //int sMakeCountLast = PaeCount - PaedModCount - 1;
                int sMakeCountLast = _arrWholePae.Count;
                for (int x = 1; x <= PaedModCount; x++)
                {
                    _arrWholePae.AddPae(new Pae(sMakeCountLast, 1, PaeType.pun));
                    sMakeCountLast++;
                    //난이도를 위해서 중복패로 만들어 준다. pun
                    DebugWrite("패만들기(" + sMakeCountLast + "):-[ pun"+ 1 + "50.png / 추가:");          //디버거다
                }
            }
            nowpae9Count = 1;
        }

        public void MakeSamepae(int ino, int PaeNum, int paetype, int SamePaeMakeCount)
        {
            // SamePaeMakeCount = 8,6,4, 6일경우 ExtraAddPae =2
            //======================================================
            //  받은갯수만큼 같은패를 만들어준다.
            //======================================================
            for (int j = 1; j <= SamePaeMakeCount; j++)
            {
                int sno = (((ino * SamePaeMakeCount) - SamePaeMakeCount) + (j - 1));      //시리얼 넘버 만들자
                                                                                          // i 에다가 같은패 만들기 (8) 곱하고 첫번째를 0으로 만들기위해서 8빼주고, (j-1) 하면 0으로 시작
                if (paetype == 1) _arrWholePae.AddPae(new Pae(sno, PaeNum, PaeType.pun));
                else if (paetype == 2) _arrWholePae.AddPae(new Pae(sno, PaeNum, PaeType.ton));
                else if (paetype == 3) _arrWholePae.AddPae(new Pae(sno, PaeNum, PaeType.sak));
                else if (paetype == 4) _arrWholePae.AddPae(new Pae(sno, PaeNum, PaeType.man));
                else if (paetype == 5) _arrWholePae.AddPae(new Pae(sno, PaeNum, PaeType.won));
                //DebugWrite("패만들기(" + (sno) + "):-[" + paetype + "]" + PaeNum.ToString() + "50.png / ");          //디버거다
                //PaeNUMTEMP++;
            }
        }
        #endregion

    

        //로그인창에 웹브라우저 로딩 완료 되었을때 실행
        private void webBrowser_Login_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string IPadd = "";
            this.Text = webBrowser_Login.Document.Title;
            HtmlElement elem;
            if (webBrowser_Login.Document != null)
            {
                HtmlElementCollection elems = webBrowser_Login.Document.GetElementsByTagName("HTML");
                if (elems.Count == 1)
                {
                    elem = elems[0];
                    string pageIP = elem.OuterHtml;
                    int stiptage = pageIP.IndexOf("<IP>");
                    pageIP = pageIP.Substring(stiptage + 4, (pageIP.IndexOf("</IP>") - (stiptage + 4)));
                    RankingServerIP = pageIP;
                    DebugWrite(">>IP : [ " + pageIP + " ]");

                    if (DebugPanelVisible == 1) txt_ErrLog.Visible = true;     //디버거 출력창
                    if (LocalIPUse == 2)  txtIP.Text = RankingServerIP;
                    else
                    {
                        // 현재 컴퓨터의 아이피를 초기값으로 정합니다. 
                        IPHostEntry localHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                        // 현재 컴퓨터에서 얻은 IP 값중 첫번째 값을 기본IP 값으로 세팅해 줍니다. 
                        txtIP.Text = localHostEntry.AddressList[0].ToString();
                    }

                }
            }
            else
            {
                RankingServerIP = "182.162.141.101";        //아이피를 입력해준다.
                DebugWrite(">>IP : 못가져왔다");
                txt_ErrLog.Visible = true;
            }
        }

        private void btn_Lv4_Click(object sender, EventArgs e)
        {
            Lv4 GameFormLv4 = new Lv4();
            //GameFormLv4.Visible = true;
            //GameFormLv4.Dock = DockStyle.Top;
            //GameFormLv4.TopLevel = false;
            //GameFormLv4.MdiParent = this;
            //GameFormLv4.Show();
            //GameFormLv4.Activate();
            //GameFormLv4.TopMost = true;
            //GameFormLv4.BringToFront();
            //GameFormLv4.ShowDialog();
            GameFormLv4.Show();
            GameFormLv4.ShowInTaskbar = false;
        }

        private void GameLOSE_CloseButton(object sender, EventArgs e)   //게임 실패로 창닫기 버튼
        {
            //GameLOSE_CloseButton
            GameFinishBoardClear();     //게임판 정리하자
        }

        #region 게임 스타트 버튼들              //게임판마다 ㅏ레벨별 스타트 버튼 모음
        private void btn_Lavel1Start_Click(object sender, EventArgs e)  //Lv1 easy LEVEL     //LV1 시작버튼
        {
            _NowPlayGameLevel = 1;
            _NowPlayGameGrade = 1;
            _PaeCenterPanArrangePositionStyle = 0;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }
        private void button5_Click(object sender, EventArgs e)      //Lv1 Normal LEVEL
        {
            _NowPlayGameLevel = 1;
            _NowPlayGameGrade = 2;
            _PaeCenterPanArrangePositionStyle = 0;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void button6_Click(object sender, EventArgs e)      //Lv1 Hard LEVEL
        {
            _NowPlayGameLevel = 1;
            _NowPlayGameGrade = 3;
            _PaeCenterPanArrangePositionStyle = 0;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }
        private void btn_Lv2Easy_Start_Click(object sender, EventArgs e)    //2-1 레벨 스타트
        {
            _NowPlayGameLevel = 2;
            _NowPlayGameGrade = 1;
            _PaeCenterPanArrangePositionStyle = 0;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void btn_Lv2Nor_Start_Click(object sender, EventArgs e) //2-2 레벨 스타트
        {
            _NowPlayGameLevel = 2;
            _NowPlayGameGrade = 2;
            _PaeCenterPanArrangePositionStyle = 0;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void btn_Lv2Hard_Start_Click(object sender, EventArgs e)//2-3 레벨 스타트
        {
            _NowPlayGameLevel = 2;
            _NowPlayGameGrade = 3;
            _PaeCenterPanArrangePositionStyle = 0;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void button5_Click_1(object sender, EventArgs e)    //3레벨 이지 스타트
        {
            _NowPlayGameLevel = 3;
            _NowPlayGameGrade = 1;
            _PaeCenterPanArrangePositionStyle = 1;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void button4_Click(object sender, EventArgs e)  //3레벨 노말 스타트
        {
            _NowPlayGameLevel = 3;
            _NowPlayGameGrade = 2;
            _PaeCenterPanArrangePositionStyle = 1;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void button3_Click_1(object sender, EventArgs e)    //3레벨 하드 스타트
        {
            _NowPlayGameLevel = 3;
            _NowPlayGameGrade = 3;
            _PaeCenterPanArrangePositionStyle = 1;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }
        private void btn_Lv4Easy_Start_Click(object sender, EventArgs e)    //4레벨 이지 스타트
        { 
            _NowPlayGameLevel = 4;
            _NowPlayGameGrade = 1;
            _PaeCenterPanArrangePositionStyle = 1;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void btn_Lv4Nor_Start_Click(object sender, EventArgs e)     //4레벨 너밀 스타트
        {
            _NowPlayGameLevel = 4;
            _NowPlayGameGrade = 2;
            _PaeCenterPanArrangePositionStyle = 1;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }

        private void btn_Lv4Hard_Start_Click(object sender, EventArgs e)        //4레벨 하드 스타트
        {
            _NowPlayGameLevel = 4;
            _NowPlayGameGrade = 3;
            _PaeCenterPanArrangePositionStyle = 1;
            Lv001_Start(_NowPlayGameLevel, _NowPlayGameGrade);
        }
        #endregion  게임 스타트 버튼들

       
        private void lbl_MouseOver_ChoiceCheck(object sender, EventArgs e)
        {
            //마우스 올렸을때 움직일수 있는 패인지 체크후 표시해준다.
                // 패를 치기 위해 선택한 라벨로 부터 패정보를 가져옮니다. 
                Pae thisPae = ((Pae)((Label)sender).Tag);
                //PaePo thisPaePo = ((PaePo)((Label)sender).Tag);
                Label thisLabelPae = ((Label)sender);      //움직일수 있는 패인지 체크한다.
                if (CheckChoicePassblePaeHEXA(thisPae, thisLabelPae))
                {
                //패를 살짝 움직여준다.
                Point beforLocation = new Point(thisLabelPae.Location.X - 3, thisLabelPae.Location.Y - 3);
                thisLabelPae.Location = beforLocation;      //3포인트씩 이동해준다.
                }
        }
        private void lbl_MouseOver_ChoiceCheckLeave(object sender, EventArgs e) //패에 마우스 가따대따  댓을때
        {
            Label thisLabelPae = ((Label)sender);      
            Point beforLocation = new Point(thisLabelPae.Location.X + 3, thisLabelPae.Location.Y + 3);
            thisLabelPae.Location = beforLocation;      //3포인트씩 이동해준다.
        }

        private void lblWarringUP_Hard(object sender, EventArgs e)  //하드 레벨에 마우스 가따댔을때...
        {
            lblWarringUP_HardTEXT.Visible = true;
            lblWarringUP_HardTEXT.Text = "< 심약자 클릭 금지!! > \r\n\r\n못깰 확률이 높으며 가끔가다가 시작하자 마자 끝날때도 있습니다.\r\n(극악 난이도)";
        }

        private void lblWarringLeave_Hard(object sender, EventArgs e)
        {
            //하드레벨 마우스 내렷을때
            lblWarringUP_HardTEXT.Visible = false;
        }

        private void panTitle_MouseDownInPnl(object sender, MouseEventArgs e)   // 마우스를 이동하기 시작한 위치를 구합니다. 
        {
            //안쪽 패널 움익이기 nPnl
            ptRect2.X = e.X;
            ptRect2.Y = e.Y;
        }

        private void panTitle_MouseMoveInPnl(object sender, MouseEventArgs e)
        {
            //안쪽 패널 움익이기 nPnl
            // 타이틀에서 마우스의 왼쪽 버튼을 클릭 했을때의 구현입니다.
            if (e.Button == MouseButtons.Left)
            {
                // 현재 위치로 부터 이벤트가 발생한 위치까지를 계산합니다.
                Point pt = new Point(pnl_GameEnd.Location.X + e.X - ptRect2.X, pnl_GameEnd.Location.Y + e.Y - ptRect2.Y);
                // 마우스에 의해 이동된 위치까지 이동 시킵니다. 
                pnl_GameEnd.Location = pt;
            }
        }

        public int GetSubStringEnHanAllSize(string title) // 한글2,영문1 글자수
        {
            //한글(2)과 영문(1) 총 글자수를 돌려준다
            byte[] ch = Encoding.Default.GetBytes(title);
            int EngINt = 0;

            if (title.Trim().Length == 0) EngINt = 0;
            else
            {
                for (int i = 0; i < ch.Length; i++)
                {
                    if (ch[i] < 128)  EngINt = EngINt + 1;
                    else
                    {
                        EngINt = EngINt + 2;
                        i++;
                    }
                }
            }
            return EngINt;
        }

        private void txtSend_KeyPressChatDataWindows(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                txtSend.Visible = true;
                txtSend.Focus();
            }
            if (e.KeyChar == '')      //컨트롤 디를 입력하면
            {
                if (txt_ErrLog.Visible == true)
                {
                    txt_ErrLog.Visible = false;
                    lbl_4_NextChoicePaeDisplay.Visible = false;
                    lbl_4_FirstChoicePaeDisplay.Visible = false;
                    lbl_firstchoecePaename.Visible = false;
                    lbl_NextchoecePaename.Visible = false;
                }
                else
                {
                    txt_ErrLog.Location = _ChatBox_Gaming_Locattion;
                    txt_ErrLog.Size = new Size(228, 400);
                    txt_ErrLog.Visible = true;
                    txt_ErrLog.BringToFront();
                    lbl_4_NextChoicePaeDisplay.Visible = true;
                    lbl_4_FirstChoicePaeDisplay.Visible = true;
                    lbl_firstchoecePaename.Visible = true;
                    lbl_NextchoecePaename.Visible = true;
                }
            }
        }

        private void txt_PWD_KeyDown(object sender, KeyEventArgs e)     //패스워드 텍스트 박스에서 엔터 쳤을때
        {
            if (e.KeyCode == Keys.Enter)  btnConn.PerformClick();
        }

       

        private void userinfo(object sender, MouseEventArgs e)
        {
            // 리스트뷰에서 마우스 클릭시
            if(listViewUserList.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listViewUserList.SelectedItems;
                ListViewItem LvItem = items[0];
                _UserInfoID = LvItem.Text;
                UserInfo u2 = new UserInfo();
                //u2.ShowDialog();
                u2.Show();
                u2.BringToFront();
                //userinfo frm_userinfo = new userinfo();
                //userinfo.Show();

            }
        }

        private void Ranking4000Sung_FormClosed(object sender, FormClosedEventArgs e)
        {
            //메인폼 닫았을때
            //서버로 종료 메시지 보내주자
            Application.Exit();
        }
    }
}
