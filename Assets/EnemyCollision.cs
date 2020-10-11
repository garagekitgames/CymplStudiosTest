using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage();
        }   
    }
}
