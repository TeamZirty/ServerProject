// -------------------------------
// 📜 스크립트 이름: CameraFollow.cs
// 🎥 기능: 카메라가 지정된 플레이어를 일정 거리에서 따라가며 회전
// -------------------------------

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;              // 따라갈 대상
    public Vector3 offset = new Vector3(0f, 10f, -10f); // 카메라 위치 오프셋
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.LookAt(target); // 플레이어 바라보기
    }
}
