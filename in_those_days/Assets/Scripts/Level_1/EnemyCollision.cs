using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollision : MonoBehaviour
{
    public float PhysicalDamage = 1;

    /// <summary>
    /// Destroys object and damages character when collided 
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (!GetComponent<CharacterMovement>().Frozen && 
            collision.gameObject.tag == "Player" && 
            GetComponent<CharacterMovement>().StunTime <= 0 &&
            collision.gameObject.GetComponent<CharacterMovement>().StunTime <= 0)
        {
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(PhysicalDamage);
        }
    }
}
