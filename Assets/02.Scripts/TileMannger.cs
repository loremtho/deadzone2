
using System.Collections;
using System.Collections.Generic;
using TempleRun.Player;
using UnityEngine;
using UnityEngine.UIElements;

public class TileMannger : MonoBehaviour
{
    [SerializeField] private List<GameObject> tilePrefabs; // �������� ������ Ÿ���� ������ ������ ����Ʈ.
    [SerializeField] private GameObject player; // �÷��̾� ��ü.

    private Vector3 nextSpawnPoint; // ���� Ÿ���� ������ ��ġ.
    private float playerSpeed; // �÷��̾��� �ӵ�.
    private float lastTileEndPositionZ = -80f; // ������ Ÿ���� �� ��ġ.

    private const float TILE_LENGTH = 80f; // Ÿ���� ����.

    void Start()
    {
        // �ʱ� �ӵ��� �÷��̾��� �ӵ��� ����.
        playerSpeed = player.GetComponent<PlayerController>().initialPlayerSpeed;

        // ������ �� Ÿ���� ����.
        for (int i = 0; i < 5; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // �÷��̾��� �ӵ��� ������.
        playerSpeed = player.GetComponent<PlayerController>().playerSpeed;

        // ���� Ÿ���� ������ ��ġ�� �÷��̾�� �־����� Ÿ���� �����ϰ� ������ Ÿ���� ����.
        if (nextSpawnPoint.z - player.transform.position.z < 300f)
        {
            SpawnTile();
            DeleteTile();
        }
    }

    // Ÿ�� ���� �Լ�.
    private void SpawnTile()
    {
        // �������� Ÿ���� ����.
        GameObject tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];

        // ���� Ÿ���� ������ ��ġ ���.
        nextSpawnPoint.z += TILE_LENGTH;

        // Ÿ�� ����.
        GameObject tile = Instantiate(tilePrefab, nextSpawnPoint, Quaternion.identity);

        // Ÿ���� �� ��ũ��Ʈ�� �ڽ����� ����.
        tile.transform.SetParent(transform);

        // Ÿ���� �� ��ġ�� ����Ͽ� ����.
        // lastTileEndPositionZ = nextSpawnPoint.z + TILE_LENGTH;
        lastTileEndPositionZ = 80f;
    }

    // Ÿ�� ���� �Լ�.
    private void DeleteTile()
    {
        // �÷��̾��� ��ġ�� ������ Ÿ���� �� ��ġ���� �־����� �����մϴ�.
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
    [SerializeField] private List<GameObject> straightTilePrefabs; // ���� Ÿ�� ������ ����Ʈ.
    [SerializeField] private List<GameObject> turnTilePrefabs; // �� Ÿ�� ������ ����Ʈ.
    [SerializeField] private GameObject player; // �÷��̾� ��ü.

    private Vector3 nextSpawnPoint; // ���� Ÿ���� ������ ��ġ.
    private float playerSpeed; // �÷��̾��� �ӵ�.
    private float lastTileEndPositionZ = -80f; // ������ Ÿ���� �� ��ġ.

    private const float TILE_LENGTH = 80f; // Ÿ���� ����.

    void Start()
    {
        // �ʱ� �ӵ��� �÷��̾��� �ӵ��� ����.
        playerSpeed = player.GetComponent<PlayerController>().initialPlayerSpeed;

        // ������ �� Ÿ���� ����.
        for (int i = 0; i < 5; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // �÷��̾��� �ӵ��� ������.
        playerSpeed = player.GetComponent<PlayerController>().playerSpeed;

        // ���� Ÿ���� ������ ��ġ�� �÷��̾�� �־����� Ÿ���� �����ϰ� ������ Ÿ���� ����.
        if (nextSpawnPoint.z - player.transform.position.z < 60f)
        {
            SpawnTile();
            DeleteTile();
        }
    }

    // Ÿ�� ���� �Լ�.
    private void SpawnTile()
    {
        GameObject tilePrefab;
        Vector3 nextTilePosition;

        // ���� Ÿ���� �� Ÿ���� ��� ���� Ÿ���� ���� Ÿ�Ϸ� ����.
        if (transform.childCount > 0 && transform.GetChild(transform.childCount - 1).CompareTag("TurnTile"))
        {
            tilePrefab = straightTilePrefabs[Random.Range(0, straightTilePrefabs.Count)];
            nextTilePosition = nextSpawnPoint + new Vector3(0, 0, TILE_LENGTH);
        }
        // ���� Ÿ���� ���� Ÿ���� ��� ���� Ÿ���� �� Ÿ�Ϸ� ����.
        else if (transform.childCount > 0 && transform.GetChild(transform.childCount - 1).CompareTag("StraightTile"))
        {
            tilePrefab = turnTilePrefabs[Random.Range(0, turnTilePrefabs.Count)];
            nextTilePosition = nextSpawnPoint + new Vector3(0, 0, TILE_LENGTH / 2f);
        }
        // ù Ÿ���� ���� Ÿ�Ϸ� ����.
        else
        {
            tilePrefab = straightTilePrefabs[Random.Range(0, straightTilePrefabs.Count)];
            nextTilePosition = nextSpawnPoint + new Vector3(0, 0, TILE_LENGTH);
        }

        // Ÿ�� ����.
        GameObject tile = Instantiate(tilePrefab, nextTilePosition, Quaternion.identity);

        // Ÿ���� �� ��ũ��Ʈ�� �ڽ����� ����.
        tile.transform.SetParent(transform);

        // ���� Ÿ���� ������ ��ġ�� ����.
        nextSpawnPoint = tile.transform.GetChild(0).transform.Find("SpawnPoint");
        */


