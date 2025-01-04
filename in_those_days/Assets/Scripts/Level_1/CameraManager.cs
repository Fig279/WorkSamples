using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    public GameObject Player;
    public Vector2 LevelSize = new Vector2(16, 9);
    public Vector2 UIOffset = new Vector2(0,-0.5f);
    public float CameraSpeed = 30;

    public UnityEvent CameraStartMove;
    public UnityEvent CameraStopMove;

    
    public bool Moving = false;
    
    public Vector2 CurrentArea = new Vector2(0, 0);
    private Vector2 TargetPosition = new Vector2(0,0);

    public void Start()
    {
        Vector3 startPosition = (Vector3)((CurrentArea * LevelSize) - UIOffset);
        startPosition.z = transform.position.z;
        transform.position = startPosition;
        TargetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player outside of level
        if (Player.transform.position.x > LevelSize.x * (CurrentArea.x + 0.5))
        {
            CurrentArea.x++;
            TargetPosition = (CurrentArea * LevelSize) - UIOffset/2;
        }
        else if (Player.transform.position.y > LevelSize.y * (CurrentArea.y + 0.5))
        {
            CurrentArea.y++;
            TargetPosition = (CurrentArea * LevelSize) - UIOffset/2;
        }
        else if (Player.transform.position.x < LevelSize.x * (CurrentArea.x - 0.5))
        {
            CurrentArea.x--;
            TargetPosition = (CurrentArea * LevelSize) - UIOffset/2;
        }
        else if (Player.transform.position.y < LevelSize.y * (CurrentArea.y - 0.5))
        {
            CurrentArea.y--;
            TargetPosition = (CurrentArea * LevelSize) - UIOffset/2;
        }

        // Move camera if necessary
        Vector2 moveVector = TargetPosition - (Vector2)transform.position;
        if ((moveVector).magnitude > CameraSpeed * Time.deltaTime)
        {
            if(Moving == false)
            {
                CameraStartMove.Invoke();
            }
            Moving = true;
            transform.position += (Vector3)moveVector.normalized * CameraSpeed * Time.deltaTime;
        }
        else
        {
            if(Moving == true)
            {
                CameraStopMove.Invoke();
            }
            Moving = false;
            transform.position += (Vector3)moveVector;
        }
    }
}
