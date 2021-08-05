using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour
{
    public SwarmMember monsterPrefab;
    public List<SwarmMember> swarm = new List<SwarmMember>();
    public int maxSwarmSize = 50;

    Vector3 movementInput;
    
    // Start is called before the first frame update
    void Start()
    {
        if (swarm.Count <= 0)
        {
            SwarmMember firstMonster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            AddMember(firstMonster);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (swarm.Count <= 0 || Input.GetButtonDown("Cancel"))
        {
            Fail();
        }
        
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }

        bool willAttack = Input.GetButtonDown("Fire1");
        bool willConsume = Input.GetButtonDown("Fire2") && swarm.Count < maxSwarmSize;

        foreach (SwarmMember member in swarm)
        {
            if (member != null)
            {
                member.MoveInDirection(movementInput);

                member.CountTimers();

                if (willAttack)
                {
                    //Debug.Log("Attacking");
                    member.Attack();
                }

                if (willConsume)
                {
                    //Debug.Log("Checking corpse");
                    member.CheckCorpseToConsume();
                }
            }
        }






        swarm.RemoveAll(member => member == null);
    }

    void Fail()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    

    public void AddMember(SwarmMember sm)
    {
        //Debug.Log("adding " + sm.name);
        swarm.Add(sm);
        sm.controller = this;
    }

    private void LateUpdate()
    {
        // Move controller so it is in the centre of the horde
        Vector3 centre = swarm[0].transform.position;
        for (int i = 1; i < swarm.Count; i++)
        {
            centre += swarm[i].transform.position;
        }
        centre /= swarm.Count;

        transform.position = centre;
    }
}
