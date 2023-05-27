using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject[] platformPrefabs;         // 바닥 프리팹 배열
    public float platformSpeed = 2f;            // 바닥 이동 속도
    public float platformDistance = 10f;        // 바닥 생성 거리
    public float destroyDistance = -20f;        // 바닥 제거 위치

    private Vector3 lastPosition;               // 마지막 생성 위치
    private Vector3 playerDirection;            // 캐릭터 방향
    private float platformLength;               // 바닥 길이
    private List<GameObject> platforms;         // 생성된 바닥 리스트

    private void Start()
    {
        // 초기화
        lastPosition = transform.position;
        playerDirection = Vector3.back;
        platformLength = GetPlatformLength();
        platforms = new List<GameObject>();

        // 첫번째 바닥 생성
        CreateNextPlatform();
    }

    private void Update()
    {
        // 바닥 제거
        PlatformCleanup();
    }

    private void CreateNextPlatform()
    {
        // 플레이어와의 거리 계산
        float distanceFromPlayer = platformDistance + platformLength;
        Vector3 nextPlatformPosition = lastPosition + playerDirection * distanceFromPlayer;

        // 새로운 바닥 생성
        GameObject newPlatform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], nextPlatformPosition, Quaternion.identity);

        // 마지막 위치 갱신
        lastPosition = newPlatform.transform.position;

        // 생성된 바닥을 리스트에 추가
        platforms.Add(newPlatform);
    }

    private IEnumerator CreateNextPlatformWithDelay()
    {
        yield return new WaitForSeconds(1f);
        CreateNextPlatform();
    }

    private void PlatformCleanup()
    {
        // 바닥이 일정 거리를 넘어가면 삭제
        if (platforms.Count > 0 && platforms[0].transform.position.z < destroyDistance)
        {
            Destroy(platforms[0]);
            platforms.RemoveAt(0);
        }
    }

    private float GetPlatformLength()
    {
        // 첫번째 바닥 프리팹의 길이를 반환
        return platformPrefabs[0].GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
    }

    private void LateUpdate()
    {
        // 모든 바닥을 이동
        foreach (GameObject platform in platforms)
        {
            platform.transform.Translate(playerDirection * Time.deltaTime * platformSpeed, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 바닥의 중앙에 닿았을 때 다음 바닥 생성
        if (other.CompareTag("Player"))
        {
            CreateNextPlatform();
        }
    }
}
