using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private List<Transform> wayPoints;
    [SerializeField] private float enemyMoveSpeed = 0.1f;

    private int wayPointindex = 0;

	void Start ()
	{
	    transform.position = wayPoints[wayPointindex].transform.position;
	}
	
	void Update ()
    {
        EnemyMove();
    }

    private void EnemyMove()
    {
        if (wayPointindex <= wayPoints.Count - 1)
        {
            var targetPosition = wayPoints[wayPointindex].transform.position;
            var moveSpeed = enemyMoveSpeed;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed);

            if (transform.position == targetPosition)
            {
                wayPointindex++;
            }
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
