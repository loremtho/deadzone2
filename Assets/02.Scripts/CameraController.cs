using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform
    public Vector3 offset; // ī�޶� ��ġ���� �÷��̾������ �Ÿ�
    public float zoomSpeed = 2f; // ī�޶� �� �ӵ�
    public float minZoom = 5f; // ī�޶� �ּ� �� �Ÿ�
    public float maxZoom = 15f; // ī�޶� �ִ� �� �Ÿ�
    public float pitch = 2f; // ī�޶� ȸ�� ����

    private float currentZoom = 10f; // ���� ī�޶� �� �Ÿ�

    void LateUpdate()
    {
        // �÷��̾� ��ġ�� ������ �Ÿ�(offset) ����
        transform.position = target.position - offset * currentZoom;

        // ī�޶� ȸ��
        transform.LookAt(target.position + Vector3.up * pitch);

        // ī�޶� �� ��/�ƿ�
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }
}
