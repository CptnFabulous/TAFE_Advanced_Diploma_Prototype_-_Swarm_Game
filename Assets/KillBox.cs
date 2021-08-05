using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public float maxDistance = 200;
    public float delay = 5;

    float timer = float.MaxValue;
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            timer = 0;

            List<GameObject> list = new List<GameObject>(FindObjectsOfType<GameObject>());
            foreach (GameObject g in list)
            {
                if (Vector3.Distance(g.transform.position, transform.position) > maxDistance)
                {
                    
                    Health h = g.GetComponent<Health>();
                    if (h != null)
                    {
                        h.Damage(int.MaxValue);
                    }
                    else
                    {
                        Destroy(g);
                    }


                }
            }

            list.RemoveAll(go => go == null);
        }
        
        
    }
}
