using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float paddingX = 1f;
    [SerializeField] private float paddingY = 1f;
    [SerializeField] private float shootPeriod = 0.1f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 15f;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

   Coroutine shootOnHoldCoroutine;

    void Start ()
	{
	    SetUpMoveBorders();
	}

    void Update ()
	{
	    Move();
	    Shoot();
	}

    private void Shoot()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            shootOnHoldCoroutine = StartCoroutine(ShootOnHold());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(shootOnHoldCoroutine);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax); 
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBorders()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingX;
        xMax = gameCamera.ViewportToScreenPoint(new Vector3(0.0075f, 0, 0)).x - paddingX;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingY;
        yMax = gameCamera.ViewportToScreenPoint(new Vector3(0, 0.0055f, 0)).y + paddingY;
    }

    private IEnumerator ShootOnHold()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            Destroy(laser, 1f);

            yield return new WaitForSeconds(shootPeriod);
        }
    }
}
