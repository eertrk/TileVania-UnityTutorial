using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] int coinPoints = 100;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<GameSession>().AddToScore(coinPoints);
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}
