using UnityEngine;

public class CookieShooterScript : MonoBehaviour
{
    public GameObject cookiePrefab;
    public Transform spawnPoint;
    public float minForce = 5f;
    public float maxForce = 16f;
    public float spawnInterval = 1f;

    void Start()
    {
        InvokeRepeating("ShootCookie", 1f, spawnInterval);
    }

    void ShootCookie()
    {
        GameObject cookie = Instantiate(cookiePrefab, spawnPoint.position, Random.rotation);

        Rigidbody rb = cookie.GetComponent<Rigidbody>();

        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        float randomForce = Random.Range(minForce, maxForce);
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
    }
}