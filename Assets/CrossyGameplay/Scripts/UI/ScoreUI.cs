using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text myScoreTxt;

    private void Update()
    {
        myScoreTxt.text = ""+ScoreManager.Instance.score;
    }
}
