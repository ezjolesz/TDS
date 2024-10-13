using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float roationSpeed = 200f;

    private Transform target;
    private void Update()
    {
        FindTarget();

        if (target != null)
        {
            RotateTowardsTarget();

            if (!CheckTargetIsInRange())
            {
                target = null;
            }
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,targetingRange, (Vector2) transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, roationSpeed * Time.deltaTime );

    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward,targetingRange);

    }
}
