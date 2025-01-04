using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ElementType
{
    fire,
    water
}

public class PuddleController : MonoBehaviour
{
    //variable to determine what kind of puddle
    //collision(trigger) to restart the game.
    //  collisions is based on whoever is able to walk in it 

    //fields
    //variables to determine the kind of puddles there are.

    [SerializeField]
    private ElementType puddleType;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RemovePuddle()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //determine if the collision is with the right player.
        if(other.gameObject.tag == "Player" && puddleType == ElementType.water)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Level_3");
        }
        if(other.gameObject.tag == "Girlfriend" && puddleType == ElementType.fire)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Level_3");
        }

    }
}
