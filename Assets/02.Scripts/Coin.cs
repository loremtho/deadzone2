using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 10f; // ���� ȸ�� �ӵ�
    // Update is called once per frame
    void Update()
    {
        // ���� ȸ��
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
