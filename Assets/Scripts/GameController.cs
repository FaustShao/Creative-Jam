using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update


    [Header("SceneKeyAttributes")]
    public int maxActionCount;
    public string nextScene;
    public int remainingActionCount;
    public int maxRewindCount;

    public int remainingRewindCount;
    
    public bool isSolved;

    public MenuController Menu;

    //TOD: Replace GameObject Class with actual class
    [Header("GameObjects")]

    public PlayerController CurrentActivePlayer;
    public List<PlayerController> Player_Lives;

    public GoalDevice Goal;
    public List<GameObject> Walls;

    public List<GameObject> Turrets;

    public UnlockableBarrier Door;

    public Locks DoorTrigger;


    void Start()
    {
        //isSolved = false;
        //remainingActionCount = maxActionCount;
        //remainingRewindCount = maxRewindCount;
        //CurrentActivePlayer = Player_Lives[0];
    }

    // Update is called once per frame
    void Update()
    {

        CheckPause();
        //if(Goal.isActivated){
            //WinGame();
        //}
    }

    public void ProceedGame(){
        remainingActionCount--;
        foreach (PlayerController p in Player_Lives){
            p.PlayStep(maxActionCount - remainingActionCount);
        }
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

    void CheckPause(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Menu.isActiveOnStart = true;
            Menu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
