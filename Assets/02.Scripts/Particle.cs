using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particle : MonoBehaviour
{
    public GameObject coin;
    public float destroyDelay = 2.0f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("캐릭터 코인과 충돌");
            GameObject particle =  Instantiate(coin, transform.position, Quaternion.identity);

            Destroy(particle, destroyDelay);

            Destroy(gameObject);
        }

    }
}
