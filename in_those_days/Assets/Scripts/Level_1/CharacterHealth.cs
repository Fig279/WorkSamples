using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour
{
    public float Health;
    public UnityEvent Defeated;

    private float _redTimer;
    public float P_Health
    {
        get
        {
            return Health;
        }
        set
        {
            Health = value;
            if(Health <= 0)
            {

                if (gameObject.tag == "Player")
                {
                    SceneManager.LoadScene("Level_1");
                }
                else
                {
                    Defeated.Invoke();
                    GetComponent<CharacterAttack>().DestroyProjectiles();
                    Destroy(gameObject);
                }
            }
            if(Health > 3)
            {
                Health = 3;
            }
        }
    }

    public void Update()
    {
        if(_redTimer > 0)
        {
            _redTimer -= Time.deltaTime;
        }
        else if(_redTimer < 0)
        {
            _redTimer = 0;
            Color color = GetComponent<SpriteRenderer>().color;
            color = new Color(1, 1, 1);
            GetComponent<SpriteRenderer>().color = color;
        }
    }
    /// <summary>
    /// Decreases health by a specified amount
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(float amount)
    {
        P_Health -= amount;
        if(gameObject.tag == "Player")
        {
            GetComponent<CharacterMovement>().StunTime = 1.5f;
        }
        
        _redTimer = 0.3f;
        Color color = GetComponent<SpriteRenderer>().color;
        color = new Color(1, 0.7f, 0.7f);
        GetComponent<SpriteRenderer>().color = color;
        
    }
    public void Heal(float amount)
    {
        P_Health += amount;
    }
}
