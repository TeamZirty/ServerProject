// -------------------------------
// 📜 스크립트 이름: GameManager.cs
// 🧠 기능: 서버에서 받은 모든 유저 정보를 바탕으로 로컬/리모트 유저 오브젝트를 관리하는 매니저
// -------------------------------

using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject remotePlayerPrefab;
    public Transform remotePlayerContainer;

    private Dictionary<string, RemotePlayer> players = new Dictionary<string, RemotePlayer>();

    void Start()
    {
        TCPClientHandler.Instance.OnDataReceived += OnDataReceived;
    }

    void OnDataReceived(List<TCPClientHandler.PlayerState> serverPlayers)
    {
        foreach (var state in serverPlayers)
        {
            if (!players.ContainsKey(state.id))
            {
                GameObject obj = Instantiate(remotePlayerPrefab, remotePlayerContainer);
                players[state.id] = obj.GetComponent<RemotePlayer>();
            }

            players[state.id].UpdateState(state.x, state.y, state.weaponAngle);
        }
    }
}
