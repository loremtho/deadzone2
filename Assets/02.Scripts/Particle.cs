using UnityEngine;

public class Particle : MonoBehaviour
{
    public GameObject coin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("캐릭터 코인과 충돌");
            Instantiate(coin, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
