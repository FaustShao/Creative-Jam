using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update


    [Header("SceneKeyAttributes")]
    public int maxActionCount;

    public int remainingActionCount;
    public int maxRewindCount;

    public int remainingRewindCount;
    
    public bool isSolved;

    //TOD: Replace GameObject Class with actual class
    [Header("GameObjects")]

    public GameObject CurrentActivePlayer;
    public List<GameObject> Player_Lives;

    public GoalDevice Goal;
    public List<GameObject> Walls;

    public List<GameObject> Turrets;

    public GameObject Door;

    public GameObject DoorTrigger;


    void Start()
    {
        isSolved = false;
        remainingActionCount = maxActionCount;
        remainingRewindCount = maxRewindCount;
        CurrentActivePlayer = Player_Lives[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Goal.isActivated){

        }
    }

    public void ProceedGame(){


        remainingActionCount--;


    }

    public void FailGame(){
        //TODO: add Game Fail ;

        //reload Current Scene;
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);


    }

     public void WinGame(){
        //TODO: add Game Win Event ;

        Debug.Log("TODO: Activate Win Game Effect");
        

    }
}
