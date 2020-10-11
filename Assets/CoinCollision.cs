using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CoinCollision : MonoBehaviour, ICollectible
{
    public UnityEvent OnCoinCollected;
    public void Collect()
    {
        Sequence mySequence = DOTween.Sequence();

        //mySequence.Append(transform.DOJump(transform.position, 0.5f, 1, 0.5f));
        mySequence.Append(transform.DOShakePosition(0.1f, 0.1f, 10, 0, false, true));
        mySequence.AppendCallback(AnimOver);
        
    }

    public void AnimOver()
    {
        Destroy(this.gameObject);
        OnCoinCollected.Invoke();
    }
    
}
