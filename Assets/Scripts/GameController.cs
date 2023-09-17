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

      isSolved = false;
      remainingActionCount = maxActionCount;
      remainingRewindCount = maxRewindCount;
 

      CurrentActivePlayer = Player_Live[maxRewindCount - remainingRewindCount];
      CurrentActivePlayer.isAvailable = true;

      for(int i = 0; i < Player_Live.Count; i++){
        if(i != (maxRewindCount - remainingRewindCount)){
          Player_Live[i].respawnPoint = Player_Live[i].transform.position;
          Player_Live[i].isAvailable = false;
          Player_Live[i].gameObject.SetActive(false);
        }
      }
    }

    // Update is called once per frame
    void Update()
    {
      CheckPause();
    }

    public void ProceedGame()
    {
      ResetAllUnAvailable();
      remainingActionCount--;

      CheckEndOfRewind();
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
        CurrentActivePlayer.isAvailable = false;
        CurrentActivePlayer.exhausted = true;
        remainingActionCount = maxActionCount;
        remainingRewindCount--;
        if(remainingRewindCount != 0){
          int newIndex = maxRewindCount - remainingRewindCount;
          CurrentActivePlayer = Player_Live[newIndex];
          CurrentActivePlayer.gameObject.SetActive(true);
          CurrentActivePlayer.isAvailable = true;
        }else{
          FailGame();
        }
      }
    }

    public void ResetAllUnAvailable(){
      foreach ( PlayerController p in Player_Live){
        if(p.exhausted){
          p.SoftReset();
          p.isInReplay = true;
          p.exhausted = false;
        }
      }
    }

    void CheckPause(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Menu.isActiveOnStart = true；
            Time.timeScale = 0;
        }
    }



    
}
