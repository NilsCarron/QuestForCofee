using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CharacterPathfindingMovementHandler Player;
    [SerializeField] public CharacterPathfindingMovementHandler Enemy;
    [SerializeField] public bool ControlledByAstar;
    [SerializeField] public Scoring scoring;
    
    
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Game Manager is Null!");
            }

            return instance;
        }


    }

    private void Awake()
    {
        ControlledByAstar = true;
        instance = this;
    }

}