using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    private bool playerDoorEntered = false;

    [SerializeField]
    private GirlfriendDoorController otherDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDoorEntered && otherDoor.GirlfriendDoorEntered)
        {
            SceneManager.LoadScene("cutscene_Final");
        }
    }

    /// <summary>
    /// when a door is entered check if the correct player entered the correct door
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerDoorEntered = true;
        }

    }

    /// <summary>
    /// when a player leaves a door make sure that player's doorEntered value is false
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerDoorEntered = false;
        }

    }

}
