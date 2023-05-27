using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform
    public Vector3 offset; // 카메라 위치에서 플레이어까지의 거리
    public float zoomSpeed = 2f; // 카메라 줌 속도
    public float minZoom = 5f; // 카메라 최소 줌 거리
    public float maxZoom = 15f; // 카메라 최대 줌 거리
    public float pitch = 2f; // 카메라 회전 각도

    private float currentZoom = 10f; // 현재 카메라 줌 거리

    void LateUpdate()
    {
        // 플레이어 위치와 일정한 거리(offset) 유지
        transform.position = target.position - offset * currentZoom;

        // 카메라 회전
        transform.LookAt(target.position + Vector3.up * pitch);

        // 카메라 줌 인/아웃
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }
}
