using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4000Client
{
    class SampleCodeByMu
    {
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//출처: https://ryuschool.tistory.com/entry/%EC%B4%88%EA%B0%84%EB%8B%A8-C%EC%9C%BC%EB%A1%9C-TCPIP-%ED%86%B5%EC%8B%A0%ED%95%98%EB%8A%94%EB%B2%95-1

//C#은 참 간단하다.
//뭐든지 뚝딱뚝딱.
//그렇다고 C++ / MFC처럼 복잡한것도아니고,
//VB처럼 쉽지만 코드가 병맛이 되는것도 아니고 .... 
//참 MS에서 요로코롬 잘 맹글어 놓은 언어인것같당!

//이번에는 C#에서 참 ! 쉽게 , TCP/IP 소켓프로그래밍을 해보장 =_=
//C#왕초보인 내가 사용경험에 의해 쓰는 글이므로 , 초보분들이 보시면 도움이 되겠다.

//※읽으면 좋은 0순위 : VB에서는 소켓프로그래밍 참잘했는데 C#에서는 막막한 분.

//맨처음 소켓프로그래밍을 시작하려면 , 네임스페이스를 지정해주자.
//소켓관련 네임스페이스는

//using System.Net;
//using System.Net.Sockets;

//이것 둘 뿐이다
//이 네임스페이스들안에는 Socket 클래스 , IPEndPoint 클래스등..
//여러 클래스들이 있다 .

//자 !! 거두절미하고 !! ( 다들 서문이 긴건 싫어하실듯 ㅋ_ㅋ )

//일단 서버를 만들어보자.

//Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

/*
=_= 간단하게 TCP 소켓 인스턴스가 하나생성되었다.
이놈을 초기화시켜주자.

IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8088);
server.Bind ( ipep );
server.Listen ( 10 );
MessageBox.Show ( "Connection request!!" );

끝이다.너무 간단한가 ?
이 몇줄 안되는 구문으로 , 8088포트로 TCP프로토콜을 이용하여 스트림을 수신할수 있는
서버가 만들어진것이다;;

그러나!! 이건 중요하다.잘보시길.
지금서버를 연 방식은 동기소켓 ( Sync socket) 이다.
더욱 간단하게 말해 , 저 코드에서 클라이언트가 8088포트로 접속하기 전에는 죽어도 server.Listen ( 10 ); 밑 라인의 MessageBox가 뜨지 않는다는 말이다.
계속 Listen 메서드가 실행되면서 접속을 기다릴뿐....

어? 그러면 지장이있다.
예를들어 메인 폼에서 Load 이벤트가 발생하는곳에 저 코드를 넣었다고 가정해보자.
.... 클라이언트가 접속하기전까지는 폼이 뜨지도 않을것이다.

그래서...대안인 쓰레드를 쓰는것이다!!!!
메인 쓰레드와 다른 또 다른 서버 쓰레드를 돌려서, 저 코드를 돌리는것이다.
그러면 해결된다 .
( 쓰레드가 싫은 사람은 비동기소켓 ( Async socket) 을 이용하시길..비동기소켓은 Event-Driven 방식을 이용한당.VB에서의 Winsock컨트롤과 똑같다고 보시면 된다. )

자 그럼..저 코드에서 클라이언트가 접속한다고 가정해보자.
서버는 접속승인을 할것이고 , 클라이언트는 접속을 할것이다.
그럼 코드로는 다음과 같이 된다.

Socket client = server.Accept();

... 한줄의 코드로 클라이언트가 접속되었다 = _ =..
그리고 상세히 설명하자면 : 클라이언트의 데이터를 바탕으로 또 다른 소켓 인스턴스를 생성한다.
이제 저놈을 가지고 데이터를 보내고 이리저리 요리하면 되는것이다.
( ... 이쯤되면 VB에서 Winsock으로 소켓프로그래밍 하시던 분들은 삘이 오실것이다)

자,, 그럼 이제 클라이언트가 데이터를 보내는걸 받아보자.
클라이언트가 접속했을때 생성된 소켓 인스턴스를 이용한다.
데이터를 받을 바이트배열이 필요하다.

byte[] buf = new byte[1024];
client.Receive ( buf );

MessageBox.Show ( Encoding.Default.GetString ( buf ) );

... 끝. =_=
데이터를 1024짜리 바이트배열에 받아서(참고로 오버로딩된 메서드를 뒤져보면 원하는 크기만큼 스트림에서 땡겨올수있다 = _ = 그건 찾아보시길 ㅋ_ㅋ) byte배열에 저장된 스트림 데이터를 string으로 변환해서 찍어주었다.

코드를 보면 이해가 팍팍가지 않는가 ??;
정말 간결한 C#이다 .... =_=;

자, 그럼, 클라이언트에게 데이터를 보내볼까?
당연히 아까 생성된 client 소켓 인스턴스를 이용한다.
보낼대는 byte배열에 보낼내용을 채워넣은다음 보내면 된다.
byte배열 buf에 보낼 내용이 들어있다고 가정하자.


client.Send ( buf );

..끝이다.. =_=
허무한가?
그런데 정적소켓방식에서는 정말 이게 다다..;;

예외처리는 어떻게 할까?
try ~catch로 묶어주고 , 예외처리 클래스는 무엇이 올까?
C#을 만지신 분들은 빡 감이 올것이다

try
{
  client.Send ( buf );
}
catch ( SocketException se )
{
   MessageBox.Show ( se.Message );
   return;
}

뭐..이런식으로 해주면 된다 =_=;
저 코드에서 데이터를 보내려하는데 연결이 끊겨있으면 에러메시지가 출력되고
메서드가 종료될것이다.
참으로 간단하다 허허 ..;;

출처: https://ryuschool.tistory.com/entry/초간단-C으로-TCPIP-통신하는법-1 [Ryu School]










////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////







[출처: http://blog.naver.com/mankeys?Redirect=Log&logNo=138953202 ]




1. TCP Server






using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.IO;

using System.Net;

using System.Net.Sockets;




namespace ConsoleApplication_TCPEcho01

{

    class TCP_Server

    {

        static void Main(string[] args)

        {

            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];

            Console.WriteLine("ipAddress" + ipAddress);

            TcpListener tcp_Listener = new TcpListener(ipAddress, 5555);

            tcp_Listener.Start();

            while (true)

            {

                Console.WriteLine("1. EchoServer 대기상태.......");




                TcpClient client = tcp_Listener.AcceptTcpClient();      //클라이언트와 접속

                Console.WriteLine("2. Echo Client 접속....");



                NetworkStream ns = client.GetStream();

                StreamReader reader = new StreamReader(ns);

                string msg = reader.ReadLine();                     //메시지를 읽어 옴



                Console.WriteLine("3. [클라이언트 메시지]:" + msg);     //메시지 출력



                StreamWriter writer = new StreamWriter(ns);

                writer.WriteLine(msg);                              //네트워크 스트림에 쓰는 듯 함

                writer.Flush();




                Console.WriteLine("4. [Echo1]:" + msg);

                writer.WriteLine(msg);

                writer.Flush();



                Console.WriteLine("5. [Echo2]:" + msg);

                Console.WriteLine("6. 스트림과 TcpClient Close");



                writer.Close();

                reader.Close();

                client.Close();




            }

        }

    }

}





2. TCP Client





using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Net;

using System.Net.Sockets;

using System.IO;




namespace ConsoleApplication_TCPEcho01

{

    public class TCP_Client

    {

        public static void Main(string[] arg)

        {

            string args0 = "localhost";     //ip

            string args1 = "5555";          //포트 번호

            string args2 = "안녕";            //메시지




            TcpClient client = new TcpClient(args0, Int32.Parse(args1));    // (ip주소 , 포트 번호)

            NetworkStream ns = client.GetStream();

            StreamWriter writer = new StreamWriter(ns);




            writer.WriteLine(args2);

            writer.Flush();

            Console.WriteLine(args2 + "를 전송합니다. ");




            StreamReader reader = new StreamReader(ns);




            string msg1 = reader.ReadLine();                //네트워크에서 스트림을 읽음

            Console.WriteLine("[Echo1]: " + msg1);

            string msg2 = reader.ReadLine();

            Console.WriteLine("[Echo2]: " + msg2);



            writer.Close();

            reader.Close();

            client.Close();




            Console.ReadLine();




        }// Main







    }// class

}







///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




출처: https://sosal.kr/760





Server에서 Listen으로 Client의 접속을 기다립니다.

Client가 접속하면, Client가 보낸 메시지를 시간과 함께 돌려줍니다. (Echo 서버)




Stream을 이용하여 데이터 전송을 구현하였습니다. (StreamReader, StreamWriter)

인코딩은 UTF8을 사용하였습니다.











Client 소스


--------------------------------------------------------------------------------





using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.Windows;

 

using System.IO;

using System.Net;

using System.Net.Sockets;

 

namespace ConsoleTest

{

    class Program

    {

        static void Main(string[] args)

        {

            int PORT = 5555;

            string IP = "localhost";



            NetworkStream NS = null;

            StreamReader SR = null;

            StreamWriter SW = null;

            TcpClient client = null;



            try

            {

                client = new TcpClient(IP, PORT); //client 연결

                Console.WriteLine("{0}:{1}에 접속하였습니다.", IP, PORT);

                NS = client.GetStream(); // 소켓에서 메시지를 가져오는 스트림

                SR = new StreamReader(NS, Encoding.UTF8); // Get message

                SW = new StreamWriter(NS, Encoding.UTF8); // Send message



                string SendMessage = string.Empty;

                string GetMessage = string.Empty;



                while ((SendMessage = Console.ReadLine()) != null)

                {

                    SW.WriteLine(SendMessage); // 메시지 보내기

                    SW.Flush();



                    GetMessage = SR.ReadLine();

                    Console.WriteLine(GetMessage);

                }

            }



            catch (Exception e)

            {

                System.Console.WriteLine(e.Message);

            }

            finally

            {

                if (SW != null) SW.Close();

                if (SR != null) SR.Close();

                if (client != null) client.Close();





            }

        }

    }


}










Server 소스


--------------------------------------------------------------------------------








using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

 

 

using System.IO;

using System.Net;

using System.Net.Sockets;

 

 

namespace Sock_console_server

{

    class Program

    {

        static void Main(string[] args)

        {

            TcpListener Listener = null;

            NetworkStream NS = null;



            StreamReader SR = null;

            StreamWriter SW = null;

            TcpClient client = null;



            int PORT = 5555;



            Console.WriteLine("서버소켓");

            try

            {

                Listener = new TcpListener(PORT);

                Listener.Start(); // Listener 동작 시작



                while (true)

                {

                    client = Listener.AcceptTcpClient();



                    NS = client.GetStream(); // 소켓에서 메시지를 가져오는 스트림

                    SR = new StreamReader(NS, Encoding.UTF8); // Get message

                    SW = new StreamWriter(NS, Encoding.UTF8); // Send message



                    string GetMessage = string.Empty;



                    while (client.Connected == true) //클라이언트 메시지받기

                    {

                        GetMessage = SR.ReadLine();



                        SW.WriteLine("Server: {0} [{1}]", GetMessage, DateTime.Now); // 메시지 보내기

                        SW.Flush();

                        Console.WriteLine("Log: {0} [{1}]", GetMessage, DateTime.Now);

                    }

                }

            }

            catch (Exception e)

            {

                System.Console.WriteLine(e.Message);

            }

            finally

            {

                SW.Close();

                SR.Close();

                client.Close();

                NS.Close();

            }

        }

    }

}















출처: https://sosal.kr/760 [so_sal　]







///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




출처: http://www.csharpstudy.com/net/article/9-Socket-%ED%81%B4%EB%9D%BC%EC%9D%B4%EC%96%B8%ED%8A%B8




Socket 클라이언트

Socket 클래스

.NET Framework에서 Socket 클래스는 가장 Low 레벨의 클래스로서 TcpClient, TcpListener, UdpClient 들은 모두 Socket 클래스를 이용하여 작성되었다.TcpClient, TcpListener, UdpClient 들이 모두 TCP/IP와 UDP/IP 프로토콜 만을 지원하는 반면, Socket 클래스는 IP 뿐만 아니라 AppleTalk, IPX, Netbios, SNA 등 다양한 네트워크들에 대해 사용될 수도 있다. 여기서는 Socket 클래스를 사용하여 TCP, UDP 네트워크를 사용하는 부분에 대해 살펴 본다.

Socket 클라이언트

Socket 클래스는 클라이언트와 서버에서 공히 사용할 수 있다. 먼저 Socket 클래스를 사용하여 TCP 클라이언트를 만드는 간단한 예제를 살펴보자. 아래 예제는 간단한 메시지를 TCP 서버에 보내고 Echo 된 문자열을 계속 화면에 표시하는 프로그램이다. 이 프로그램은 Q 를 누를 때까지 계속 된다.



1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
 
namespace sockcli
{
    class Program
    {
        static void Main(string[] args)
        {
            // (1) 소켓 객체 생성 (TCP 소켓)
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // (2) 서버에 연결
            var ep = new IPEndPoint(IPAddress.Parse("192.168.1.13"), 7000);
            sock.Connect(ep);

            string cmd = string.Empty;
            byte[] receiverBuff = new byte[8192];

            Console.WriteLine("Connected... Enter Q to exit");

            // Q 를 누를 때까지 계속 Echo 실행
            while ((cmd = Console.ReadLine()) != "Q")
            {
                byte[] buff = Encoding.UTF8.GetBytes(cmd);

                // (3) 서버에 데이타 전송
                sock.Send(buff, SocketFlags.None);

                // (4) 서버에서 데이타 수신
                int n = sock.Receive(receiverBuff);

                string data = Encoding.UTF8.GetString(receiverBuff, 0, n);
                Console.WriteLine(data);
            }

            // (5) 소켓 닫기
            sock.Close();
        }
    }
}



위 예제를 각 스텝별로 살펴보면,
1.먼저 Socket 객체를 생성하는데, 첫번째 파라미터는 IP 를 사용한다는 것이고, 두번째는 스트림 소켓을 사용한다는 것이며, 마지막은 TCP 프로토콜을 사용한다는 것을 지정한 것이다.TCP 프로토콜은 스트림 소켓을 사용하고, UDP는 데이타그램 소켓을 사용한다.
2.Socket 객체의 Connect() 메서드를 호출하여 서버 종단점(EndPoint)에 연결한다.
3.소켓을 통해 데이타를 보내기 위해 Socket 객체의 Send() 메서드를 사용하였다.데이타 전송은 첫번째 파라미터에 바이트 배열을 넣으면 되고, 두번째 파라미터는 옵션으로 SocketFlags를 지정할 수 있다.이 플래그는 소켓에 보다 고급 옵션들을 지정하기 위해 사용된다.
4.소켓에서 데이타를 수신하기 위해 Socket 객체의 Receive() 메서드를 사용하였다.Receive() 메서드는 첫번째 파라미터에 수신된 데이타를 넣게 되고, Send()와 마찬가지로 SocketFlags 옵션을 지정할 수도 있다.Receive() 메서드는 실제 수신된 바이트수를 정수로 리턴한다.
5.마지막으로 소켓을 닫는다.







///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



출처: https://it-jerryfamily.tistory.com/entry/Socket-%ED%86%B5%EC%8B%A0-%EC%98%88%EC%A0%9C















[Program C#]Socket 통신 - 예제



Berkeley 소켓 인터페이스를 이용한 구현을 예제로 만듭니다. 서버와 클라이언트는 모두 콘솔 프로그램으로 만들며, 지금 작성하는 프로그램은 가장 간단한 프로그램으로 클라이언트가 서버로 메시지를 한 번만 전송하고 콘솔 화면에서 엔터키를 입력함으로 프로그램이 종료되는 것입니다.




프로그램 설명




서버는 소켓을 생성하고 Bind 시키며 Listen 상태인 대기 상태로 둡니다. 클라이언트의 연결 요청이 들어오면 accept 소켓을 생성하고 데이타를 받기 시작합니다. 받은 데이타는 콘솔 화면에 보여주고 사용자가 엔터키를 입력함으로 프로그램은 종료 됩니다.




클라이언트는 서버에 아이피와 포트를 이용해서 연결을 하고 콘솔에 텍스트를 입력하고 엔터 키를 입력함으로 서버에 데이타를 전송하고, 데이타가 전송이 되었다는 메시지를 보이고, 사용자가 엔터키를 입력함으로 프로그램이 종료 됩니다.




실행 후

















메시지 전송 후

















프로그램 작성 순서




1. 소켓과 관련한 네임스페이스를 서버/클라이언트 모두에 포함 시킵니다.




?

1
2
 
using System.Net;
using System.Net.Sockets;
 


2. 서버에 아래의 코드를 입력합니다.







?

1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26


static byte[] Buffer { get; set; }
static Socket sck;

static void Main(string[] args)
{
    sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    sck.Bind(new IPEndPoint(IPAddress.Any, 1234));
    sck.Listen(100);

    Socket accepted = sck.Accept();

    Buffer = new byte[accepted.SendBufferSize];
    int bytesRead = accepted.Receive(Buffer);
    byte[] formatted = new byte[bytesRead];
    for (int i = 0; i < bytesRead; ++i)
    {
        formatted[i] = Buffer[i];
    }

    string strdata = Encoding.UTF8.GetString(formatted);
    Console.Write(strdata + "\r\n");
    Console.Read();

    accepted.Close();
    sck.Close();
}
 







3. 클라이언트에 아래의 코드를 입력합니다.





?

1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26


static Socket sck;
static void Main(string[] args)
{
    sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);

    try
    {
        sck.Connect(localEndPoint);
    }
    catch
    {
        Console.Write("Unable to connect to remote end point!\r\n");
        Main(args);
    }

    Console.Write("Enter Text: ");
    string text = Console.ReadLine();
    byte[] data = Encoding.UTF8.GetBytes(text);

    sck.Send(data);
    Console.Write("Data Sent!\r\n");
    Console.Write("Press any key To continue...");
    Console.Read();
    sck.Close();
}



출처: https://it-jerryfamily.tistory.com/entry/Socket-통신-예제 [IT 이야기]








///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




출처: https://docs.microsoft.com/ko-kr/dotnet/framework/network-programming/synchronous-client-socket-example







동기 클라이언트 소켓 예제





다음 예제 프로그램에서는 서버에 연결하는 클라이언트를 만듭니다.이 클라이언트는 동기 소켓으로 빌드되므로 서버에서 응답을 반환할 때까지 클라이언트 애플리케이션의 실행이 일시 중단됩니다.애플리케이션은 서버에 문자열을 보낸 다음 서버에서 반환된 문자열을 콘솔에 표시합니다.

C#복사
using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
  
public class SynchronousSocketClient
{

    public static void StartClient()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];

        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.  
                byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                // Send the data through the socket.  
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        StartClient();
        return 0;
    }
}







///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




출처: https://www.hooni.net/xe/study/2382










// Server Socket





?

1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
77
78
79
80
81
82
83
84
85
86
87
88
89
90
91
92
93
94
95
96
97
 
// NameSpace 선언
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
 
namespace ServerSideSocket
{
    class ServerClass
    {
        public static Socket Server, Client;

        public static byte[] getByte = new byte[1024];
        public static byte[] setByte = new byte[1024];

        public const int sPort = 5000;

        [STAThread]
        static void Main(string[] args)
        {
            string stringbyte = null;
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);

            try
            {
                Server = new Socket(
                  AddressFamily.InterNetwork,
                  SocketType.Stream, ProtocolType.Tcp);

                Server.Bind(serverEndPoint);
                Server.Listen(10);

                Console.WriteLine("------------------------");
                Console.WriteLine("클라이언트의 연결을 기다립니다. ");
                Console.WriteLine("------------------------");

                Client = Server.Accept();

                if (Client.Connected)
                {
                    while (true)
                    {
                        Client.Receive(getByte, 0, getByte.Length, SocketFlags.None);
                        stringbyte = Encoding.UTF7.GetString(getByte);

                        if (stringbyte != String.Empty)
                        {
                            int getValueLength = 0;
                            getValueLength = byteArrayDefrag(getByte);

                            stringbyte = Encoding.UTF7.GetString(
                              getByte, 0, getValueLength + 1);

                            Console.WriteLine("수신데이터:{0} | 길이:{1}",
                              stringbyte, getValueLength + 1);

                            setByte = Encoding.UTF7.GetBytes(stringbyte);
                            Client.Send(setByte, 0, setByte.Length, SocketFlags.None);
                        }

                        getByte = new byte[1024];
                        setByte = new byte[1024];
                    }
                }
            }
            catch (System.Net.Sockets.SocketException socketEx)
            {
                Console.WriteLine("[Error]:{0}", socketEx.Message);
            }
            catch (System.Exception commonEx)
            {
                Console.WriteLine("[Error]:{0}", commonEx.Message);
            }
            finally
            {
                Server.Close();
                Client.Close();
            }
        }

        public static int byteArrayDefrag(byte[] sData)
        {
            int endLength = 0;

            for (int i = 0; i < sData.Length; i++)
            {
                if ((byte)sData[i] != (byte)0)
                {
                    endLength = i;
                }
            }

            return endLength;
        }
    }
}






// Client Socket





?

1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
77
78
79
80
81
82
83
84
85
86
87
 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
 
namespace ClientSideSocket
{
    class ClientClass
    {
        public static Socket socket;
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];

        public const int sPort = 5000;

        [STAThread]
        static void Main(string[] args)
        {
            string sendstring = null;
            string getstring = null;

            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);

            socket = new Socket(
              AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("------------------------------");
            Console.WriteLine(" 서버로 접속합니다.[엔터를 입력하세요] ");
            Console.WriteLine("------------------------------");
            Console.ReadLine();

            socket.Connect(serverEndPoint);

            if (socket.Connected)
            {
                Console.WriteLine(">>연결 되었습니다.(데이터를 입력하세요)");
            }

            while (true)
            {
                Console.Write(">>");
                sendstring = Console.ReadLine();

                if (sendstring != String.Empty)
                {
                    int getValueLength = 0;
                    setbyte = Encoding.UTF7.GetBytes(sendstring);

                    socket.Send(setbyte, 0,
                      setbyte.Length, SocketFlags.None);

                    Console.WriteLine("송신 데이터 : {0} | 길이{1}",
                      sendstring, setbyte.Length);

                    socket.Receive(getbyte, 0,
                      getbyte.Length, SocketFlags.None);

                    getValueLength = byteArrayDefrag(getbyte);

                    getstring = Encoding.UTF7.GetString(getbyte,
                      0, getValueLength + 1);

                    Console.WriteLine(">>수신된 데이터 :{0} | 길이{1}",
                      getstring, getValueLength + 1);
                }

                getbyte = new byte[1024];
            }
        }

        public static int byteArrayDefrag(byte[] sData)
        {
            int endLength = 0;

            for (int i = 0; i < sData.Length; i++)
            {
                if ((byte)sData[i] != (byte)0)
                {
                    endLength = i;
                }
            }

            return endLength;
        }
    }
}









///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




출처: https://devvcxxx.tistory.com/31







서버소스.
접속한 클라이언트에게 메세지를 10번 보낸다.
 




1

2

3

4

5

6

7

8

9

10

11

12

13

14

15

16

17

18

19

20

21

22

23

24

25

26

27

28

29

30

31

32

33

34

35

36

37

38

39

40

41

42

43

44

45

46

47

48

49

50

51

52

53

54

55

56

57

58

59 


    using System.Net.Sockets;

    using System.Threading;

 

    private TcpListener client;



public ServerForm()

{

    InitializeComponent();

}



private void ServerForm_Load(object sender, EventArgs e)

{

    // 클라이언트의 접속요청을 받는 스레드 시작

    Thread thr = new Thread(new ThreadStart(ListenerThread));

    thr.Start();

}



public void ListenerThread()

{

    client = new TcpListener(12345);

    client.Start();



    while (true)

    {

        // 클라이언트의 연결 요청 확인

        while (!client.Pending())

        {

            Thread.Sleep(100);

        }



        // 클라이언트와의 통신처리 스레드 시작

        ConnectionHandler newConnection = new ConnectionHandler();

        newConnection.threadListener = this.client;

        Thread newThread = new Thread(new ThreadStart(newConnection.clientHandler));

        newThread.Start();

    }

}

}

 

public class ConnectionHandler

{

    public TcpListener threadListener;



    public void clientHandler()

    {

        TcpClient client = threadListener.AcceptTcpClient();

        NetworkStream ns = client.GetStream();



        string msg = "Welcome to server\n";



        byte[] send = Encoding.ASCII.GetBytes(msg);



        for (int i = 0; i < 10; i++)

        {

            ns.Write(send, 0, send.Length);

            Thread.Sleep(1000);

        }

    }

}



클라이언트 소스.
서버에서 수신한 데이터를 리치 텍스트박스에 출력한다.





1

2

3

4

5

6

7

8

9

10

11

12

13

14

15

16

17

18

19

20

21

22

23

24

25

26

27

28

29

30

31

32

33

34

35

36

37

38

39

40

41

42

43

44

45

46

47

48

49

50

51

52

53

54

55

56

57

58

59

60

61

62

63

64 


using System.Net.Sockets;

using System.Threading;

 

public partial class Form1 : Form

{

    private TcpClient server;

    private NetworkStream ns;

    private bool isRunning = true;



    public Form1()

    {

        InitializeComponent();

    }



    private void Form1_Load(object sender, EventArgs e)

    {

        try

        {

            server = new TcpClient("127.0.0.1", 12345);

            ns = server.GetStream();

            Thread recvThread = new Thread(new ThreadStart(RecvThread));

            recvThread.Start();

        }

        catch (SocketException)

        {

            MessageBox.Show("서버와의 연결에 실패했습니다.");

        }

    }



    delegate void LogToForm(string msg);

    private void Log(string msg)

    {

        serverMessage.AppendText(msg);

        serverMessage.ScrollToCaret();

    }



    public void RecvThread()

    {

        byte[] buffer = new byte[1024];

        string msg;



        while (isRunning)

        {

            try

            {

                ns.Read(buffer, 0, buffer.Length);

                msg = Encoding.ASCII.GetString(buffer);

                serverMessage.Invoke(new LogToForm(Log), new object[] { msg });

            }

            catch (Exception ex)

            {

                Console.WriteLine(ex.Message);

            }

        }

    }



    private void Form1_FormClosing(object sender, FormClosingEventArgs e)

    {

        isRunning = false;



        ns.Close();

        server.Close();

    }

}






출처: https://devvcxxx.tistory.com/31 [개발 관련]




///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




출처: https://trip2ee.tistory.com/23




요즘 원래 본업에서 많이 벗어나 C#과 Windows Mobile로 계속 삽질을 하고있다. 가끔씩 내가 왜 이러고 있는건가? 하는 생각이 들기도 하지만 나름 재미있기도 하다. 이번엔 MSDN을 참고하며 C#으로 socket programming 삽질을 해 보았다. 빨리 이 삽질을 끝내고 본업으로 돌아갔으면 좋겠다.

Socket 프로그래밍을 위한 namespace
Socket을 초기화하기 위해 사용할 namespace는 다음과 같다.
using System.Net;
using System.Net.Sockets;
using System.Threading;

System.Net.Sockets 는 Socket 클래스를 사용하기 위함이며 System.Net 은 IPAddress 와 IPEndPoint 클래스를 사용하기 위함이다.

Server
서버는 특정 포트를 열고 클라이언트의 접속을 기다리다가(Listen) 클라이언트가 접속을 시도하면 Accept해준다.IPAddress 클래스로 host IP를 나타내며 IPEndPoint 클래스로 hostIP와 서버의 포트번호를 지정한 후 server socket에 bind 한다.그리고 Listen() 을 수행하면 서버는 클라이언트로부터 접속을 기다릴 준비가 된 것이다.이 때 Listen() 함수의 인자는 접속가능한 클라이언트의 최대 갯수이며 이 코드에서는 하나의 클라이언트만 접속을 허용한다.그리고 server.Blocking = ture 는 server socket이 blocking mode에서 동작한다는 것으로 Accept() 와 Receive() 함수가 blocking 함수로 동작한다.
한가지 중요한 것은 클라이언트가 서버에 접속할 때 서버를 초기화 할 때 입력해 준 host IP로 접속해야 한다는 것이다.내부적으로 테스트를 위한 것이라면 strIP 대신 "127.0.0.1" 을 입력해 주고 클라이언트에서 "127.0.0.1" 로 접속을 해도 되지만 그렇지 않은 경우는 꼭 Dns.GetHostName() 과 Dns.Resolve() 를 이용해 host IP를 알아내는 것이 좋다.

Socket server;
int port = 8000;











try

{

    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);



IPHostEntry host = Dns.Resolve(Dns.GetHostName());

string strIP = host.AddressList[0].ToString();         // Get the local IP address

IPAddress hostIP = IPAddress.Parse(strIP);



IPEndPoint ep = new IPEndPoint(hostIP, port);

server.Bind(ep);

    server.Blocking = true;     // The server socket is working in blocking mode

    server.Listen(1);

}

catch (SocketException exc)

{

    MessageBox.Show(exc.ToString());

}








이제 서버는 클라이언트가 접속하기를 기다리며 접속을 시도하면 Accept() 그리고 클라이언트에서 데이터를 보내오면 Receive() 해야한다.하지만 접속시도와 데이터를 받아옴을 알려주는 이벤트가 없기 때문에 계속해서 polling 방식으로 확인을 해야한다.저 초기화 루틴에서 무한루프를 돌며 polling을 한다면 프로그램 자체가 응답을 하지 못하기 때문에 별도의 thread를 생성해 이러한 일을 해야한다.
C#에서 thread를 사용하는 방법은 다음과 같다. Thread 클래스 인스턴스를 선언하고 생성자에서 ServerProc() 을 thread procedure를 정의해 준 후 Start() method를 호출해 thread를 시작한다.
Server에서 client와 데이터를 주고 받을 때는 server에서 client를 accept하고 연결을 할당해 준 소켓을 통해서 이루어진다.아래 예에서는 나오지 않았지만 client로 데이터를 전송하기 위해서는 client.Send() method를 이용한다.

Thread threadServer;
threadServer = new Thread(new ThreadStart(ServerProc));
threadServer.Start();

그리고 ServerProc() 에서는 다음과 같이 client의 접속과 데이터 전송을 기다린다.






public void ServerProc()

{

    while (true)

    {

        // If a client is not connected

        if (bConnected == false)

        {

            client = server.Accept();

            bConnected = true;

        }

        // If a client is connected, wait for data from client

        else

        {

            int len;

            try

            {

                len = client.Receive(buffer);

                if (len == 0)       // If the client is disconnected

                {

                    bConnected = false;



                    client.Disconnect(true);

                    this.Invoke(new EventHandler(UpdateServerUI));

                }

                else                // If data from client is arrived

                {

                    received = System.Text.Encoding.ASCII.GetString(buffer);

                    this.Invoke(new EventHandler(UpdateServerUI));

                }

            }

            catch (SocketException exc)

            {

                MessageBox.Show(exc.ToString());

                bConnected = false;

            }

        }

    }

}







Accept()는 blocking method이기 때문에 client에서 접속하기를 기다리다 client가 접속을 하면 client socket에 연결을 할당을 해 주고 다음 명령으로 넘어간다.마찬가지로 Receive() method 도 blocking method로 데이터가 도착할 때 까지 기다리다가 데이터가 도착하면 데이터의 길이와 데이터를 출력하고 다음 명령으로 넘어간다.
Socket 클래스가 명시적으로 제공하지 않는것 중 하나는 client가 접속을 했다가 접속을 끊는것을 알아내는 method나 property이다. MSDN에서는 Receive() method 에서 다음과 같이 설명하는 부분이 있다.

If the remote host shuts down the Socket connection with the Shutdown method, and all available data has been received, the Receive method will complete immediately and return zero bytes.

즉 client가 접속을 끊으면 Receive() method는 0을 return 한다.따라서 위 코드에서 len 이 0인 경우 연결을 해제하고 bConnected flag를 false로 설정하여 다시 다른 client가 접속하기를 기다린다.
또한 client 프로그램이 올바르게 접속을 종료하지 않고 종료되는 경우 SocketException이 발생하기 때문에 이에 대한 예외처리를 해 주었다.그리고 socket으로 받은 데이터를 main Form 의 컨트롤에 표시해주기 위해 UI를 업데이트하는 함수를 invoke해 주었다.main Form과 socket을 위한 thread는 서로 다른것이기 때문에 socket의 thread 에서 main Form의 데이터를 임의로 변경하는 것이 허용되지 않기 때문에 이러한 방법을 사용한다.

통신이 종료되면 다음과 같이 소켓을 닫고 thread도 종료한다.
server.Close();
threadServer.Abort();

Client
Client가 서버로 접속하기 위해서는 Connect() method로 접속할 host와 포트번호를 지정해 주면 된다.다음 코드는 txtAddress 라는 TextBox 에 입력된 IP주소로 위 서버의 예와 같이 8000번 포트로 접속한다.

Socket client;
client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
client.Connect(IPAddress.Parse(txtAddress.Text), 8000);

서버에 접속이 되었다면 다음과 같이 Send() method를 이용해 서버로 데이터를 전송할 수 있다.

byte[] buffer = System.Text.Encoding.ASCII.GetBytes(txtTransmit.Text);
client.Send(buffer);
txtTransmit.Text = "";

그리고 접속 종료시는 Disconnect() method를 이용해 연결을 끊는다.이 때 함수 인자는 현재 연결을 종료 후 이 소켓을 다시 사용할지에 대한 flag이다.

client.Disconnect(false);

실행 결과
다음 화면과 같이 접속을 하고 Hello, Server 라고 보내주면





client가 접속시 accepted 라는 메세지를 출력하고 Hello, Server라는 데이터를 받아 화면에 보여주어 통신이 성공적으로 이루어졌음을 보여준다.





이 예제는 매우 간단한 테스트를 위한 코드이며 client에도 server와 같이 thread 가 돌아가며 서버에서 오는 데이터를 polling 하는 부분이 추가되어야 원활한 양방향 통신을 할 수 있다.

Windows Mobile 을 위한 porting
Windows Mobile에서의 동작을 위해서는 다음 사항을 변경해야 한다.

1.Client 에서 server로 접속시에 Connect() method 의 인자는 IPEndPoint만을 받기 때문에 다음과 같이 수정되어야 한다.
IPEndPoint ep = new IPEndPoint(IPAddress.Parse(txtAddress.Text), 8000);
client.Connect(ep);

2. socket.Disconnect() method가 없다.대신 Shutdown() method를 이용한다.

3. byte[] 에서 string으로 변환하기 위한 유용한 툴인 System.Text.Encoding.ASCII.GetString() 함수의 인자가 다르다. byte[], 시작 인덱스, 문자열 길이가 필요하다.
System.Text.Encoding.ASCII.GetString(buffer, 0, len);

그 외의 부분은 모두.NET compact framework에서도 호환되는 것이기 때문에 모바일 플랫폼에서도 잘 동작한다.다음은 Windows Mobile 6.0 Professional 이 설치된 스마트폰 GB-P100 에서 서버와 통신하는 화면을 캡쳐한 것이다.이 프로그램은 위에서 설명한 Client에 thread를 추가해 Receive() method로 계속해서 데이터를 받아 아래 화면에 표시를 해 준다.




화면에 가려진 부분은 IP 주소 부분이다.

참고자료
[1] http://msdn.microsoft.com/en-us/library/system.net.sockets.socket.aspx
[2] http://msdn.microsoft.com/en-us/library/system.net.ipaddress(VS.71).aspx
[3] http://msdn.microsoft.com/en-us/library/system.threading.thread.aspx


출처: https://trip2ee.tistory.com/23 [지적(知的) 탐험]










///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




#기타관련링크

- http://nas.neostack.co.kr/wordpress/2018/07/09/c-socket-%ED%86%B5%EC%8B%A0-%EC%98%88%EC%A0%9C-server-client/
 
*/
