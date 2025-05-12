using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallWait = 2f;
    public float destroyWait = 1f;

    private bool isFalling;
    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        GameController.OnReset += Respawn;
    }

    private void OnDestroy()
    {
        GameController.OnReset += Respawn;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyWait);
        gameObject.SetActive(false);
    }

    private void Respawn()
    {
        isFalling = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.SetActive(true);
    }
}
