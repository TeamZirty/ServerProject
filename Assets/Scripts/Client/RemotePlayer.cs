// -------------------------------
// 📜 스크립트 이름: RemotePlayer.cs
// 👥 기능: 서버에서 받은 다른 유저의 위치/무기 회전/HP 상태를 화면에 표시하는 스크립트
// -------------------------------

using UnityEngine;

public class RemotePlayer : MonoBehaviour
{
    public Transform weaponTransform;

    public void UpdateState(float x, float y, float weaponAngle)
    {
        transform.position = new Vector2(x, y);
        weaponTransform.rotation = Quaternion.Euler(0, 0, weaponAngle);
    }
}
