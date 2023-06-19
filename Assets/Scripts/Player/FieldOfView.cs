using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public float height;
    public PlayerMovement playerMove;
    public LayerMask enemyMask;
    public LayerMask obsMask;

    [SerializeField] float delay = 0.2f;

    public List<Transform> visibleTargets = new List<Transform>();

    private void Start()
    {
        StartCoroutine("FindTargets", delay);
        playerMove = GetComponent<PlayerMovement>();
    }

    //set an infinate loop that check each number of seconds if the player did catch something on sight
    IEnumerator FindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        playerMove.pickAnEnemy = 0;
        //set an array of all the object, with the specific layer, that entered the cast sphere
        Collider[] targetsInFieldView = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
        for (int i = 0; i < targetsInFieldView.Length; i++)
        {
            Transform target = targetsInFieldView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            //if the position is on the middle of the camera view// that means the player is looking right at it
            if (Vector3.Angle(transform.forward, dirToTarget) <viewAngle)
            {
                float distTarget = Vector3.Distance(transform.position, target.position);
                //cast a ray that make sure that the target is not hiding behind anything
                if (!Physics.Raycast(transform.position, dirToTarget, distTarget, obsMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
        playerMove.enemies = visibleTargets;
    }

    //always make sure that the field of view is with the player rotation/position
    public Vector3 DirFromAngle(float angleInDegree, bool angelIsGlobal)
    {
        if (!angelIsGlobal)
        {
            angleInDegree += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }
}
