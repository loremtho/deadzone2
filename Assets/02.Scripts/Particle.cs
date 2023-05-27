using UnityEngine;

public class Particle : MonoBehaviour
{
    public GameObject coin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ĳ���� ���ΰ� �浹");
            Instantiate(coin, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
