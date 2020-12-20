using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase singleton
public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    public void SetPlayerPosition(int walkedRows)
    {
        if (walkedRows > score)
        {
            score = walkedRows;
        }
    }

    //Variables y metodos para el Singleton
    public static ScoreManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}
