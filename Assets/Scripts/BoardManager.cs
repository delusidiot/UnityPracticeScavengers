using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;//부모역할의
    private List<Vector3> gridPositions = new List<Vector3>();//Random으로 같은자리에 2개의 Item을 놓지 않게 하는것이다. (중복 방지) 배열의 인덱스에서 랜덤으로 하나를 뽑는다. 하나씩 지우는데 이게 리스트여서 빈자리를 채움.

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; x < rows + 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
                GameObject toInstantiage = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiage = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiage, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }
    void InitialiseList()
    {//매 레벨이 시작될 때마다 실행한다.
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }
    Vector3 RandomPosition()
    {//Randoom 으로 배치하고 두개가 겹치지 못하도록 gridPosition에서 Remove한다.
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 RandomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return RandomPosition;
    }
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{//벽, 적, 음식 들의 배치 함수는 모두 같은(비슷한)동작으로 배치된다. 그러므로 한 함수로 모두를 컨트롤 할 수 있으면 좋다.
		int objectCount = Random.Range(minimum, maximum + 1);
		for(int i = 0; i<objectCount; i++){
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}
	public void SetupScenes(int level)//적때문에
	{
		BoardSetup();
		InitialiseList();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log(level,2f);
		LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
		Instantiate(exit, new Vector3(columns-1,rows-1), Quaternion.identity);
	}
}
