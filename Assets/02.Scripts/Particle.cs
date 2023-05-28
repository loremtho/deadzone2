using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particle : MonoBehaviour
{
    public GameObject coin;
    public float destroyDelay = 0.1f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ĳ���� ���ΰ� �浹");
            GameObject particle =  Instantiate(coin, transform.position, Quaternion.identity);
            particle.transform.position = new Vector3(transform.position.x,transform.position.y + 1.5f,transform.position.z);
            Destroy(particle, destroyDelay);
        }
    }
}
