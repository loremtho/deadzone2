using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particle : MonoBehaviour
{
    public GameObject coin;
    public GameObject obstacle;
    public float destroyDelay = 0.1f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            GameObject particle =  Instantiate(coin, transform.position, Quaternion.identity);
            particle.transform.position = new Vector3(transform.position.x,transform.position.y + 1.5f,transform.position.z +1.5f);
            Destroy(particle, destroyDelay);
        }

        if(other.gameObject.CompareTag("Obstacle"))
        {
            GameObject particle = Instantiate(obstacle, transform.position, Quaternion.identity); ;
            particle.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z + 1.5f);
            Destroy(particle, destroyDelay);

        }
    }

}
