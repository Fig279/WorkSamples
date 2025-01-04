using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 initialPosition = new Vector3(48.06f, -3.59f, 0f);

    //information for jumping
    Vector3 leapDirection = new Vector3(0, 0, 0);
    float jumpForce = 350f;

    float speed = 4f;

    public bool hasJump;

    //Animation
    private Animator animator;

    private Rigidbody2D rBody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
        hasJump  = true;
    }

    // Update is called once per frame
    void Update()
    {
        SideMovements();
    }

    /// <summary>
    /// Move left and right if a specific key is pressed.
    /// </summary>
    public void SideMovements()
    {
        //position and direction
        Vector3 playerDirection = Vector3.zero;

        //Right movement
        if (Input.GetKey(KeyCode.D))
        {
            //change direction
            animator.SetBool("WalkRight", true);

            //player moves right
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 2 * Time.deltaTime
                , gameObject.transform.position.y, -1);
        }
        else
        {
            animator.SetBool("WalkRight", false);
        }
        //left movement
        if (Input.GetKey(KeyCode.A))
        {
            //change direction
            animator.SetBool("WalkLeft", true);

            //player moves right
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 2 * Time.deltaTime
                , gameObject.transform.position.y,-1);
        }
        else
        {
            animator.SetBool("WalkLeft", false);

        }


        if (Input.GetKey(KeyCode.W) && hasJump)
        {
            Vector2 jumpVect = Vector2.up * jumpForce;
            rBody.AddForce(jumpVect);
            hasJump = false;
        }
        //velocity
        Vector3 velocity = playerDirection * speed;
        velocity *= Time.deltaTime;

    }


    /// <summary>
    /// move the character in a upwards direction
    /// </summary>
    public void Jump()
    {
        if (hasJump)
        {
            leapDirection.y += 2;

            Vector3 jumpVelocity = leapDirection.normalized * jumpForce;

            gameObject.GetComponent<Rigidbody2D>().AddForce(jumpVelocity);

            leapDirection.y = 0;
            hasJump = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && collision.gameObject.transform.position.y < gameObject.transform.position.y)
        {
            hasJump = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fire")
        {
            gameObject.transform.position = initialPosition;
        }
    }
}
