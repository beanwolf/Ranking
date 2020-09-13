using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4000Client
{
    public partial class UserInfo : Form
    {
        public string userinfoID = "";
        public UserInfo()
        {
            InitializeComponent();
            
            
                userinfoID =  _4000Client.Ranking4000Sung._UserInfoID;
            uinfo_id.Text = userinfoID+ "님의 상세정보";
            this.Text = userinfoID + "님 정보 보기";
            UriBuilder builder = new UriBuilder("https://www.ranking.co.kr/Account/AvataDisplay?myid=" + userinfoID);
            string url = builder.ToString();
            webBrowser_UserAVATA.Navigate(url); //내아바타불러온다.
            UriBuilder builder2 = new UriBuilder("https://www.ranking.co.kr/Game/ShangHaiGameInfo?myid=" + userinfoID);
            string url2 = builder2.ToString();
            webBrowser1.Navigate(url2);
            UriBuilder builder3 = new UriBuilder("https://www.ranking.co.kr/Game/ShangHaiGameResult?myid=" + userinfoID);
            string url3 = builder3.ToString();
            webBrowser2.Navigate(url3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //창닫기 버튼
            this.Close();
        }
    }
}
