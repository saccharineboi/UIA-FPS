using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
        public float speed;
        public float obstacleRange;

        bool isDead;
        [SerializeField]
        GameObject fireballPrefab;
        GameObject fireball;

        void Start()
        {
                
        }

        void Update()
        {
                if (!isDead)
                {
                        Wander(); 
                }
        }

        void Wander()
        {
                transform.Translate(transform.forward * speed * Time.deltaTime);

                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.SphereCast(ray, 1.5f, out RaycastHit hit))
                {
                        if (hit.distance < obstacleRange)
                        {
                                if (hit.transform.tag == "Player" && fireball == null)
                                {
                                        fireball = Instantiate(fireballPrefab);
                                        fireball.transform.position = transform.position + transform.forward * 1.5f;
                                        fireball.transform.rotation = transform.rotation;
                                }
                                float angle = Random.Range(-120f, 120f);
                                transform.Rotate(0f, angle, 0f);
                        }
                }
        }

        public void GetHit()
        {
                StartCoroutine(Die());
        }

        IEnumerator Die()
        {
                isDead = true;
                transform.Rotate(60f, 0f, 0f);
                yield return new WaitForSeconds(1.0f);
                Destroy(gameObject);
        }
}
