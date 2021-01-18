using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
        public float speed;
        public int damage;

        void Update()
        {
                transform.Translate(transform.forward * speed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
                if (other.tag == "Player")
                {
                        Player player = other.GetComponent<Player>();
                        if (player != null)
                        {
                                player.GetDamaged(damage);
                        }
                }
                Destroy(gameObject);
        }
}
