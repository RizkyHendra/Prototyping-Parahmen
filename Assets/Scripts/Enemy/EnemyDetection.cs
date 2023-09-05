using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    private MovementInput movementInput;

    public LayerMask layerMask;

    [SerializeField] Vector3 inputDirection;
    [SerializeField] private EnemyScript currentTarget;

    [SerializeField] private GameObject enemyTargetMarker;
    public int targetIndex;

    public GameObject cam;

    private void Start()
    {
        movementInput = GetComponentInParent<MovementInput>();
        targetIndex = 0;
    }

    private void Update()
    {
        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        inputDirection = forward * movementInput.moveAxis.y + right * movementInput.moveAxis.x;
        inputDirection = inputDirection.normalized;
        /*
        RaycastHit info;

        if (Physics.SphereCast(transform.position, 3f, inputDirection, out info, 10,layerMask))
        {
            if(info.collider.transform.GetComponent<EnemyScript>().IsAttackable())
                currentTarget = info.collider.transform.GetComponent<EnemyScript>();
        }*/

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            targetIndex += 1;
            if (targetIndex >= enemyManager.allEnemies.Length)
            {
                targetIndex = 0;
            }

            if(enemyManager.allEnemies[targetIndex].enemyAvailability == true)
            {
                currentTarget = enemyManager.allEnemies[targetIndex].enemyScript;
            }
            else
            {
                targetIndex += 1;
            }
        }

        SetMarketTarget();
    }

    private void SetMarketTarget()
    {
        if (CurrentTarget() != null)
        {
            enemyTargetMarker.transform.position = new Vector3(CurrentTarget().transform.position.x, CurrentTarget().transform.position.y + 2, CurrentTarget().transform.position.z);
            enemyTargetMarker.SetActive(true);
        }
        else
        {
            enemyTargetMarker.SetActive(false);
        }
    }

    public EnemyScript CurrentTarget()
    {
        return currentTarget;
    }

    public void SetCurrentTarget(EnemyScript target)
    {
        currentTarget = target;
    }

    public float InputMagnitude()
    {
        return inputDirection.magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, inputDirection);
        Gizmos.DrawWireSphere(transform.position, 1);
        if(CurrentTarget() != null)
            Gizmos.DrawSphere(CurrentTarget().transform.position, .5f);
    }
}
