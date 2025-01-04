using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    private List<GameObject> _pickups;

    // Start is called before the first frame update
    void Start()
    {
        _pickups = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pickup"));
    }
    /// <summary>
    /// Attempt pickup for all pickups in the level
    /// </summary>
    public void AttemptPickups()
    {
        for(int i = 0; i < _pickups.Count; i++)
        {
            if(_pickups[i] == null)
            {
                _pickups.RemoveAt(i);
                i--;
            }
            else
            {
                _pickups[i].GetComponent<PickupController>().AttemptPickup();
            }
        }
    }
}
