 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;

public class CharacterPathfindingMovementHandler : MonoBehaviour
{

    private const float speed = 40f;

    public bool isMoving;

    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    [SerializeField] private GameObject objective;







    private void Start()
    {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        animatedWalker = new AnimatedWalker(unitAnimation, UnitAnimType.GetUnitAnimType("dMarine_Idle"), UnitAnimType.GetUnitAnimType("dMarine_Walk"), 1f, 1f);
    }

    private void Update()
    {

        isMoving = (pathVectorList != null);
        HandleMovement();
        unitSkeleton.Update(Time.deltaTime);

        if (Input.GetKeyDown("space") && (!GameManager.Instance.scoring.endGame))
        {
            SetTargetPosition(objective.GetComponent<Transform>().position);
        }
    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                animatedWalker.SetMoveVector(moveDir);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    animatedWalker.SetMoveVector(Vector3.zero);
                }
            }
        }
        else
        {
            animatedWalker.SetMoveVector(Vector3.zero);
        }
    }

    public void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void itemPickedUp()
    {
        SetTargetPosition(pathVectorList[currentPathIndex]);

    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        Debug.Log(targetPosition);
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    // Get Mouse Position in World with Z = 0f
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