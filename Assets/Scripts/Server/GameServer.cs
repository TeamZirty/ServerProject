// -------------------------------
// 📜 스크립트 이름: GameServer.cs
// 🧠 기능: TCP 서버를 열어 클라이언트와의 실시간 통신을 처리하고,
//        클라이언트 위치/무기 회전 정보를 받아 충돌 및 데미지 판정을 수행하는 메인 서버 스크립트
// -------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json; // Newtonsoft.Json 패키지 필요 (JSON 직렬화)

class GameServer
{
    private static TcpListener listener;
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static object locker = new object(); // 동기화를 위한 락
    private const int PORT = 7777;

    static void Main(string[] args)
    {
        listener = new TcpListener(IPAddress.Any, PORT);
        listener.Start();
        Console.WriteLine($"✅ 서버 시작됨. 포트 {PORT}에서 대기 중...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Thread t = new Thread(() => HandleClient(client));
            t.Start();
        }
    }

    // 💡 클라이언트 하나 처리
    private static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        string playerId = Guid.NewGuid().ToString(); // 고유 플레이어 ID 생성
        Console.WriteLine($"🟢 새 클라이언트 접속됨: {playerId}");

        Player myPlayer = new Player(playerId);
        lock (locker)
        {
            players[playerId] = myPlayer;
        }

        byte[] buffer = new byte[4096];

        while (client.Connected)
        {
            try
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0) break;

                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                NetworkMessage msg = JsonConvert.DeserializeObject<NetworkMessage>(json);

                // 🔁 받은 위치/무기 회전 정보 저장
                lock (locker)
                {
                    myPlayer.UpdateState(msg);
                    GameLogic.CheckCollisionAndApplyDamage(myPlayer, players);
                }

                // 📡 모든 플레이어 상태를 다시 보내줌
                string responseJson = JsonConvert.SerializeObject(players.Values);
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseJson);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 오류: {ex.Message}");
                break;
            }
        }

        // 🔴 연결 끊긴 경우
        lock (locker)
        {
            players.Remove(playerId);
        }
        Console.WriteLine($"🔴 클라이언트 연결 종료: {playerId}");
        client.Close();
    }
}
