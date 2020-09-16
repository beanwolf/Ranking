using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectSound;

namespace DXClient
{
    public partial class Main : Form
    {
        //private Main _formMain = null;
        public Main()
        {
            InitializeComponent();
            this.ClientSize = new Size(1280, 700);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //private Microsoft.DirectX.Direct3D.Device dx_device = null;
 


        //public bool initializeApplication(Main topoLevelForm) {
        //    this._formMain = topoLevelForm;
        //    //메인 폼 로드
        //    PresentParameters pp = new PresentParameters();
        //    //윈도우모드 true,  풀스크린 모드라면 false 지정
        //    pp.Windowed = true;
        //    //스왑효과 우선.discard
        //    pp.SwapEffect = SwapEffect.Discard;

        //    //최고의 포퍼먼스로 동작시도 실패하면 하위 퍼포먼스로 동작
        //    try
        //    {
        //        dx_device = new Microsoft.DirectX.Direct3D.Device(0, DeviceType.Hardware,
        //                        topoLevelForm.Handle,
        //                        CreateFlags.HardwareVertexProcessing, pp);

        //    }
        //    catch(DirectXException ex1)
        //    {
        //        //하드웨어 가속 실패시..
        //        Debug.WriteLine(ex1.ToString());
        //        try
        //        {
        //            //소프트웨어에 의한정점 처리
        //            dx_device = new Microsoft.DirectX.Direct3D.Device(0,
        //                       DeviceType.Hardware,
        //                       topoLevelForm.Handle,
        //                       CreateFlags.SoftwareVertexProcessing,
        //                       pp);
        //        }catch(DirectXException ex2)
        //        {
        //            //동작 실패시
        //            Debug.WriteLine(ex2.ToString());
        //            try
        //            {
        //                //소프ㅡ웨어에 의한 정점 처리  실시
        //                //매우 낮은 퍼포먼스 
        //                //거의 대부분의 처리를 제한 없이 동작시킬수 있습니다.
        //                dx_device = new Microsoft.DirectX.Direct3D.Device(0,
        //                       DeviceType.Reference,
        //                       ttopoLevelForm.Handle,
        //                       CreateFlags.SoftwareVertexProcessing,
        //                       pp);
        //            }
        //            catch (DirectXException ex3)
        //            {
        //                //동작실패
        //                //디바이스를 동작시킬수 없습니다.
        //                MessageBox.Show(ex3.ToString(), "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return false;

        //                throw;
        //            }
        //        }
        //    }


        //    s = new Sprite(dx_device);
        //    //택스쳐 불러오기( 2:파일이름, 3:이미지가로크기, 4:이미지 세로크기)
        //   // dx_texture = TextureLoader.FromFile(dx_device, "OneSizeAllPae.jpg", 720, 547, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
        //    dx_texture = TextureLoader.FromFile(dx_device, "OneSizeAllPae.jpg", 720, 547, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0); 
        //}

        //private void Main_Paint(object sender, PaintEventArgs e)
        //{
        //    dx_device.Clear(ClearFlags.Target, Color.Blue, 1.0f, 0);
        //    dx_device.BeginScene();

        //    s.Begin(SpriteFlags.AlphaBlend);

        //    //Draw2D 함수의 인수는 차례로, Texture, 회전 시 중심점, 회전각, 그릴 위치, 색채(?) 입니다
        //    s.Draw2D(dx_texture, new Point(0, 0), 0f, new Point(0, 0), Color.White);
        //    s.End();

        //    dx_device.EndScene();
        //    dx_device.Present();
        //}



        //private Sprite s = null;
        //private Texture dx_texture = null;
    }
}
