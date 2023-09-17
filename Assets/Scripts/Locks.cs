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
    

    

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") || other.CompareTag("MovableBox")){
            isUnlocked = true;
        }
        
    }


    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player") || other.CompareTag("MovableBox")){
            Debug.Log("Not LOcked");
            isUnlocked = false;
        }
    }


    public void ForceDisable(){
        isUnlocked = false;
    }

    public void SoftReset(){
        ForceDisable();
    }
}
