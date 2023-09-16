using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDevice : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isActivated;
    void Start()
    {
        isActivated = false;
    }

    // Update is called once per frame
    public void ActivateGoal(){
        isActivated = true;
    }
}
