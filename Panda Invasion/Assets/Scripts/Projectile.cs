using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float lifeDuration;
    // Start is called before the first frame update
    void Start()
    {
        direction = direction.normalized;

        float angleRad = Mathf.Atan2(-direction.x, direction.y);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);

        Destroy(gameObject, lifeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (direction * speed) * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().RecibeDamage(damage);
            Destroy(gameObject);
        }
    }
}
