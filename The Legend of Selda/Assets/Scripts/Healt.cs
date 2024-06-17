using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healt : MonoBehaviour
{
    [SerializeField] private float maxHealt = 100.0f;
    [SerializeField] private float currentHealt;

    [SerializeField] private float hitTime;

    private Rigidbody2D _Rigidbody;

    private void Start()
    {
        currentHealt = maxHealt;
        _Rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Hit(float damage, Vector3 position)
    {
        currentHealt -= damage;
        if(currentHealt <= 0)
        {
            currentHealt = 0;
            StartCoroutine(Dead());
        }

        Vector3 dir = (transform.position - position).normalized;

        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            dir = new Vector2(dir.x, 0).normalized;
        }
        else
        {
            dir = new Vector2(0, dir.y).normalized;
        }

        StartCoroutine(HitCorrutine(dir));

    }

    IEnumerator HitCorrutine(Vector3 dir)
    {
        float timeCount = 0;

        PlayerController playerController;
        if(TryGetComponent<PlayerController>(out playerController))
        {
            playerController.enabled = false;
        }


        while(timeCount < hitTime)
        {
            timeCount += Time.fixedDeltaTime;
            _Rigidbody.AddForce(dir * 5, ForceMode2D.Impulse);
            yield return new WaitForFixedUpdate();
        }

        if(playerController != null)
        {
            playerController.enabled = true;
        }
    }

    IEnumerator Dead()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(5);
        spriteRenderer.enabled = true;
        currentHealt = maxHealt;
    }
}
