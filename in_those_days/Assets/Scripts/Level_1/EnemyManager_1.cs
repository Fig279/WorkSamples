using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_1 : MonoBehaviour
{
    public CameraManager cameraManager;
    public List<GameObject> enemies;

    public void Start()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    public void FreezeAll()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
            }
            else
            {
                Debug.Log("Freezing Enemies:" + (i+1) + "/" + enemies.Count);
                enemies[i].GetComponent<EnemyController>().Freeze(true);
            }
        }
    }
    public void ActiveCheckAll()
    {
        
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
            }
            else
            {
                bool inArea = GameDictionary.InGameArea((Vector2)enemies[i].transform.position,i==0,cameraManager);
                Debug.Log("Checking Enemies:" + (i+1) + "/" + enemies.Count + "; In area = " + inArea);
                enemies[i].GetComponent<EnemyController>().Freeze(!inArea);
            }
        }
    }
}
