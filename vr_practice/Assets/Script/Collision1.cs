using UnityEngine;

public class Collision1 : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != gameObject)
      {
        Debug.Log("Столкновение! Куб " + gameObject.name + "врезался в " + collision.gameObject.name);

        GetComponent<Renderer>().material.color = Color.red;

        Destroy(gameObject, 0.1f);

        Destroy(collision.gameObject);
      }
    }
}