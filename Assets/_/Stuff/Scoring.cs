using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    // Start is called before the first frame update
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endMessage;
    public bool endGame;


    void Start()
    {

        score = 0;
        scoreText.text = "Score :" + score;
        endGame = false;
    }

    private void UpdateScore()
    {
        scoreText.text = "Score :" + score;
    }

    public void Emprisonned()
    {
        endMessage.text = "oh non! Vous avez enfermé un des participants!";
        endGame = true;

    }
    public void addPoint()
    {
        score = score  + 1;
        UpdateScore();

    }

    public void activateEndgame()
    {
        if(score > 0)
        {
            endMessage.text = "Félicitations! Vous avez réussi à rendre votre projet avec " + score + " minutes d'avance!";

        }
        else
        {
            endMessage.text = "oh non! Vous avez rendu votre devoir avec " + Mathf.Abs(score) + " jours de retard!";
            

        }
        


    }


    public void removePoint()
    {
        score = score  - 1;
        UpdateScore();

    }

}
