using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailSceneCollide : MonoBehaviour
{   
    public static int lastScene;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(backToPReviousScene),1f);
    }
    void backToPReviousScene(){
        SceneManager.LoadScene(lastScene);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
