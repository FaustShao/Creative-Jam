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
    public GameObject Endpoint;
    public List<Vector3> boxPositions;
    public List<GameObject> boxs;
    public PlayerController CurrentActivePlayer;
    public List<PlayerController> Player_Live;
    public GoalDevice Goal;
    public List<GameObject> Walls;
    public List<GameObject> Turrets;
    public UnlockableBarrier Door;
    public Locks DoorTrigger;

    int playerIndex = 0;
    bool isPause;

    //[Header("Prefabs")]
    //public GameObject playerPrefab;
    //public GameObject level1Object;

    void Start()
    {
      isSolved = false;
      remainingActionCount = maxActionCount;
      remainingRewindCount = maxRewindCount;
      isPause = false; 

      CurrentActivePlayer = Player_Live[maxRewindCount - remainingRewindCount];
      for(int i =0; i < Player_Live.Count;i++){
        if(i != maxRewindCount - remainingRewindCount){
          Player_Live[i].gameObject.SetActive(false);
        }
      }
    }


    public bool playerAllSettle(){
      
      foreach(PlayerController p in Player_Live){
        if (p.isMoving()) return false;
      }
      return true;
    }
    // Update is called once per frame
    void Update()
    {
      CheckPause();
      if(isPause){
        CheckFinishedReplay();
      }
      CheckWinCondition();
    }

    void CheckWinCondition()
    {
      // Get player's and core object's positions
      Vector2 playerPosition = CurrentActivePlayer.transform.position;
      Vector2 corePosition = Endpoint.transform.position;

      // Define the grid size (assuming a grid size of 1 unit here)
      float gridSize = 1f;

      // Check if the player is beside the core object in any of the four grid directions
      if (Vector2.Distance(playerPosition, corePosition + Vector2.up * gridSize) < 0.1f ||
          Vector2.Distance(playerPosition, corePosition + Vector2.down * gridSize) < 0.1f ||
          Vector2.Distance(playerPosition, corePosition + Vector2.left * gridSize) < 0.1f ||
          Vector2.Distance(playerPosition, corePosition + Vector2.right * gridSize) < 0.1f)
      {
        WinGame();
      }
    }


    void CheckFinishedReplay(){
      if(CurrentActivePlayer.ReplayIndex > 0){
        isPause = false;
        playerIndex++;
        if(playerIndex < Player_Live.Count){
          CurrentActivePlayer = Player_Live[playerIndex];
        }else{
          FailGame();
        }
      }
    }

    public void DecreaseActionCount(){
      remainingActionCount--;

      if(remainingActionCount == 0){
        remainingRewindCount--;
        //death animation then Convert to Phantom
        remainingActionCount = maxActionCount;
        ResetWalls();
        CurrentActivePlayer.ConvertToPhantom();
        ResetAllPhantom();
        GetNextPlayer();
        
      }
    }

    public void ResetWalls()
    {
      for(int i=0; i<boxs.Count; i++)
      {
        Debug.Log("reset"+boxs[i]);
        boxs[i].transform.position = boxPositions[i];
      }
    }


    

    void ResetAllPhantom(){
      foreach ( PlayerController p in Player_Live){
        p.ResetPhantom();
      }
    }
    public void GetNextPlayer(){
      playerIndex++;
      if(playerIndex < Player_Live.Count){
        CurrentActivePlayer = Player_Live[playerIndex];
        CurrentActivePlayer.gameObject.SetActive(true);
      }else{
        FailGame();
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
            Time.timeScale = 0;
        }
    }
}
