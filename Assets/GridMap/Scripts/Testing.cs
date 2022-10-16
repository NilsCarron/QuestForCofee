

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Testing : MonoBehaviour
{

    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private CharacterPathfindingMovementHandler characterPathfinding;
    [SerializeField] private GameObject objective ;

    public Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(20, 10);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);


        }



        if ((!GameManager.Instance.scoring.endGame) && Input.GetMouseButtonDown(1) &&
            !GameManager.Instance.Enemy.isMoving && !GameManager.Instance.Player.isMoving)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            (int x, int y) = pathfinding.GetGrid().GetXY(mouseWorldPosition);

            if (!objective.GetComponent<Objective>().WillStuck(x, y))
            {

                pathfinding.GetNode(x, y).SetIsWalkable(false);
            }
        }

        if ((!GameManager.Instance.scoring.endGame) && Input.GetMouseButtonDown(0) &&
            !GameManager.Instance.Enemy.isMoving && !GameManager.Instance.Player.isMoving)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            (int x, int y) = pathfinding.GetGrid().GetXY(mouseWorldPosition);


            pathfinding.GetNode(x, y).SetAcceleration();

        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;

        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

}
