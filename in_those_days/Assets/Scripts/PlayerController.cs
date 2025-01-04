using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode InteractKey;
    public int AttackOneButton;
    public int AttackTwoButton;

    public DirectionEvent Move;
    public UnityEvent Jump;
    public UnityEvent InteractDown;
    public UnityEvent InteractUp;
    public UnityEvent AttackOne;
    public UnityEvent AttackTwo;

    // Update is called once per frame
    void Update()
    {
        // Check for keyboard input
        if (Input.GetKey(UpKey))
        {
            Move.Invoke(FaceDirection.Up);
        }
        else if (Input.GetKey(DownKey))
        {
            Move.Invoke(FaceDirection.Down);
        }
        else if (Input.GetKey(LeftKey))
        {
            Move.Invoke(FaceDirection.Left);
        }
        else if (Input.GetKey(RightKey))
        {
            Move.Invoke(FaceDirection.Right);
        }
        else
        {
            Move.Invoke(FaceDirection.Stop);
        }
        if (Input.GetKeyDown(InteractKey))
        {
            InteractDown.Invoke();
        }
        if (Input.GetKeyUp(InteractKey))
        {
            InteractUp.Invoke();
        }
        if (Input.GetKeyDown(UpKey))
        {
            Jump.Invoke();
        }

        // Check for mouse input
        if (Input.GetMouseButtonDown(AttackOneButton))
        {
            AttackOne.Invoke();
        }
        if (Input.GetMouseButtonDown(AttackTwoButton))
        {
            AttackTwo.Invoke();
        }
    }
}
