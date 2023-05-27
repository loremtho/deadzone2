
using System.Collections.Generic;
using UnityEngine;


namespace TempleRun
{

    public class TileSpawer : MonoBehaviour
    {
        [SerializeField]
        private int tileStartCount = 10;  // ó�� ������ Tile�� ������ ���ϴ� ����
        [SerializeField]
        private int minimumStraightTiles = 3;   // ������ Tile ������ �ּ� ������ ���ϴ� ����
        [SerializeField]
        private int maximumStraightTiles = 15;   // ������ Tile ������ �ִ� ������ ���ϴ� ����
        [SerializeField]
        private GameObject startingTile;    // ���� ������ �� ������ Tile
        [SerializeField]
        private List<GameObject> turnTiles;   // ȸ�� Tile�� ������ ��� �ִ� GameObject List
        [SerializeField]
        private List<GameObject> obstacles;  // ��ֹ��� ������ ��� �ִ� GameObject List

        [SerializeField]
        private List<GameObject> items;  // �������� ������ ��� �ִ� GameObject List

        private Vector3 currentTileLocation = Vector3.zero;  // ���� Tile�� ��ġ�� ��� �ִ� ����
        private Vector3 currentTileDirection = Vector3.forward; //���� Tile�� ������ ��� �ִ� ����
        private GameObject prevTile;

        private List<GameObject> currentTiles;  // ���� ������ Tile���� ��� �ִ� GameObject List
        private List<GameObject> currentobstacles;  // ���� ������ ��ֹ����� ��� �ִ� GameObject List


        float zOffset = 10f;  // ���ο� Ÿ�� ���� ��ġ�� z ��


        private void Start()
        {

 
            currentTiles = new List<GameObject>();  // ���� ������ Tile���� ���� List�� �ʱ�ȭ
            currentobstacles = new List<GameObject>();  // ���� ������ ��ֹ����� ���� List�� �ʱ�ȭ
  

            Random.InitState(System.DateTime.Now.Millisecond);  // �ð��� �������� ������ ���� �����ϱ� ���� �ʱ�ȭ

            for (int i =0; i< tileStartCount; ++i)
            {
                SpawnTile(startingTile.GetComponent<Tile>()); // ���� Tile�� ����
            }    

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>());  // ȸ�� Tile�� ����


        }

    


        private void SpawnTile(Tile tile, bool spawnObstacle = false )  // Tile�� �����ϴ� �Լ�
        {
            Quaternion newTileRotation = tile.gameObject.transform.rotation* Quaternion.LookRotation(currentTileDirection, Vector3.up); // ���ο� Tile�� ȸ�� ���� ���
            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation); // Tile�� ����
            currentTiles.Add(prevTile);  // ������ Tile�� List�� �߰�

           

            if (spawnObstacle) SpawnObstacle();  // ��ֹ��� ����

  

            if (tile.type == TileType.STRAIGHT)   // Tile�� ������ Tile�� ���
                currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection);  // ���� ��ġ�� ������ �������� �̵�

        }

        private void DeletePreviousTiles()  // ������ ������ Tile�� �����ϴ� �Լ�
        {
            while (currentTiles.Count != 1)  // currentTiles ����Ʈ�� Ÿ���� 1�� ���� ������ ����
            {
                GameObject tile = currentTiles[0];   // ����Ʈ�� ù ��° Ÿ���� �����ͼ� ����
                currentTiles.RemoveAt(0);
                Destroy(tile);             // ������ Ÿ�� ��ü�� �ı�
            }

            while (currentobstacles.Count != 0)   // currentobstacles ����Ʈ�� ��ֹ��� �����ϴ� ���� �ݺ�
            {
                GameObject obstacle = currentobstacles[0];   // ����Ʈ�� ù ��° ��ֹ��� �����ͼ� ����
                currentobstacles.RemoveAt(0);
                Destroy(obstacle);     // ������ ��ֹ� ��ü�� �ı�
            }

        }

        public void AddNewDirection(Vector3 direction)   // AddNewDirection �Լ��� ���ο� ������ �޾ƿͼ� Ÿ�ϰ� ��ֹ��� �����ϴ� �Լ�
        { 
            currentTileDirection = direction;  // ���ο� ������ ���� Ÿ�� �������� ����
            DeletePreviousTiles();     // ������ ������ Ÿ�ϰ� ��ֹ����� ����


            Vector3 tilePlacementScale;   // Ÿ���� ũ��� ���⿡ ���� Ÿ���� ũ�⸦ ���
            if (prevTile.GetComponent<Tile>().type == TileType.SIDEWAYS)
            {
                tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size / 2 +(Vector3. one *   // ���� Ÿ���� SIDEWAYS Ÿ���� ��� Ÿ���� ũ�⸦ ���
                    startingTile.GetComponent<BoxCollider>().size.z / 2) , currentTileDirection);
            }
            else
            {
                tilePlacementScale = Vector3.Scale((prevTile.GetComponent<Renderer>().bounds.size -(Vector3.one *2))   // ���� Ÿ���� STRAIGHT Ÿ���� ��� Ÿ���� ũ�⸦ ���
                    + (Vector3.one*startingTile.GetComponent<BoxCollider>().size.z / 2), currentTileDirection);
            }

            currentTileLocation += tilePlacementScale;   // Ÿ���� ��ġ�� Ÿ���� ũ�⸸ŭ �̵�

            int currentPatgLength = Random.Range(minimumStraightTiles, maximumStraightTiles);   // ���� ���� ������ ������ ���̸� ���� Ÿ���� ����
            for (int i =0; i< currentPatgLength; i++)
            {
                SpawnTile(startingTile.GetComponent<Tile>(), (i == 0) ? false : true);
            }

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(), false);
            // ������ ȸ�� Ÿ���� ����
          
        }
        private void SpawnObstacle()
        {
            if (Random.value > 0.5f) return;

            GameObject obstaclePrefad = SelectRandomGameObjectFromList(obstacles);
            Quaternion newObjectRotation = obstaclePrefad.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);

            Vector3 obstaclePosition = currentTileLocation;
            bool isPositionValid = false;
            int maxAttempts = 10;
            int currentAttempt = 0;

            while (!isPositionValid && currentAttempt < maxAttempts)
            {
                currentAttempt++;
                obstaclePosition.z += Random.Range(-3f, 3f);
                obstaclePosition.x += Random.Range(-3f, 3f);
                isPositionValid = CheckObstaclePosition(obstaclePosition);
            }

            if (isPositionValid)
            {
                GameObject obstacle = Instantiate(obstaclePrefad, obstaclePosition, newObjectRotation);
                currentobstacles.Add(obstacle);
            }
        }

        private bool CheckObstaclePosition(Vector3 position)
        {
            foreach (GameObject obstacle in currentobstacles)
            {
                float distance = Vector3.Distance(obstacle.transform.position, position);
                if (distance < 2f) return false;
            }
            return true;
        }


        /*

        private void SpawnObstacle()
        {
            if (Random.value > 0.3f) return;

            GameObject obstaclePrefad = SelectRandomGameObjectFromList(obstacles);
            Quaternion newObjectRotation = obstaclePrefad.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);

            Vector3 obstaclePosition = currentTileLocation;
            obstaclePosition.z += Random.Range(-3f, 3f);
            obstaclePosition.x += Random.Range(-3f, 3f);

            GameObject obstacle = Instantiate(obstaclePrefad, obstaclePosition, newObjectRotation);
            currentobstacles.Add(obstacle);
        }
        */

        /*
        private void SpawnObstacle()  //���� �ڵ�
        {
            if (Random.value > 0.8f) return;   // ���� Ȯ���� ��ֹ��� ����

            GameObject obstaclePrefad = SelectRandomGameObjectFromList(obstacles);   // ��ֹ� ������ ����Ʈ �� ������ �������� ����
            Quaternion newObjectRotation = obstaclePrefad.gameObject.transform.rotation * Quaternion.LookRotation    // ��ֹ��� ������ ���� Ÿ�� �������� ����
                (currentTileDirection, Vector3.up);
            GameObject obstacle = Instantiate(obstaclePrefad, currentTileLocation, newObjectRotation);   // ������ ��ֹ��� ���� ��ġ�� ����
            currentobstacles.Add(obstacle);     // ������ ��ֹ��� ����Ʈ�� �߰�


        }
        */



        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;   // ����Ʈ�� �ƹ��� ��ҵ� ���ٸ� null ���� ��ȯ
            return list[Random.Range(0, list.Count)];   // ����Ʈ���� ������ �ε����� �����Ͽ� �ش� ��Ҹ� ��ȯ
        }
        
    

    }

}
