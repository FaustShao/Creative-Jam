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

    private void OnTriggerEnter2D(Collider2D other) {
        isUnlocked = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isUnlocked = false;   
    }


    public void ForceDisable(){
        isUnlocked = false;
    }

    public void SoftReset(){
        ForceDisable();
    }
}
