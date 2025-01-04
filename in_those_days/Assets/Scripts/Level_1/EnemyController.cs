using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public EnemyType Type;
    public GameObject Target;

    public UnityEvent Attack;
    public DirectionEvent Move;
    
    private CharacterMovement moveScript;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveScript.StunTime == 0 && !GetComponent<CharacterMovement>().Frozen)
        {
            FaceDirection direction = FaceDirection.Stop;
            float xDistance = Mathf.Abs(Target.transform.position.x - transform.position.x);
            float yDistance = Mathf.Abs(Target.transform.position.y - transform.position.y);
            CharacterAttack attackScript = GetComponent<CharacterAttack>();

            switch (Type)
            {
                case EnemyType.Melee:
                    
                    if ((xDistance < yDistance || yDistance < 0.4f) && xDistance > 0.4f)
                    {
                        // Move X
                        if(Target.transform.position.x > transform.position.x)
                        {
                            direction = FaceDirection.Right;
                        }
                        else
                        {
                            direction = FaceDirection.Left;
                        }
                    }
                    else if (yDistance > 0.4f)
                    {
                        // Move Y
                        if (Target.transform.position.y > transform.position.y)
                        {
                            direction = FaceDirection.Up;
                        }
                        else
                        {
                            direction = FaceDirection.Down;
                        }
                    }
                    else
                    {
                        direction = FaceDirection.Stop;
                    }
                    moveScript.Move(direction);
                    break;
                case EnemyType.Ranged:
                    // Shoot Segment
                    if((xDistance < 1.5 || yDistance < 1.5) && attackScript.AttackCooldown == 0)
                    {
                        // Choose Direction
                        if(xDistance > yDistance)
                        {
                            if(Target.transform.position.x > transform.position.x)
                        {
                                direction = FaceDirection.Right;
                            }
                            else
                            {
                                direction = FaceDirection.Left;
                            }
                        }
                        else
                        {
                            if (Target.transform.position.y > transform.position.y)
                            {
                                direction = FaceDirection.Up;
                            }
                            else
                            {
                                direction = FaceDirection.Down;
                            }
                        }
                        // Shoot
                        GetComponent<CharacterMovement>().Direction = direction;
                        attackScript.ProjectileAttack();
                        
                    }
                    // Move Segment
                    else
                    {
                        if ((xDistance < yDistance) && xDistance > 0.4f)
                        {
                            // Move X
                            if (Target.transform.position.x > transform.position.x)
                            {
                                direction = FaceDirection.Right;
                            }
                            else
                            {
                                direction = FaceDirection.Left;
                            }
                        }
                        else if (yDistance > 0.4f)
                        {
                            // Move Y
                            if (Target.transform.position.y > transform.position.y)
                            {
                                direction = FaceDirection.Up;
                            }
                            else
                            {
                                direction = FaceDirection.Down;
                            }
                        }
                        else
                        {
                            direction = FaceDirection.Stop;
                        }
                        moveScript.Move(direction);
                    }
                    
                    break;
                case EnemyType.Boss:
                    break;
            }
        }
    }
    /// <summary>
    /// Freeze movement and projectiles, stops AI
    /// </summary>
    /// <param name="frozen"></param>
    public void Freeze(bool frozen)
    {
        GetComponent<CharacterMovement>().Freeze(frozen);
        GetComponent<CharacterAttack>().FreezeProjectiles(frozen);
    }
}

public enum EnemyType
{
    Melee,
    Ranged,
    Boss
}
