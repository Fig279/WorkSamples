using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileController : MonoBehaviour
{
    [HideInInspector]
    public float Damage;
    [HideInInspector]
    public bool PlayerFriendly;
    [HideInInspector]
    public float MaxDistance;
    public bool FadeAway = false;
    public float Knockback = 0;

    private float _distanceTraveled;
    private Vector2 _lastPosition;

    private void Update()
    {
        Vector2 currentPosition = transform.position;
        _distanceTraveled += (currentPosition - _lastPosition).magnitude;

        if (_distanceTraveled > MaxDistance) // NOTE: this does not extrapolate, so it can technically go beyond the max distance
        {
            Destroy(gameObject);
        }
        else if (FadeAway)
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1-Mathf.Pow(_distanceTraveled / MaxDistance,2);
            GetComponent<SpriteRenderer>().color = color;
        }

        _lastPosition = transform.position;
    }

    /// <summary>
    /// Set initial velocity and variables
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    /// <param name="damage"></param>
    /// <param name="playerFriendly"></param>
    public void Initiate(float speed, FaceDirection direction, float damage, float distance, bool playerFriendly)
    {
        CharacterMovement moveScript = gameObject.GetComponent<CharacterMovement>();
        moveScript.MoveSpeed = speed;
        moveScript.Move(direction);
        moveScript.Direction = direction;

        if (direction == FaceDirection.Left)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        Damage = damage;
        PlayerFriendly = playerFriendly;
        MaxDistance = distance;

        _lastPosition = transform.position;

    }
    /// <summary>
    /// Destroys object and damages character when collided 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && PlayerFriendly)
        {
            Debug.Log("Hit enemy, damage: " + Damage);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(Damage);
            if(Knockback != 0)
            {
                collision.gameObject.GetComponent<CharacterMovement>().RecieveKnockback(
                    GameDictionary.moveDirections[gameObject.GetComponent<CharacterMovement>().Direction]*5, 
                    Knockback, 
                    1f);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player" && !PlayerFriendly && collision.gameObject.GetComponent<CharacterMovement>().StunTime == 0)
        {
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(Damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

}
