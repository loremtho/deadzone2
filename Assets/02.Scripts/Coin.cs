using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 10f; // 코인 회전 속도
    // Update is called once per frame
    void Update()
    {
        // 코인 회전
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
