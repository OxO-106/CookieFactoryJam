using UnityEngine;

public class CookieCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCookies.instance.AddCookie(1);

            Destroy(transform.root.gameObject);
        }
    }
}