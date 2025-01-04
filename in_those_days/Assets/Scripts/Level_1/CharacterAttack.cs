using UnityEngine;
using System.Collections.Generic;

public class CharacterAttack : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Sprite ProjectileSprite;
    public Sprite MeleeSprite;
    
    public float ProjectileDamage = 1;
    public float MeleeDamage = 2;
    public float ProjectileSpeed = 10;
    public float MeleeSpeed = 6;
    public float Cooldown = 1f;
    public float StopTime = 0.3f;
    public float ProjectileDistance = 8;
    public float MeleeDistance = 2;
    public bool ProjectileUnlocked = true;
    public bool MeleeUnlocked = true;

    private List<GameObject> _projectiles = new List<GameObject>(5);
    [HideInInspector]
    public float AttackCooldown;
    private float _moveCooldown;
    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (_animator != null)
        {
            _animator.SetBool("Attacking", _moveCooldown > 0);
        }
        if (_moveCooldown > 0)
        {
            _moveCooldown -= Time.deltaTime;
        }
        if (_moveCooldown < 0)
        {
            GetComponent<CharacterMovement>().Freeze(false);
            _moveCooldown = 0;
        }
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        if(AttackCooldown < 0)
        {
            AttackCooldown = 0;
        }
    }

    /// <summary>
    /// Create a projectile object that moves the direction the player is facing
    /// </summary>
    public void ProjectileAttack()
    {
        if(AttackCooldown == 0 && ProjectileUnlocked)
        {
            // Spawn projectile
            GameObject projectileObject = Instantiate(
                ProjectilePrefab, 
                transform.position + (Vector3)GameDictionary.moveDirections[gameObject.GetComponent<CharacterMovement>().Direction]*0.2f, 
                new Quaternion());
            _projectiles.Add(projectileObject);
            projectileObject.GetComponent<SpriteRenderer>().sprite = ProjectileSprite;
            projectileObject.GetComponent<ProjectileController>().Initiate(
                ProjectileSpeed,
                gameObject.GetComponent<CharacterMovement>().Direction,
                ProjectileDamage,
                ProjectileDistance,
                gameObject.tag == "Player");

            // Set Cooldown
            AttackCooldown = Cooldown;
            _moveCooldown = StopTime;
            GetComponent<CharacterMovement>().Freeze(true);

            if (_animator != null)
            {
                _animator.SetTrigger("TrAttack");
                _animator.SetBool("Attacking", true);
            }
        }
    }
    /// <summary>
    /// Create a melee object that moves the direction the player is facing
    /// </summary>
    public void MeleeAttack()
    {
        if (AttackCooldown == 0 && MeleeUnlocked)
        {
            // Spawn projectile
            Vector2 direction = GameDictionary.moveDirections[gameObject.GetComponent<CharacterMovement>().Direction];
            GameObject meleeObject = Instantiate(
                ProjectilePrefab,
                transform.position + (Vector3)direction * 0.1f,
                Quaternion.AngleAxis(90 + 180 / 3.1416f * Mathf.Atan2(direction.y,direction.x),Vector3.forward));
            _projectiles.Add(meleeObject);
            meleeObject.GetComponent<SpriteRenderer>().sprite = MeleeSprite;
            ProjectileController pController = meleeObject.GetComponent<ProjectileController>();
            pController.Knockback = 1.5f;
            pController.Initiate(
                MeleeSpeed,
                gameObject.GetComponent<CharacterMovement>().Direction,
                MeleeDamage,
                MeleeDistance,
                gameObject.tag == "Player");

            pController.FadeAway = gameObject.tag == "Player";
            
            // Set Cooldown
            AttackCooldown = Cooldown;
            _moveCooldown = StopTime;
            GetComponent<CharacterMovement>().Freeze(true);


            if (_animator != null)
            {
                _animator.SetTrigger("TrAttack");
                _animator.SetBool("Attacking", true);
            }
        }
    }
    /// <summary>
    /// Freezes all projectiles
    /// </summary>
    /// <param name="frozen"></param>
    public void FreezeProjectiles(bool frozen)
    {
        for(int i = 0; i < _projectiles.Count; i++)
        {
            if (_projectiles[i] == null)
            {
                _projectiles.RemoveAt(i);
                i--;
            }
            else
            {
                _projectiles[i].GetComponent<CharacterMovement>().Freeze(frozen);
            }
        }
    }
    public void DestroyProjectiles()
    {
        for(int i = 0; i<_projectiles.Count; i++)
        {
            Destroy(_projectiles[i]);
        }
    }
    public void UnlockWeapon(int identifier)
    {
        switch (identifier)
        {
            case 1:
                MeleeUnlocked = true;
                break;
            case 2:
                ProjectileUnlocked = true;
                break;
            default:
                Debug.Log("Not a valid weapon");
                break;
        }
    }
}
