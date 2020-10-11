using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerCollision : MonoBehaviour, IDamageable
{
    public bool gameWon;
    public UnityEvent OnDeath;
    public void Damage()
    {
        if(!gameWon)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOShakePosition(0.1f, 0.1f, 10, 0, false, true));
            //mySequence.Append(transform.DOMove(transform.position + Vector3.up, 0.5f));
            mySequence.AppendCallback(Dead);
        }
        
    }

    public void SetGameWon(bool value)
    {
        gameWon = value;
    }

    public void Dead()
    {
        OnDeath.Invoke();
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision : " + collision.transform.name);
        ICollectible collectible = collision.transform.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.Collect();
        }
    }


}
