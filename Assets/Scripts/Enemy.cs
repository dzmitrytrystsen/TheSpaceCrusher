using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 300;
    [SerializeField] private int scoreValue = 1;

    [Header("War")]
    [SerializeField] public GameObject[] hitVFX;
    [SerializeField] public GameObject[] explosionVFX;
    [SerializeField] public AudioClip[] explosionSounds;
    [SerializeField] public AudioClip[] hitSounds;
    [SerializeField] private GameObject enemyLaserPrefab;
    [SerializeField] private AudioClip[] enemyLaserSound;
    [SerializeField] private float enemyLaserSpeed = 10f;
    [SerializeField] private float shootCounter;
    [SerializeField] private float minTimeBetweenShots = 0.2f;
    [SerializeField] private float maxTimeBetweenShots = 3f;

    //Cached
    public AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        shootCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shootCounter -= Time.deltaTime;
        if (shootCounter <= 0f)
        {
            EnemyFire();
            shootCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        damageDealer.Hit();
        health = health - damageDealer.GetDamage();

        if (health <= 0)
        {
            Destroy(gameObject);
            TriggerExplosion();
        }

        TriggerHit();
    }

    private void TriggerHit()
    {
        GameObject hit = Instantiate(hitVFX[Random.Range(0, hitVFX.Length)], transform.position, transform.rotation);
        Destroy(hit, 2f);
        AudioSource.PlayClipAtPoint(hitSounds[Random.Range(0, hitVFX.Length)], Camera.main.transform.position);
    }

    private void TriggerExplosion()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);

        GameObject hit = Instantiate(explosionVFX[Random.Range(0, hitVFX.Length)], transform.position,
            transform.rotation);
        Destroy(hit, 2f);
        AudioSource.PlayClipAtPoint(explosionSounds[Random.Range(0, hitVFX.Length)], Camera.main.transform.position);
    }

    private void EnemyFire()
    {
        GameObject enemyLaser = Instantiate(
            enemyLaserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);
        Destroy(enemyLaser, 1f);
        AudioClip clips = enemyLaserSound[Random.Range(0, enemyLaserSound.Length)];
        myAudioSource.PlayOneShot(clips);
    }
}
