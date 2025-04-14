// -------------------------------
// 📜 스크립트 이름: TCPClientHandler.cs
// 🔌 기능: 유니티에서 서버와 TCP 연결을 유지하고, 위치/무기 회전 데이터를 보내며,
//        서버로부터 다른 유저들의 상태를 받아 처리하는 클래스
// -------------------------------

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class TCPClientHandler : MonoBehaviour
{
    public string serverIP = "127.0.0.1";
    public int serverPort = 7777;

    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    public static TCPClientHandler Instance;
    public Action<List<PlayerState>> OnDataReceived;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();

            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log("🟢 서버 연결 성공");
        }
        catch (Exception e)
        {
            Debug.LogError("❌ 서버 연결 실패: " + e.Message);
        }
    }

    public void SendPlayerData(float x, float y, float weaponAngle)
    {
        if (stream == null) return;

        PlayerMessage msg = new PlayerMessage { x = x, y = y, weaponAngle = weaponAngle };
        string json = JsonUtility.ToJson(msg);
        byte[] data = Encoding.UTF8.GetBytes(json);
        stream.Write(data, 0, data.Length);
    }

    void ReceiveData()
    {
        while (true)
        {
            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0) continue;

                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                PlayerStateList wrapper = JsonUtility.FromJson<PlayerStateList>("{\"players\":" + json + "}");
                OnDataReceived?.Invoke(wrapper.players);
            }
            catch (Exception e)
            {
                Debug.LogError("수신 오류: " + e.Message);
                break;
            }
        }
    }

    [Serializable]
    public class PlayerMessage
    {
        public float x;
        public float y;
        public float weaponAngle;
    }

    [Serializable]
    public class PlayerState
    {
        public string id;
        public float x;
        public float y;
        public float weaponAngle;
        public int hp;
    }

    [Serializable]
    public class PlayerStateList
    {
        public List<PlayerState> players;
    }
}
