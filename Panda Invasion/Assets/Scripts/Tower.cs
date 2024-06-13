using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float reloadTime;
    [SerializeField] private float timeSinceLastShot;
    [SerializeField] private float shotRange;

    [SerializeField] private int upgradeLevel;
    [SerializeField] private Sprite[] upgradeSprites;
    [SerializeField] private bool isUpgradable = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (reloadTime >= timeSinceLastShot)
        {
            Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, shotRange);

            if(hitCollider.Length !=0)
            {
                float minDistance = int.MaxValue;
                int index = -1;

                for (int i = 0; i < hitCollider.Length; i++)
                {
                    if (hitCollider[i].CompareTag("Enemy"))
                    {
                        float distance = Vector3.Distance(transform.position, hitCollider[i].transform.position);
                        if(distance < minDistance)
                        {
                            minDistance = distance;
                            index = i;
                        }
                    }
                }

                if(index < 0)
                {
                    return;
                }

                Transform target = hitCollider[index].transform;
                Vector3 direction = target.transform.position - transform.position;

                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().SetDirection(direction);
                timeSinceLastShot = 0;
            }
        }
    }

    public void UpgradeTower()
    {
        if(!isUpgradable)return;

        upgradeLevel++;
        if (upgradeLevel == upgradeSprites.Length - 1) isUpgradable = false;

        GetComponent<SpriteRenderer>().sprite = upgradeSprites[upgradeLevel];

        shotRange++;
        reloadTime -= 0.2f;
    }
}
