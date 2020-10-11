using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class GoalCollision : MonoBehaviour
{
    //public GameObject gateObject;
    public UnityEvent GameWin;
    public bool allCoinsCollected;
    public SpriteRenderer renderer;
    public Sprite openGate;
    public Ease ease;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(allCoinsCollected)
        {
            Debug.Log("collision : " + collision.transform.name);
            GameWin.Invoke();
        }
        
    }

    public void OnAllCoinsCollected()
    {
        OpenGate();
    }
    void OpenGate()
    {
        allCoinsCollected = true;
        renderer.sprite = openGate;
        transform.DOMove(transform.position + Vector3.up * 0.5f, 0.25f).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
        //gateObject.SetActive(false);
    }
}
