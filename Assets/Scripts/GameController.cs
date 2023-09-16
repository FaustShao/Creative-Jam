using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("SceneKeyAttributes")]
    public int maxActionCount;
    public int maxRewindCount;
    public string sceneName;
    public Vector2 spawnPoint;
    public int remainingActionCount;
    public int playerGenerationCounter = 0;
    public int remainingRewindCount;
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
    public GameObject level1Object;

    void Awake()
    {
      DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
      // Set the initial game state
      isSolved = false;

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
      HandleExhaustedPlayer();
        //if(Goal.isActivated){
        //WinGame();
        //}
    }

    public void ProceedGame()
    {
      remainingActionCount--;

      foreach (GameObject player in Player_Lives)
      {
        // Move each player according to their recorded action
        if (player.GetComponent<PlayerController>().playerGeneration < playerGenerationCounter)
        {
          player.GetComponent<PlayerController>().PlayStep(maxActionCount - remainingActionCount);
        }
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


    public void HandleExhaustedPlayer()
    {
      if (CurrentActivePlayer.GetComponent<PlayerController>().exhausted)
      {
        // Naming the exhausted player and incrementing the generation counter
        CurrentActivePlayer.gameObject.name = "PlayerLife" + playerGenerationCounter;
        playerGenerationCounter++;

        // Instantiating the new player
        GameObject newPlayer = Instantiate(CurrentActivePlayer, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
        PlayerController newPlayerController = newPlayer.GetComponent<PlayerController>();
        newPlayerController.playerGeneration = playerGenerationCounter;
        newPlayerController.gameController = this; // Linking the GameController
        newPlayerController.exhausted = false;
        Player_Lives.Add(newPlayer);
        // Setting the new player as the active player
        CurrentActivePlayer = newPlayer;
        CurrentActivePlayer.GetComponent<PlayerController>().exhausted = false;

        // Setting the exhausted flag to false for the new player
      }
    }
}
