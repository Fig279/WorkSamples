using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    private SpriteRenderer timerSprite;
    private float timer = 0;
    private float timeOfDay = 0;

    [SerializeField]
    private List<Sprite> sprites;

    private void Awake()
    {
        timerSprite = this.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;   

        //an hour is every 20 seconds
        if(timer >= 15)
        {
            timeOfDay++;
            timer = 0;
        }


        //changes the sprite of clock acording to time passed
        switch (timeOfDay){
            case 1:
                timerSprite.sprite = sprites[0];

                break;
            case 2:

                timerSprite.sprite = sprites[1];

                break;
            case 3:

                timerSprite.sprite = sprites[2];

                break;
            case 4:

                timerSprite.sprite = sprites[3];

                break;
            case 5:
                timerSprite.sprite = sprites[4];


                break;
            case 6:
                timerSprite.sprite = sprites[5];

                //you win
                SceneManager.LoadScene("cutscene3");

                break;
        }

    }
}
