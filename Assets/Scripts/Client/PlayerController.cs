// -------------------------------
// 📜 스크립트 이름: PlayerController.cs
// 🎮 기능: 키보드로 이동하고, 마우스로 바라보는 방향으로 몸체를 회전하며,
//        무기는 자식 오브젝트로 따라 회전됨. 위치/회전 서버 전송 포함
// -------------------------------

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform weaponTransform;

    void Update()
    {
        // 🔼 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = new Vector3(h, 0f, v).normalized;
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // 🎯 마우스 방향 계산 → 회전
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0f; // 수평 회전만
            if (lookDir.magnitude > 0.1f)
            {
                Quaternion rot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);

                // 무기도 같이 회전
                if (weaponTransform != null)
                    weaponTransform.rotation = rot;
            }

            // ✅ 서버 전송
            TCPClientHandler.Instance.SendPlayerData(transform.position.x, transform.position.z, transform.eulerAngles.y);
        }
    }
}
