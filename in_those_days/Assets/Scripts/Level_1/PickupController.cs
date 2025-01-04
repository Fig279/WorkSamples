using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PickupController : MonoBehaviour
{
    public int ItemID;
    public float PickupRange;
    public GameObject Player;
    public GameObject Particle;
    public SpriteRenderer ControlDisplay;
    public bool InRange;
    public IntEvent ItemPickup;
    public UnityEvent Collected;
    

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPosition = Player.transform.position;
        Vector2 itemPosition = transform.position;
        InRange = (playerPosition - itemPosition).magnitude <= PickupRange;
        if (InRange)
        {

            GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f);
        }
        ControlDisplay.enabled = InRange;
    }
    public void AttemptPickup()
    {
        if (InRange)
        {
            ItemPickup.Invoke(ItemID);
            Collected.Invoke();
            Destroy(gameObject);
        }
    }
    public void ReleaseParticle()
    {
        Instantiate(Particle, transform.position, new Quaternion());
    }
}