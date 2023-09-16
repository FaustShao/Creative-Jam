using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("SceneKeyAttributes")]
    public int maxActionCount;
    public string nextScene;
    public int maxRewindCount;
    public string sceneName;
    public Vector2 spawnPoint;
    public int remainingActionCount;
    public int playerGenerationCounter = 0;
    public int remainingRewindCount;
    public bool isSolved;

    public MenuController Menu;

    //TOD: Replace GameObject Class with actual class
    [Header("GameObjects")]

    public PlayerController CurrentActivePlayer;
    public List<PlayerController> Player_Live;
    public GoalDevice Goal;
    public List<GameObject> Walls;
    public List<GameObject> Turrets;
    public UnlockableBarrier Door;
    public Locks DoorTrigger;

    //[Header("Prefabs")]
    //public GameObject playerPrefab;
    //public GameObject level1Object;

    void Start()
    {
      //maxActionCount = 6;
      //maxRewindCount = 2;
      // Set the initial game state
      isSolved = false;
      remainingActionCount = maxActionCount;
      remainingRewindCount = maxRewindCount;
      // Instantiate the first player at the spawn point and set it as the current active player
      //GameObject newPlayer = Instantiate(playerPrefab, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
      //CurrentActivePlayer = newPlayer;
      //Player_Lives.Add(newPlayer);

      CurrentActivePlayer = Player_Live[maxRewindCount - remainingRewindCount];

      Debug.Log( Player_Live[0]);

      // Link the new player to this game controller
      //newPlayer.GetComponent<PlayerController>().gameController = this;
    }

    // Update is called once per frame
    void Update()
    {
      //HandleExhaustedPlayer();
      CheckPause();
    }

    public void ProceedGame()
    {
      //remainingActionCount = CurrentActivePlayer.GetComponent<PlayerController>().remain;
      Debug.Log("remainActionCount" + remainingActionCount);
      ReplayAllAvailable();
      remainingActionCount--;

      CheckEndOfRewind();


      /*
      foreach (GameObject player in Player_Lives)
      {
        if(player.GetComponent<PlayerController>().remain == maxActionCount)
        {
          player.GetComponent<PlayerController>().ResetToRespawnPoint();
        }
        // Move each player according to their recorded action
        if (player.GetComponent<PlayerController>().playerGeneration < playerGenerationCounter)
        {
          player.GetComponent<PlayerController>().PlayStep(maxActionCount - remainingActionCount);
        }
      }
      */
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

    public void CheckEndOfRewind(){
      
      if(remainingActionCount == 0){
        
        remainingActionCount = maxActionCount;
        CurrentActivePlayer.SoftReset(spawnPoint);
        ReplayAllAvailable();
        remainingRewindCount--;

        CurrentActivePlayer = Player_Live[maxRewindCount - remainingRewindCount];
        
      }
    }

    public void ResetAllAvailable(){

      for(int i =0; i < 3; i++){
        if(Player_Live[i].isInReplay){
          Player_Live[i].SoftReset(spawnPoint);
        }
      }

    }

    void CheckPause(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Menu.isActiveOnStart = true;
            Menu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }


    void ReplayAllAvailable(){
      for(int i = 0; i < 3; i++){
        if(Player_Live[i].isInReplay){
            Player_Live[i].PlayStep(maxActionCount - remainingActionCount);
        }
      }
    }
}
