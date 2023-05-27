
using System.Collections.Generic;
using UnityEngine;


namespace TempleRun
{

    public class TileSpawer : MonoBehaviour
    {
        [SerializeField]
        private int tileStartCount = 10;  // 처음 생성할 Tile의 갯수를 정하는 변수
        [SerializeField]
        private int minimumStraightTiles = 3;   // 일직선 Tile 생성의 최소 갯수를 정하는 변수
        [SerializeField]
        private int maximumStraightTiles = 15;   // 일직선 Tile 생성의 최대 갯수를 정하는 변수
        [SerializeField]
        private GameObject startingTile;    // 게임 시작할 때 생성될 Tile
        [SerializeField]
        private List<GameObject> turnTiles;   // 회전 Tile의 종류를 담고 있는 GameObject List
        [SerializeField]
        private List<GameObject> obstacles;  // 장애물의 종류를 담고 있는 GameObject List

        [SerializeField]
        private List<GameObject> items;  // 아이템의 종류를 담고 있는 GameObject List

        private Vector3 currentTileLocation = Vector3.zero;  // 현재 Tile의 위치를 담고 있는 변수
        private Vector3 currentTileDirection = Vector3.forward; //현재 Tile의 방향을 담고 있는 변수
        private GameObject prevTile;

        private List<GameObject> currentTiles;  // 현재 생성된 Tile들을 담고 있는 GameObject List
        private List<GameObject> currentobstacles;  // 현재 생성된 장애물들을 담고 있는 GameObject List


        float zOffset = 10f;  // 새로운 타일 생성 위치의 z 값


        private void Start()
        {

 
            currentTiles = new List<GameObject>();  // 현재 생성된 Tile들을 담을 List를 초기화
            currentobstacles = new List<GameObject>();  // 현재 생성된 장애물들을 담을 List를 초기화
  

            Random.InitState(System.DateTime.Now.Millisecond);  // 시간을 기준으로 랜덤한 값을 생성하기 위해 초기화

            for (int i =0; i< tileStartCount; ++i)
            {
                SpawnTile(startingTile.GetComponent<Tile>()); // 시작 Tile을 생성
            }    

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>());  // 회전 Tile을 생성


        }

    


        private void SpawnTile(Tile tile, bool spawnObstacle = false )  // Tile을 생성하는 함수
        {
            Quaternion newTileRotation = tile.gameObject.transform.rotation* Quaternion.LookRotation(currentTileDirection, Vector3.up); // 새로운 Tile의 회전 값을 계산
            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation); // Tile을 생성
            currentTiles.Add(prevTile);  // 생성된 Tile을 List에 추가

           

            if (spawnObstacle) SpawnObstacle();  // 장애물을 생성

  

            if (tile.type == TileType.STRAIGHT)   // Tile이 일직선 Tile일 경우
                currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection);  // 현재 위치를 일직선 방향으로 이동

        }

        private void DeletePreviousTiles()  // 이전에 생성된 Tile을 삭제하는 함수
        {
            while (currentTiles.Count != 1)  // currentTiles 리스트에 타일이 1개 남을 때까지 삭제
            {
                GameObject tile = currentTiles[0];   // 리스트의 첫 번째 타일을 가져와서 제거
                currentTiles.RemoveAt(0);
                Destroy(tile);             // 가져온 타일 객체를 파괴
            }

            while (currentobstacles.Count != 0)   // currentobstacles 리스트에 장애물이 존재하는 동안 반복
            {
                GameObject obstacle = currentobstacles[0];   // 리스트의 첫 번째 장애물을 가져와서 제거
                currentobstacles.RemoveAt(0);
                Destroy(obstacle);     // 가져온 장애물 객체를 파괴
            }

        }

        public void AddNewDirection(Vector3 direction)   // AddNewDirection 함수는 새로운 방향을 받아와서 타일과 장애물을 생성하는 함수
        { 
            currentTileDirection = direction;  // 새로운 방향을 현재 타일 방향으로 설정
            DeletePreviousTiles();     // 이전에 생성된 타일과 장애물들을 삭제


            Vector3 tilePlacementScale;   // 타일의 크기와 방향에 따라 타일의 크기를 계산
            if (prevTile.GetComponent<Tile>().type == TileType.SIDEWAYS)
            {
                tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size / 2 +(Vector3. one *   // 이전 타일이 SIDEWAYS 타입일 경우 타일의 크기를 계산
                    startingTile.GetComponent<BoxCollider>().size.z / 2) , currentTileDirection);
            }
            else
            {
                tilePlacementScale = Vector3.Scale((prevTile.GetComponent<Renderer>().bounds.size -(Vector3.one *2))   // 이전 타일이 STRAIGHT 타입일 경우 타일의 크기를 계산
                    + (Vector3.one*startingTile.GetComponent<BoxCollider>().size.z / 2), currentTileDirection);
            }

            currentTileLocation += tilePlacementScale;   // 타일의 위치를 타일의 크기만큼 이동

            int currentPatgLength = Random.Range(minimumStraightTiles, maximumStraightTiles);   // 일정 범위 내에서 랜덤한 길이를 갖는 타일을 생성
            for (int i =0; i< currentPatgLength; i++)
            {
                SpawnTile(startingTile.GetComponent<Tile>(), (i == 0) ? false : true);
            }

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(), false);
            // 랜덤한 회전 타일을 생성
          
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
        private void SpawnObstacle()  //원래 코드
        {
            if (Random.value > 0.8f) return;   // 일정 확률로 장애물을 생성

            GameObject obstaclePrefad = SelectRandomGameObjectFromList(obstacles);   // 장애물 프리팹 리스트 중 랜덤한 프리팹을 선택
            Quaternion newObjectRotation = obstaclePrefad.gameObject.transform.rotation * Quaternion.LookRotation    // 장애물의 방향을 현재 타일 방향으로 설정
                (currentTileDirection, Vector3.up);
            GameObject obstacle = Instantiate(obstaclePrefad, currentTileLocation, newObjectRotation);   // 선택한 장애물을 현재 위치에 생성
            currentobstacles.Add(obstacle);     // 생성한 장애물을 리스트에 추가


        }
        */



        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;   // 리스트에 아무런 요소도 없다면 null 값을 반환
            return list[Random.Range(0, list.Count)];   // 리스트에서 랜덤한 인덱스를 선택하여 해당 요소를 반환
        }
        
    

    }

}
