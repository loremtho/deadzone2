
using System.Collections;
using System.Collections.Generic;
using TempleRun.Player;
using UnityEngine;
using UnityEngine.UIElements;

public class TileMannger : MonoBehaviour
{
    [SerializeField] private List<GameObject> tilePrefabs; // 랜덤으로 생성할 타일의 종류를 저장할 리스트.
    [SerializeField] private GameObject player; // 플레이어 객체.

    private Vector3 nextSpawnPoint; // 다음 타일을 생성할 위치.
    private float playerSpeed; // 플레이어의 속도.
    private float lastTileEndPositionZ = -80f; // 마지막 타일의 끝 위치.

    private const float TILE_LENGTH = 80f; // 타일의 길이.

    void Start()
    {
        // 초기 속도를 플레이어의 속도로 설정.
        playerSpeed = player.GetComponent<PlayerController>().initialPlayerSpeed;

        // 시작할 때 타일을 생성.
        for (int i = 0; i < 5; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // 플레이어의 속도를 가져옴.
        playerSpeed = player.GetComponent<PlayerController>().playerSpeed;

        // 다음 타일을 생성할 위치가 플레이어보다 멀어지면 타일을 생성하고 마지막 타일을 삭제.
        if (nextSpawnPoint.z - player.transform.position.z < 300f)
        {
            SpawnTile();
            DeleteTile();
        }
    }

    // 타일 생성 함수.
    private void SpawnTile()
    {
        // 랜덤으로 타일을 선택.
        GameObject tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];

        // 다음 타일을 생성할 위치 계산.
        nextSpawnPoint.z += TILE_LENGTH;

        // 타일 생성.
        GameObject tile = Instantiate(tilePrefab, nextSpawnPoint, Quaternion.identity);

        // 타일을 이 스크립트의 자식으로 설정.
        tile.transform.SetParent(transform);

        // 타일의 끝 위치를 계산하여 저장.
        // lastTileEndPositionZ = nextSpawnPoint.z + TILE_LENGTH;
        lastTileEndPositionZ = 80f;
    }

    // 타일 삭제 함수.
    private void DeleteTile()
    {
        // 플레이어의 위치가 마지막 타일의 끝 위치보다 멀어지면 삭제합니다.
        if (player.transform.position.z > lastTileEndPositionZ + 100f)
        {
            Transform oldestTile = transform.GetChild(0);
            transform.GetChild(0).SetParent(null);
            Destroy(oldestTile.gameObject);
        }
    }

}







/*
using System.Collections;
using System.Collections.Generic;
using TempleRun.Player;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> straightTilePrefabs; // 직선 타일 프리팹 리스트.
    [SerializeField] private List<GameObject> turnTilePrefabs; // 턴 타일 프리팹 리스트.
    [SerializeField] private GameObject player; // 플레이어 객체.

    private Vector3 nextSpawnPoint; // 다음 타일을 생성할 위치.
    private float playerSpeed; // 플레이어의 속도.
    private float lastTileEndPositionZ = -80f; // 마지막 타일의 끝 위치.

    private const float TILE_LENGTH = 80f; // 타일의 길이.

    void Start()
    {
        // 초기 속도를 플레이어의 속도로 설정.
        playerSpeed = player.GetComponent<PlayerController>().initialPlayerSpeed;

        // 시작할 때 타일을 생성.
        for (int i = 0; i < 5; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // 플레이어의 속도를 가져옴.
        playerSpeed = player.GetComponent<PlayerController>().playerSpeed;

        // 다음 타일을 생성할 위치가 플레이어보다 멀어지면 타일을 생성하고 마지막 타일을 삭제.
        if (nextSpawnPoint.z - player.transform.position.z < 60f)
        {
            SpawnTile();
            DeleteTile();
        }
    }

    // 타일 생성 함수.
    private void SpawnTile()
    {
        GameObject tilePrefab;
        Vector3 nextTilePosition;

        // 이전 타일이 턴 타일인 경우 다음 타일은 직선 타일로 생성.
        if (transform.childCount > 0 && transform.GetChild(transform.childCount - 1).CompareTag("TurnTile"))
        {
            tilePrefab = straightTilePrefabs[Random.Range(0, straightTilePrefabs.Count)];
            nextTilePosition = nextSpawnPoint + new Vector3(0, 0, TILE_LENGTH);
        }
        // 이전 타일이 직선 타일인 경우 다음 타일은 턴 타일로 생성.
        else if (transform.childCount > 0 && transform.GetChild(transform.childCount - 1).CompareTag("StraightTile"))
        {
            tilePrefab = turnTilePrefabs[Random.Range(0, turnTilePrefabs.Count)];
            nextTilePosition = nextSpawnPoint + new Vector3(0, 0, TILE_LENGTH / 2f);
        }
        // 첫 타일은 직선 타일로 생성.
        else
        {
            tilePrefab = straightTilePrefabs[Random.Range(0, straightTilePrefabs.Count)];
            nextTilePosition = nextSpawnPoint + new Vector3(0, 0, TILE_LENGTH);
        }

        // 타일 생성.
        GameObject tile = Instantiate(tilePrefab, nextTilePosition, Quaternion.identity);

        // 타일을 이 스크립트의 자식으로 설정.
        tile.transform.SetParent(transform);

        // 다음 타일을 생성할 위치를 갱신.
        nextSpawnPoint = tile.transform.GetChild(0).transform.Find("SpawnPoint");
        */


