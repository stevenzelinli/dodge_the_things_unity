using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    public PowerUpType typeKey;

    private void Update()
    {
		transform.Rotate(new Vector3(40, 100, 60) * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.PowerUp(typeKey);
            Destroy(gameObject);
        }
    }
}
