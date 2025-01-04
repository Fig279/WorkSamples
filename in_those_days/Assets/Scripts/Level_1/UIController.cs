using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public SpriteRenderer Healthbar;
    public SpriteRenderer[] StickIcon;
    public SpriteRenderer[] RockIcon;
    public GameObject Player;

    public Sprite[] HealthBars;

    private bool P_StickIcon
    {
        set
        {
            for(int i = 0; i < StickIcon.Length; i++)
            {
                StickIcon[i].enabled = value;
            }
        }

    }
    private bool P_RockIcon
    {
        set
        {
            for (int i = 0; i < StickIcon.Length; i++)
            {
                RockIcon[i].enabled = value;
            }
        }

    }

    public float Health
    {
        set
        {
            _health = (int)Mathf.Min(3, value);
            if(_health < 0)
            {
                _health = 0;
            }
        }
    }
    private int _health;

    public void Start()
    {
        P_StickIcon = false;
        P_RockIcon = false;
    }


    // Update is called once per frame
    void Update()
    {
        Health = Player.GetComponent<CharacterHealth>().Health;

        Healthbar.sprite = HealthBars[_health];
    }
    /// <summary>
    /// Unlocks a specific weapon
    /// </summary>
    /// <param name="identifier"></param>
    public void UnlockWeapon(int identifier)
    {
        switch (identifier)
        {
            case 1:
                P_StickIcon = true;
                break;
            case 2:
                P_RockIcon = true;
                break;
            default:
                Debug.Log("Not a valid weapon");
                break;
        }
    }
}
