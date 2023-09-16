using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isActiveOnStart;    
    void Start()
    {
        if(!isActiveOnStart){
            Time.timeScale = 1;
        }else{
            Time.timeScale = 0;
        }
        
        gameObject.SetActive(isActiveOnStart);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
