using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public float StartTime = 1f;
    public float CurrentTime;
    public Vector2 TotalOffset = new Vector2(0,1);
    public float Alpha;

    private Vector2 _startPosition;
    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = StartTime;
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
            Color color = GetComponent<SpriteRenderer>().color;
            color.a =  Mathf.Pow(CurrentTime, 2) / Mathf.Pow(StartTime,2);
            Alpha = color.a;
            GetComponent<SpriteRenderer>().color = color;
            transform.position = _startPosition + TotalOffset * (1-color.a);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
