using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locks : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isUnlocked;
    void Start()
    {
        isUnlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkUnlock();
    }

    /*

    TODO: Add Actual detection for Unlock
    */
    void checkUnlock(){
        if(true){
            isUnlocked = true;
        }else{
            isUnlocked = false;
        }
    }
}
