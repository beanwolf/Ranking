using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;


namespace DXClient
{

    // class 앞에 partial 라는 키워드!! 
    //IDisposable ???
    public partial class MainSample : IDisposable
    {

        /// <summary>
        ///     메인폼 = MainSample.cs
        /// </summary>
        //        private Main _form = null;


        /// 좌표 변화가 끝난정점 데이터
        //private CustomVertex.TransformedColored[] _vertices = new CustomVertex.TransformedColored[3];


        ///<summary>
        ///정점 버퍼
        /// </summary>
        private VertexBuffer _vertexBuffer = null;

        ///<summary>
        /// 인덱스 버퍼
        /// </summary>
        private IndexBuffer _indexBuffer = null;

        ///<summary>
        /// 인덱스 버퍼의 각 정점 번호 배열
        /// </summary>
        private static Int16[] _vertexIndices = new Int16[] { 2, 0, 1, 1, 3, 2, 4, 0, 2, 2, 6, 4, 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 6, 2, 3, 3, 7, 6, 4, 6, 7, 7, 5, 4 };


         



        ///<summary>
        /// 1개전의 마우스의 위치
        /// </summary>
        private Point _oldMousePoint = Point.Empty;

        ///<summary>
        /// PFS 초기값을 
        /// </summary>
        private int _fps = 0;
        private int _Nowfps = 0;    

        private int currentTick, previouseTick = Environment.TickCount;
       
        

        /// <summary>
        ///    재질감설정
        /// </summary>
        private Texture _texture = null;


        /// <summary>
        ///  키보드 프레스 판정
        /// </summary>
        private bool[] _keys = new bool[256];


        /// <summary>
        ///     Direct 3D 디바이스
        /// </summary>
        /// <param name="toplevelForm"></param>
        /// <returns></returns>
 
        /// <summary>
        ///     어플리케이션 초기화
        /// </summary>
        /// <param name="toplevelForm"></param>
        /// <returns>모든 초기화가 OK라면 true, 하나라도 실패하면 false를 돌려준다.</returns>
        /// <remarks>
        /// false를 돌려주었을 경우는, 자동으로 어플ㄹ리케이션이 종료됨
        /// </remarks>
        public bool InitializeApplication(Main toplevelForm)            //메인 폼 로드
        {
            //폼의 참조를 보관유지
            this._form = toplevelForm;

            #region 메인 설정

            //입력 이벤트 작성
            //this.CreateDevice(toplevelForm);
            //폰트의 작성 DirectXPartial.cs로 옮김
            //카메라의 설정 DirectXPartial.cs로 옮김
            //키보드 입력 DirectXPartial.cs로 옮김


            //마우스 이동 이벤트
            toplevelForm.MouseMove += new MouseEventHandler(this.form_MouseMove);


            //키보드 인풋
            this.CreateInputEvent(toplevelForm);


            try
            {
                // 디바이스의 작성
                this.CreateDevice(toplevelForm);

                //폰트의작성
                this.CreateFont();

            }
            catch (DirectXException ex)
            {
                // 예외 발생
                MessageBox.Show(ex.ToString(), "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //1. 사각ㅎㅇ 작성
            //this.CreateSquarePolygon();


            //2.상자를 작성하기 위한 정점 버퍼를 구성
            //상자의 정점은 8개
            this._vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 8, this._device, Usage.None, CustomVertex.PositionColored.Format, Pool.Managed);

            //8개의 정보를 보관하기 위한 메모리를 확보
            CustomVertex.PositionColored[] vertices = new CustomVertex.PositionColored[8];

            //각 정점을 설정
            //vertices[0] = new CustomVertex.PositionColored(-2.0f, 2.0f, 2.0f, Color.Yellow.ToArgb());
            //vertices[1] = new CustomVertex.PositionColored(2.0f, 2.0f, 2.0f, Color.Gray.ToArgb());
            //vertices[2] = new CustomVertex.PositionColored(-2.0f, 2.0f, -2.0f, Color.Purple.ToArgb());
            //vertices[3] = new CustomVertex.PositionColored(2.0f, 2.0f, -2.0f, Color.Red.ToArgb());
            //vertices[4] = new CustomVertex.PositionColored(-2.0f, -2.0f, 2.0f, Color.SkyBlue.ToArgb());
            //vertices[5] = new CustomVertex.PositionColored(2.0f, -2.0f, 2.0f, Color.Orange.ToArgb());
            //vertices[6] = new CustomVertex.PositionColored(-2.0f, -2.0f, -2.0f, Color.Green.ToArgb());
            //vertices[7] = new CustomVertex.PositionColored(2.0f, -2.0f, -2.0f, Color.Blue.ToArgb());

            
            vertices[0] = new CustomVertex.PositionColored(-2.0f, 2.0f, 2.0f, Color.Yellow.ToArgb());
            vertices[1] = new CustomVertex.PositionColored(2.0f, 2.0f, 2.0f, Color.Yellow.ToArgb());
            vertices[2] = new CustomVertex.PositionColored(-2.0f, 2.0f, -2.0f, Color.Yellow.ToArgb());
            vertices[3] = new CustomVertex.PositionColored(2.0f, 2.0f, -2.0f, Color.Yellow.ToArgb());
            vertices[4] = new CustomVertex.PositionColored(-2.0f, -2.0f, 2.0f, Color.Blue.ToArgb());
            vertices[5] = new CustomVertex.PositionColored(2.0f, -2.0f, 2.0f, Color.Blue.ToArgb());
            vertices[6] = new CustomVertex.PositionColored(-2.0f, -2.0f, -2.0f, Color.Blue.ToArgb());
            vertices[7] = new CustomVertex.PositionColored(2.0f, -2.0f, -2.0f, Color.Blue.ToArgb());

                //정점 버퍼를 잠근다?
            using (GraphicsStream data = this._vertexBuffer.Lock(0, 0, LockFlags.None))
            {
                //정점 데이터를 정점 버퍼에 씁니다.
                data.Write(vertices);
                //정점 버퍼의 락을 해제합니다.
                this._vertexBuffer.Unlock();
            }


            //index buffer의 작성
            //제2의 인수의 수치는 (삼각 다각형의수)*(하나의 삼각 다각형의 정점수)* (16비트의 인덱스 사이즈(2bytes))
            this._indexBuffer = new IndexBuffer(this._device, 12 * 3 * 2, Usage.WriteOnly, Pool.Managed, true);

            //index buffer를 잠근다.
            using (GraphicsStream data = this._indexBuffer.Lock(0,0, LockFlags.None))
            {
                data.Write(_vertexIndices);
                this._indexBuffer.Unlock();
            }






       

            //투영변환을 설정
            this._device.Transform.Projection = Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(60.0f), (float)this._device.Viewport.Width / (float)this._device.Viewport.Height, 1.0f, 100.0f);

            //CullMode를 none 로 해 다각형의 뒷면도 그려준다.
            //this._device.RenderState.CullMode = Cull.None;

            //라이트를 끔(현재는 true 로 하면 다각형이 새까맣게 될지 모른다)
            this._device.RenderState.Lighting = false;

            return true;
            #endregion

        }


      


        /// 
        /// 
        /// 
        /// 메인 루프 처리
        /// 

        public void MainLoop()
        {
            //카메라의 설정
            this.SettingCamera();

            //FPS 계산=============================================
            currentTick = Environment.TickCount;    //현재틱
            if (currentTick >= previouseTick + 1000)    //1초가 지난경우
            {
                previouseTick = currentTick;
                _Nowfps = _fps;
                _fps = 0;
            }
            _fps++;
            //FPS 계산=============================================끝==





            //=========================================================================
            //화면을 단색 (파랑색)으로 클리어    Z버퍼도 클리어(|ClearFlags.ZBuffer 추가)
            this._device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DarkBlue, 1.0f, 0);
            // BeginScene와 EndScene 사이에 그릴 내용을 코딩한다.
            this._device.BeginScene();
            //=========================================================================


            //사각형 그리기
            //this.RenderSquarePolygon();

            //정점 버퍼를 디바이스이ㅡ 데이터 스트림에 바인드
            this._device.SetStreamSource(0, this._vertexBuffer, 0);

            //그릴 정점의 포멧을 세트
            this._device.VertexFormat = CustomVertex.PositionColored.Format;

            //index buffer르르 세트
            this._device.Indices = this._indexBuffer;

            //렌더링(그리기)
            this._device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 12);




            //문자열 그리기
            //this._font.DrawText(null, "[↕]:카메라의 상하 회전,  [↔]:카메라의 좌우회전, 마우스에 의한 카메라 회전", 0, 0, Color.Yellow);
            this._font.DrawText(null, "[↔] :" + this._lensPosTheta, 0, 48, Color.Yellow);
            this._font.DrawText(null, "[↕] :" + this._lensPosPhi, 0, 24, Color.Yellow);
            this._font.DrawText(null, "마우스 위치 :" + this._oldMousePoint, 0, 72, Color.White);
            this._font16.DrawText(null, this._Nowfps + "fps", this._form.Width - 60 , 0, Color.Gray);   //Fps


            //this._font.DrawText(null, "3차원 공간에 3D 사각형 다각형 재질감 입혀보기 ", 0, 0, Color.PowderBlue);


            ////좌표 x=0, y=0의 위치에 흰색으로 그리기
            //this._font.DrawText(null, "DirectX tips 샘플", 0, 0, Color.White);

            ////포인트 구조체를 사용해 위치를 설정
            //this._font.DrawText(null, "포인트 구조체를 사용해 위치를 설정", new Point(250, 40), Color.Yellow);


            ////두줄 출력도 가능
            //this._font.DrawText(null, "이렇게 하면 두줄도 출력이" + Environment.NewLine + "가능", new Rectangle(400, 320, 500, 400), DrawTextFormat.Left | DrawTextFormat.Top, Color.LightGray);


            ////rectangle 구조체와 텍스ㅡㅌ 포멧을 사용해 글기
            //this._font.DrawText(null, "rectangle 구조체와 텍스ㅡㅌ 포멧을 사용해 글기", new Rectangle(100, 260, 500, 400), DrawTextFormat.Left | DrawTextFormat.Top, Color.LightPink);

            //문자열 출력(키보드로 입력받은)
            //this._font.DrawText(null, "키보드 입력받기", 0, 0, Color.White);
            //int height = 24;
            //for (int i = 0; i < this._keys.Length; i++)
            //{
            //    if (this._keys[i])
            //    {
            //        this._font.DrawText(null, ((Keys)i).ToString(), 0, height, Color.White);
            //        height += 24;
            //    }
            //}



            //버퍼에 그리기는 여기까지
            this._device.EndScene();

            //실제 출력
            this._device.Present();
        }

       /// <summary>
       ///  자원의 파기를 하기 이해서 불린다.
       /// </summary>
             
        public void Dispose()
        {
            //택스쳐 해제
            //if (this._texture != null)
            //{
            //    this._texture.Dispose();
            //}
            //정점 버프를 해제
            if(this._vertexBuffer != null)
            {
                this._vertexBuffer.Dispose();
            }
            
            //index buffer를 해제
            if(this._indexBuffer != null)
            {
                this._indexBuffer.Dispose();
            }


            //폰트의 자원을 해제
            if(this._font != null)
            {
                this._font.Dispose();
            }



            //direct3d 디바이스의 자원 해제
            if(this._device != null)
            {
                this._device.Dispose();
            }
        }
    }
}
