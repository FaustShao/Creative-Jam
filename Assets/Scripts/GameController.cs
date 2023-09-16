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
    public Vector2 spawnPoint;
    public bool isSolved;

    //TOD: Replace GameObject Class with actual class
    [Header("GameObjects")]

    public GameObject CurrentActivePlayer;
    public List<GameObject> Player_Lives;

    public GoalDevice Goal;
    public List<GameObject> Walls;

    public List<GameObject> Turrets;

    public UnlockableBarrier Door;

    public Locks DoorTrigger;

    [Header("Prefabs")]
    public GameObject playerPrefab;


    void Start()
    {
      // Set the initial game state
      isSolved = false;
      remainingActionCount = maxActionCount;
      remainingRewindCount = maxRewindCount;

      // Instantiate the first player at the spawn point and set it as the current active player
      GameObject newPlayer = Instantiate(playerPrefab, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
      CurrentActivePlayer = newPlayer;
      Player_Lives.Add(newPlayer);

      // Link the new player to this game controller
      newPlayer.GetComponent<PlayerController>().gameController = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Goal.isActivated){
            //WinGame();
        //}
    }

    public void ProceedGame(){
        //remainingActionCount--;
        //foreach (PlayerController p in Player_Lives){
            //p.PlayStep(maxActionCount - remainingActionCount);
        //}
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
