using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private List<Transform> wayPoints;

    private int wayPointindex = 0;

	void Start ()
	{
	    wayPoints = waveConfig.GetWayPoints();
        transform.position = wayPoints[wayPointindex].transform.position;
	}
	
	void Update ()
    {
        EnemyMove();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void EnemyMove()
    {
        if (wayPointindex <= wayPoints.Count - 1)
        {
            var targetPosition = wayPoints[wayPointindex].transform.position;
            var movementFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementFrame);

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
