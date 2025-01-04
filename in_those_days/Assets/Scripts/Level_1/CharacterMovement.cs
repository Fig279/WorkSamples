using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float MoveSpeed = 3;
    public FaceDirection Direction;
    public bool Frozen;

    private Animator _animator;
    private Vector2 _lastVelocity;
    
    public float StunTime;
    public float _stunDistanceLeft;
    public Vector2 _lastPosition;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {
            _animator.SetFloat("XInput", GameDictionary.moveDirections[Direction].x);
            _animator.SetFloat("YInput", GameDictionary.moveDirections[Direction].y);
        }
    }
            
    public void Update()
    {
        if (StunTime > 0)
        {
            StunTime -= Time.deltaTime;

            // Flash Color
            Color color = GetComponent<SpriteRenderer>().color;
            if (color.CompareRGB(new Color(1, 1, 1)))
            {
                if (StunTime % 0.4f > 0.2f)
                {

                    color.a = 0.5f;
                }
                else
                { 
                    color.a = 1f;
                    
                }
                GetComponent<SpriteRenderer>().color = color;
            }
            
            if (_stunDistanceLeft > 0)
            {
                _stunDistanceLeft -= (_lastPosition - (Vector2)transform.position).magnitude;
                _lastPosition = transform.position;
            }
            else if (_stunDistanceLeft <= 0 && gameObject.tag != "Player")
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
        else if(StunTime < 0)
        {
            StunTime = 0;
            _stunDistanceLeft = 0;

            // Normal Color
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1f;
            GetComponent<SpriteRenderer>().color = color;
        }
        if (_animator != null)
        {
            _animator.SetFloat("Speed", GetComponent<Rigidbody2D>().velocity.magnitude);
        }
    }

    public void Move(FaceDirection direction)
    {
        if (!Frozen && (gameObject.tag == "Player" || _stunDistanceLeft <= 0) )
        {
            if (direction != FaceDirection.Stop)
            {
                Direction = direction;
                GetComponent<SpriteRenderer>().flipX = direction == FaceDirection.Right;
                
                if (_animator != null)
                {
                    _animator.SetFloat("XInput", GameDictionary.moveDirections[direction].x);
                    _animator.SetFloat("YInput", GameDictionary.moveDirections[direction].y);
                    _animator.SetTrigger("TrWalk");
                }
            }
            GetComponent<Rigidbody2D>().velocity = GameDictionary.moveDirections[direction] * MoveSpeed;
            if (_animator != null)
            {
                _animator.SetFloat("Speed", GetComponent<Rigidbody2D>().velocity.magnitude);
            }
        }
    }
    /// <summary>
    /// Freezes movement
    /// </summary>
    /// <param name="frozen"></param>
    public void Freeze(bool frozen)
    {
        if(Frozen != frozen)
        {
            Frozen = frozen;
            if (Frozen)
            {
                if(GetComponent<Rigidbody2D>().velocity != Vector2.zero)
                {
                    _lastVelocity = GetComponent<Rigidbody2D>().velocity;
                }
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = _lastVelocity;
            }
            //GetComponent<Rigidbody2D>().isKinematic = Frozen;
        }
    }
    public void RecieveKnockback(Vector2 velocity, float distance, float time)
    {
        Debug.Log("Knockback recieved: " + velocity);
        GetComponent<Rigidbody2D>().velocity = velocity;
        StunTime = time;
        _stunDistanceLeft = distance;
        _lastPosition = transform.position;
    }
}

