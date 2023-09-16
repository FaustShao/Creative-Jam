using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableBarrier : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Locks> controlLocks;

    public bool Unlocked;

    
    void Start()
    {
        Unlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool check = true;
        foreach (Locks l in controlLocks){
            
        }
    }
}
