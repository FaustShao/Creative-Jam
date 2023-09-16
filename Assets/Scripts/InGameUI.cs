using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InGameUI : MonoBehaviour
{

    public GameController Game;

    public Image[] RoundIndicator;
    public TextMeshProUGUI ActionsIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActionsIndicator.text = Game.remainingActionCount.ToString();

        for (int i = 0; i < RoundIndicator.Length; i++){
            if (i == (Game.maxRewindCount - Game.remainingRewindCount)){

                var tempColor = RoundIndicator[i].color;
                tempColor.a = 255f;
                RoundIndicator[i].color = tempColor;
            }else{
                var tempColor = RoundIndicator[i].color;
                tempColor.a = 0f;
                RoundIndicator[i].color = tempColor;
            }
        }
    }
}
