using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject[] platformPrefabs;         // �ٴ� ������ �迭
    public float platformSpeed = 2f;            // �ٴ� �̵� �ӵ�
    public float platformDistance = 10f;        // �ٴ� ���� �Ÿ�
    public float destroyDistance = -20f;        // �ٴ� ���� ��ġ

    private Vector3 lastPosition;               // ������ ���� ��ġ
    private Vector3 playerDirection;            // ĳ���� ����
    private float platformLength;               // �ٴ� ����
    private List<GameObject> platforms;         // ������ �ٴ� ����Ʈ

    private void Start()
    {
        // �ʱ�ȭ
        lastPosition = transform.position;
        playerDirection = Vector3.back;
        platformLength = GetPlatformLength();
        platforms = new List<GameObject>();

        // ù��° �ٴ� ����
        CreateNextPlatform();
    }

    private void Update()
    {
        // �ٴ� ����
        PlatformCleanup();
    }

    private void CreateNextPlatform()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceFromPlayer = platformDistance + platformLength;
        Vector3 nextPlatformPosition = lastPosition + playerDirection * distanceFromPlayer;

        // ���ο� �ٴ� ����
        GameObject newPlatform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], nextPlatformPosition, Quaternion.identity);

        // ������ ��ġ ����
        lastPosition = newPlatform.transform.position;

        // ������ �ٴ��� ����Ʈ�� �߰�
        platforms.Add(newPlatform);
    }

    private IEnumerator CreateNextPlatformWithDelay()
    {
        yield return new WaitForSeconds(1f);
        CreateNextPlatform();
    }

    private void PlatformCleanup()
    {
        // �ٴ��� ���� �Ÿ��� �Ѿ�� ����
        if (platforms.Count > 0 && platforms[0].transform.position.z < destroyDistance)
        {
            Destroy(platforms[0]);
            platforms.RemoveAt(0);
        }
    }

    private float GetPlatformLength()
    {
        // ù��° �ٴ� �������� ���̸� ��ȯ
        return platformPrefabs[0].GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
    }

    private void LateUpdate()
    {
        // ��� �ٴ��� �̵�
        foreach (GameObject platform in platforms)
        {
            platform.transform.Translate(playerDirection * Time.deltaTime * platformSpeed, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ �ٴ��� �߾ӿ� ����� �� ���� �ٴ� ����
        if (other.CompareTag("Player"))
        {
            CreateNextPlatform();
        }
    }
}
