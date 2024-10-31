using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f;
    [Header("Idle Settings")]
    [SerializeField] private float idleRotationAngle = 0f;

    private Transform target;
    private float timeUntilFire;

    private void Start()
    {
        
        if (transform.position.x < 0)
        {
            
            turretRotationPoint.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            
            turretRotationPoint.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();

            
            if (target == null)
            {
                RotateToIdle();
                return;
            }
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
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
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 190f;

        if (turretRotationPoint.localScale.x < 0)
        {
            angle += 190f;
        }

        float currentAngle = turretRotationPoint.rotation.eulerAngles.z;
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, angle));

        if (angleDifference > 170f)
        {
            turretRotationPoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (direction.x < 0)
        {
            turretRotationPoint.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            turretRotationPoint.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void RotateToIdle()
    {
        Quaternion idleRotation = Quaternion.Euler(new Vector3(0, 0, idleRotationAngle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, idleRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
