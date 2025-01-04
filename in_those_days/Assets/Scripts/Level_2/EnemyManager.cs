using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    //represents the state the mommy is in
    private enum MommyStates
    {
        gone,
        present,
        attacking,
    }

    [SerializeField]
    private List<GameObject> mommyLocations;
    private SpriteRenderer activeMommySprite;
    private MommyStates mommyStates;
    private int mommyLocation;
    private float attackNumber = 0;

    [SerializeField]
    private PlayerHealth player;

    private float timer = 0;

    private List<Animator> animators = new List<Animator>(3);



    //amount of time mommy must wait before attacking again
    private float timeInBetweenAttacks = 5;

    private void Awake()
    {
        animators.Add(transform.GetChild(0).GetComponent<Animator>());
        animators.Add(transform.GetChild(1).GetComponent<Animator>());
        animators.Add(transform.GetChild(2).GetComponent<Animator>());

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //will check if Mommy was in the previous state for long enough to move on to the next state
        //mommy has arrived
        if (mommyStates == MommyStates.gone && timer > timeInBetweenAttacks)
        {
            //selects the window, closet or door Mommy randomly
            mommyLocation = Random.Range(0, 3);
            activeMommySprite = mommyLocations[mommyLocation].GetComponent<SpriteRenderer>();

            //activate appropriate animation
            animators[mommyLocation].SetInteger("MommyLocation", mommyLocation);

            //Mommy is in location sprite
            mommyStates = MommyStates.present;
        }
        //after looking around for 3 seconds she checks closer for the kill
        //she goes in for the kid
        else if(mommyStates == MommyStates.present && timer > timeInBetweenAttacks + 2)
        {
            //Mommy sprite is attacking
            mommyStates = MommyStates.attacking;

            //tells animator to play Attacm animation
            animators[mommyLocation].SetInteger("MommyLocation", 5);

            //the child isn't hiding during attack so mommy catches them
            if (!player.IsHiding)
            {
                player.Caught();
            }
        }
        //after 0.7 seconds of attack mommy is unsuccessful and leaves
        else if (timer > timeInBetweenAttacks + 2.7)
        {
            //Mommy goes back to normal
            mommyStates = MommyStates.gone;
            attackNumber++;

            timer = 0;

            animators[mommyLocation].SetInteger("MommyLocation", 4);

            //the first 3 attack are all 5 seconds inbetween
            //then Mommy gets generally faster after every 3 attacks
            if (attackNumber > 2 && attackNumber < 5)
            {
                timeInBetweenAttacks = Random.Range(4, 7);
            }
            else if(attackNumber < 8)
            {
                timeInBetweenAttacks = Random.Range(3, 7);
            }
            else if (attackNumber < 11)
            {
                timeInBetweenAttacks = Random.Range(2, 6);
            }
            else if (attackNumber < 14)
            {
                timeInBetweenAttacks = Random.Range(1, 6);
            }

        }

    }



}
