using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private int health = 1000;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float paddingX = 1f;
    [SerializeField] private float paddingY = 1f;
    [SerializeField] private float shootPeriod = 0.1f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 15f;
    [SerializeField] public AudioClip[] laserSounds;
    [SerializeField] public GameObject[] hitVFX;
    [SerializeField] public GameObject explosionVFX;
    [SerializeField] public AudioClip[] explosionSounds;
    [SerializeField] public AudioClip[] hitSounds;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    //Cached
    public AudioSource myAudioSource;

    Coroutine shootOnHoldCoroutine;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        SetUpMoveBorders();
    }

    void Update()
    {
        Move();
        Shoot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }

        damageDealer.Hit();
        health = health - damageDealer.GetDamage();

        if (health <= 0)
        {
            TriggerExplosion();
            GameOver();
        }

        GetHealth();
        TriggerHit();
    }

    public float GetHealth()
    {
        return health;
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
            var laserPos = new Vector2(transform.position.x, transform.position.y + 1f);

            GameObject laser = Instantiate(
                laserPrefab,
                laserPos,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            Destroy(laser, 1f);
            AudioClip clips = laserSounds[Random.Range(0, laserSounds.Length)];
            myAudioSource.PlayOneShot(clips);

            yield return new WaitForSeconds(shootPeriod);
        }
    }

    private void TriggerHit()
    {
        GameObject hit = Instantiate(hitVFX[Random.Range(0, hitVFX.Length)], transform.position, transform.rotation);
        Destroy(hit, 2f);

        AudioSource.PlayClipAtPoint(hitSounds[Random.Range(0, hitVFX.Length)], Camera.main.transform.position);
    }

    private void TriggerExplosion()
    {
        GameObject hit = Instantiate(explosionVFX, transform.position,
            transform.rotation);
        Destroy(hit, 5f);

        AudioSource.PlayClipAtPoint(explosionSounds[Random.Range(0, hitVFX.Length)], Camera.main.transform.position);
    }

    private void GameOver()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;

        StartCoroutine(WaitForIt(3f));
    }

    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(2);
    }
}
