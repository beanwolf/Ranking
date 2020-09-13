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
    public partial class Lv4 : Form
    {
        public Lv4()
        {
            InitializeComponent();
        }

        private void button_ExitGame_Click(object sender, EventArgs e)
        {
            this.Close();
            //Ranking4000Sung F1 = new Ranking4000Sung();
            this.Location = new Point(0, 10);
        }

       

        #region 이벤트 처리 모듈 마우스 이동,호버 사운드 (panTitle_MouseMove, panTitle_MouseDown,btn_mouse_hoverSound)
        // 전역 변수로 포인터를 선언 합니다. 
        public Point ptRect = new Point(0, 0);
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
        #endregion



    }
}
