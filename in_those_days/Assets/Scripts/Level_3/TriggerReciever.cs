using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerReciever : MonoBehaviour
{
    [SerializeField]
    private GameObject puddleCover;

    public GameObject buttonDecor;

    [SerializeField]
    private float yPosition;

    [SerializeField]
    private GameObject correspondingPuddle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
         puddleCover.transform.position = new Vector3
            (puddleCover.transform.position.x, yPosition);

        Destroy(correspondingPuddle);
        Destroy(buttonDecor);

    }

}
