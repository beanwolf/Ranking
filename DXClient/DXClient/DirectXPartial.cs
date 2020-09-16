using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.DirectX;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
    

namespace DXClient
{

    /// <summary>
    /// 
    /// 
    /// 
    /// 
    /// CreateFont()// 폰트의 작성
    /// RenderSquarePolygon()
    ///  CreateSquarePolygon()
    /// CreateDevice(Main topLevelForm, PresentParameters presentationParameters)// Direct3D 디바이스의 작성
    /// SettingCamera()
    /// form_MouseMove(object sender, MouseEventArgs e)//마우스 이동 이벤트
    /// form_KeyDown(object sender, KeyEventArgs e)    // 키보드의 키를 누른 순간
    /// 
    /// 
    /// 
    /// </summary>
    /// 

    public partial class MainSample
    {
        //public partial class DirectPartialCode : UserControl
        /// <summary>
        /// 메인 폼
        /// </summary>
        private Main _form = null;

        /// <summary>
        /// Direct3D 디바이스
        /// </summary>
        private Device _device = null;

        /// <summary>
        ///     Direct3D용 폰트
        /// </summary>
        private Microsoft.DirectX.Direct3D.Font _font = null;
        private Microsoft.DirectX.Direct3D.Font _font16 = null;



        /// <summary>
        /// 카메라 렌즈의 위치(R)
        /// </summary>
        private float _lensPosRadius = 10.0f;

        /// <summary>
        /// 카메라 렌즈의 위치(θ)
        /// </summary>
        private float _lensPosTheta = 300.0f;

        /// <summary>
        /// 카메라 렌즈의 위치(φ)
        /// </summary>
        private float _lensPosPhi = 30.0f;




        /// <summary>
        /// Direct3D 디바이스의 작성
        /// </summary>
        /// <param name="topLevelForm">톱 레벨 윈도우</param>
        /// 


        ///<summary>
        /// 키보드의 키를 누른 순간
        ///</summary>
        ///
        private void form_KeyDown(object sender, KeyEventArgs e)    // 키보드의 키를 누른 순간
        {
            //눌러진 키보드의 플래그를 참값으로 세팅
            if ((int)e.KeyCode < this._keys.Length)
            {
                this._keys[(int)e.KeyCode] = true;
            }
        }

        ///<summary>
        /// 키보드의 키를 놓는 순간
        ///</summary>
        private void form_KeyUp(object sender, KeyEventArgs e)
        {
            //놓은 키보드의 플래그를 거짓값 으로 세팅
            if ((int)e.KeyCode < this._keys.Length)
            {
                this._keys[(int)e.KeyCode] = false;
            }
        }

        private void CreateDevice(Main topLevelForm)
        {
            // PresentParameters.디바이스를 작성할 때에 필수
            // 어떠한 환경에서 디바이스를 사용하는지를 설정한다
            PresentParameters pp = new PresentParameters();

            // 윈도우 모드라면 true, 풀 스크린 모드라면 false 를 지정
            pp.Windowed = true;

            // 스왑 효과.우선 「Discard」를 지정.
            pp.SwapEffect = SwapEffect.Discard;

            //심도 스텐실 ㅣ버퍼.3d에서는 깊이가 있으므로 통상 true
            pp.EnableAutoDepthStencil = true;

            //자동 심도 스텐실 서페이스의 포멧.
            pp.AutoDepthStencilFormat = DepthFormat.D16;

            try
            {
                // 디바이스의 작성
                this.CreateDevice(topLevelForm, pp);
            }
            catch (DirectXException ex)
            {
                // 예외 발생
                throw ex;
            }
        }


        
        //키보드 인풋 이벤트
        private void CreateInputEvent(Form toplevelForm)        //키 이벤트 작성
        { 
            toplevelForm.KeyDown += new KeyEventHandler(this.form_KeyDown);
            toplevelForm.KeyUp += new KeyEventHandler(this.form_KeyUp);
        }



        ///<summary>
        /// 폰트의 작성
        /// </summary>
        private void CreateFont()// 폰트의 작성
        {
            try
            {
                // 폰트 데이터의 구조체를 작성
                FontDescription fd = new FontDescription();
                FontDescription fd16 = new FontDescription();

                // 구조체에 필요한 데이터를 세트
                fd.Height = 24;
                fd.FaceName = "ＭＳ고딕";
                fd16.Height = 16;
                fd16.FaceName = "ＭＳ고딕";

                // 폰트를 작성
                this._font = new Microsoft.DirectX.Direct3D.Font(this._device, fd);
                this._font16 = new Microsoft.DirectX.Direct3D.Font(this._device, fd16);
            }
            catch (DirectXException ex)
            {
                // 예외 발생
                throw ex;
            }
        }

        private void RenderSquarePolygon()
        {
            //텍스쳐 세팅
            this._device.SetTexture(0, this._texture);


            //정점 버프를 디바이스의 데이터 스트림에 바인드
              this._device.SetStreamSource(0, this._vertexBuffer, 0);

            //그릴 려는 정점의 포멧을 세트
            ///================================================================
            /// <summary>
            //this._device.VertexFormat = CustomVertex.TransformedColored.Format;       //삼각형
            //this._device.VertexFormat = CustomVertex.PositionColored.Format;          //3D상의 삼각형
            this._device.VertexFormat = CustomVertex.PositionTextured.Format;            //텍스쳐 포함
            ///     CustomVertex.TransformedColored.Format; format 에러가 나서 참조에 microsoft.visualC 어셈블리 추가했다.
            /// </summary>
            ///================================================================

            //랜더링(그리기)(TriangleList, 0, 1 - > TriangleStrip, 0, 2)
            //this._device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, this._vertices);
            //this._device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            this._device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);     //2개의 삼각형? = 사각형


          
        }
        private void CreateSquarePolygon()      //사각형 그리기
        {
            //삼각형의 다각형을 표시하기 위한 정점 버퍼를 작성
            //this._vertexBuffer = new VertexBuffer(typeof(CustomVertex.TransformedColored),3, this._device, Usage.None, CustomVertex.TransformedColored.Format, Pool.Managed);
            //TransformedColored => PositionColored (3D 상에서 아직 포인트가 없기 때문에?)
            this._vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured),4, this._device, Usage.None, CustomVertex.PositionTextured.Format, Pool.Managed);

            //3점의 정보를 보관하기 위한 메모리를 확보
            //TransformedColored => PositionColored , 3,thi -> 4,thi
            //CustomVertex.PositionColored[] vertices = new CustomVertex.PositionColored[3];        //3각형
            //4점의 정보를 보관하기 위한 메모리를 확보
            CustomVertex.PositionTextured[] vertices = new CustomVertex.PositionTextured[4];    //사각형

            ////정점 데이터의설정
            //(X 3D 공간상위치X, Y, Z, 정점의 색)
            vertices[0] = new CustomVertex.PositionTextured(
                -4.0f, 4.0f, 0.0f, 0.0f, 0.0f);
            vertices[1] = new CustomVertex.PositionTextured(
                4.0f, 4.0f, 0.0f, 1.0f, 0.0f);
            vertices[2] = new CustomVertex.PositionTextured(
                -4.0f, -4.0f, 0.0f, 0.0f, 1.0f);
            vertices[3] = new CustomVertex.PositionTextured(
                4.0f, -4.0f, 0.0f, 1.0f, 1.0f);


           
            //텍스쳐 작성
            this._texture = TextureLoader.FromFile(this._device, "OneSizeAllPae.jpg");
        }

        /// <summary>
        /// Direct3D 디바이스의 작성
        /// </summary>
        /// <param name="topLevelForm">톱 레벨 윈도우</param>
        /// <param name="presentationParameters">PresentParameters 구조체</param>
        private void CreateDevice(Main topLevelForm, PresentParameters presentationParameters)// Direct3D 디바이스의 작성
        {
            // 실제로 디바이스를 작성합니다.
            // 항상 최고의 퍼포먼스로 동작을 시도해
            // 실패하면 하위 퍼포먼스로 동작하도록 하고 있다.
            try
            {
                // 하드웨어에 의한 정점 처리, rasterize를 실시한다
                // 최고의 퍼포먼스로 처리를 실행할 수 있습니다.
                // 비디오 카드에 따라서는 실행 될 수 없는 처리가 존재합니다.
                this._device = new Device(0, DeviceType.Hardware, topLevelForm.Handle,
                    CreateFlags.HardwareVertexProcessing, presentationParameters);
            }
            catch (DirectXException ex1)
            {
                // 동작 실패
                Debug.WriteLine(ex1.ToString());
                try
                {
                    // 소프트웨어에 의한 정점 처리, 하드웨어에 의한 rasterize를 실시한다
                    this._device = new Device(0, DeviceType.Hardware, topLevelForm.Handle,
                        CreateFlags.SoftwareVertexProcessing, presentationParameters);
                }
                catch (DirectXException ex2)
                {
                    // 동작 실패
                    Debug.WriteLine(ex2.ToString());
                    try
                    {
                        // 소프트웨어에 의한 정점 처리, rasterize를 실시한다
                        // 퍼포먼스는 매우 낮습니다.
                        // 그 대신해, 대부분의 처리를 제한없이 실행할 수 있습니다.
                        this._device = new Device(0, DeviceType.Reference, topLevelForm.Handle,
                            CreateFlags.SoftwareVertexProcessing, presentationParameters);
                    }
                    catch (DirectXException ex3)
                    {
                        // 동작 실패
                        // 디바이스는 생성할 수 없습니다.
                        throw ex3;
                    }
                }
            }
        }

        /// <summary
        /// 
        private void SettingCamera()        //카메라 이동
        {
            //키에 의한이동
            if (this._keys[(int)Keys.Left])
            {
                // <- 키가 눌린경우
                this._lensPosTheta -= 3.0f;
            }
            if (this._keys[(int)Keys.Right])
            {
                // -> 키가 눌린경우
                this._lensPosTheta += 3.0f;
            }
            if (this._keys[(int)Keys.Up])
            {
                // 위로 화살표 키가 눌린경우
                this._lensPosPhi += 3.0f;
            }
            if (this._keys[(int)Keys.Down])
            {
                // -> 키가 눌린경우
                this._lensPosPhi -= 3.0f;
            }

            //_lensPosPhi??편 제한
            if (this._lensPosPhi >= 90.0f)
            {
                this._lensPosPhi = 89.9999f;
            }
            if (this._lensPosPhi <= -90.0f)
            {
                this._lensPosPhi = -89.9999f;
            }

            //렌지의 위치를 3차원 극좌표로 변환
            float radius = this._lensPosRadius;
            float theta = Geometry.DegreeToRadian(this._lensPosTheta);
            float phi = Geometry.DegreeToRadian(this._lensPosPhi);
            Vector3 lensPosition = new Vector3(
                (float)(radius * Math.Cos(theta) * Math.Cos(phi)),
                (float)(radius * Math.Sin(phi)),
                (float)(radius * Math.Sin(theta) * Math.Cos(phi)));

            //뷰 변환 행렬을 왼손 좌표계 뷰 행렬로 설정한다.
            this._device.Transform.View = Matrix.LookAtLH(lensPosition, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));


            //뷰 변환 행렬을 설정   //키보드로 움직일때? ?? 위에 것과 충돌??
            //  (카메라의위치, 카메라의 주시점, 월드의 윗쪽)
            //this._device.Transform.View = Matrix.LookAtLH(new Vector3(0.0f, 0.0f, -10.0f),new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));

            //투명 변환을 설정
            // (Y축 방향의 시야각 ,스크린폭과 세로의 비율 /(나누기) 카메라에 가까운 뷰면의 클리핑Z치수, 카메라로부터 먼 뷰먼의 클리핑Z치)
            this._device.Transform.Projection = Matrix.PerspectiveFovLH(
                Geometry.DegreeToRadian(60.0f),
                (float)this._device.Viewport.Width / (float)this._device.Viewport.Height,
                1.0f, 100.0f);
        }
        ///<summary>
        ///마우스 이동 이벤트
        /// </summary>
        ///
        private void form_MouseMove(object sender, MouseEventArgs e)//마우스 이동 이벤트
        {
            if (e.Button == MouseButtons.Left)
            {
                //회전
                this._lensPosTheta -= e.Location.X - this._oldMousePoint.X;
                this._lensPosPhi += e.Location.Y - this._oldMousePoint.Y;

                //회전에 한계를 둔다.
                if (this._lensPosPhi >= 90.0f)
                {
                    this._lensPosPhi = 89.9999f;
                }
                else if (this._lensPosPhi <= -90.0f)
                {
                    this._lensPosPhi = -89.9999f;
                }
            }
            //마우스의 위치ㅡㄹ 기억
            this._oldMousePoint = e.Location;
        }
    }
}
