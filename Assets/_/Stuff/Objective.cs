using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Objective : MonoBehaviour
{
    [SerializeField] Scoring scoreText;
    [SerializeField] Testing testing;

    bool isInAWall;
    private Vector3 position;
    private int endgame;


    Transform cofeeTransform;
    // Start is called before the first frame update
    void Start()
    {
        cofeeTransform = GetComponent<Transform>();
        endgame = 0;
        isInAWall = false;

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            scoreText.addPoint();
        }
        

        else if (collision.tag == "Enemy")
        {
            scoreText.removePoint();
        }
        if(collision.tag == "Player" || collision.tag == "Enemy") {


            GameManager.Instance.Enemy.itemPickedUp();
            GameManager.Instance.Player.itemPickedUp();
            randomizePosition();
            
            
            endgame += 1;
                if(endgame ==3)
                { 
                    scoreText.endGame = true;
                scoreText.activateEndgame();
            }
        }
    }

    public void unstuck(Vector3 position)
    {

        (int x, int y) = testing.pathfinding.GetGrid().GetXY(position);
        isInAWall = !testing.pathfinding.GetNode(x, y).isWalkable;
        if (isInAWall)
        {
            randomizePosition();
        }

    }
    public bool WillStuck(int x, int y)
    {
        return (testing.pathfinding.GetGrid().GetXY(cofeeTransform.position) == (x,y));



    }


    private void randomizePosition()
    {
        position = cofeeTransform.position;





        while (position == cofeeTransform.position || isInAWall)
        {
            position = new Vector3(Random.Range(0, 20) * 10 + 5, Random.Range(0, 10) * 10 + 5, 0);

            unstuck(position);




        }
        cofeeTransform.position = position;
    }




}
