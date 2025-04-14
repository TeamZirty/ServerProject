// -------------------------------
// 📜 스크립트 이름: PlayerController.cs
// 🎮 기능: 로컬 플레이어가 키보드로 이동하고, 마우스로 무기를 회전하며,
//        주기적으로 자신의 상태를 서버에 전송하는 스크립트
// -------------------------------

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform weaponTransform;

    void Update()
    {
        // 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v).normalized;
        transform.Translate(dir * speed * Time.deltaTime);

        // 무기 회전
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);

        // 서버 전송
        TCPClientHandler.Instance.SendPlayerData(transform.position.x, transform.position.y, angle);
    }
}
