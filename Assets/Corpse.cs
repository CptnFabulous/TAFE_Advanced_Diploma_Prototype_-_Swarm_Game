using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public SwarmMember typeCreated;
    
    public void ConvertIntoNewMember(SwarmMember memberInitiating)
    {
        if (typeCreated == null)
        {
            typeCreated = memberInitiating;
        }

        SwarmMember newMember = Instantiate(typeCreated, transform.position, Quaternion.identity);
        memberInitiating.controller.AddMember(newMember);
        newMember.HealthData.current = newMember.HealthData.max;

        Debug.Log("Destroying " + name);

        Destroy(gameObject);
    }
}
