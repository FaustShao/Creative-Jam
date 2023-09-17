using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailSceneController : MonoBehaviour
{

    public static int previouScene;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(backToPReviousScene),1f);
    }


    void backToPReviousScene(){
        SceneManager.LoadScene(previouScene);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
