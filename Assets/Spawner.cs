using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Enemy prefab;
    public float delay = 15;

    float timer;

    public float delayDecreaseSpeed = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            timer = 0;
            Instantiate(prefab, transform.position, Quaternion.identity);
            delay -= delayDecreaseSpeed;
        }
    }
}
