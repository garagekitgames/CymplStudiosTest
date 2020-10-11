using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask wallLayer;
    public MovingEntity myMovingEntity;
    public Animator animator;
    public SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        myMovingEntity = GetComponent<MovingEntity>();
        myMovingEntity.destination = new Vector2Int(myMovingEntity.X, myMovingEntity.Y);
        //animat
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        //if(Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        //{
        //    if (Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Horizontal")) == 1f)
        //    {
        //        if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, wallLayer))
        //        {
        //            movePoint.position += new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0f, 0f);
        //        }

        //    }

        //    else if (Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Vertical")) == 1f)
        //    {
        //        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, CrossPlatformInputManager.GetAxisRaw("Vertical"), 0f), 0.2f, wallLayer))
        //        {
        //            movePoint.position += new Vector3(0f, CrossPlatformInputManager.GetAxisRaw("Vertical"), 0f);
        //        }

        //    }
        //}

        if(Vector3.Distance(transform.position, GridManager.instance.Grid[myMovingEntity.destination.x, myMovingEntity.destination.y].gameObject.transform.position) <= 0.005f)
        {
            if (CrossPlatformInputManager.GetAxisRaw("Horizontal") == 1f)
            {
                if (myMovingEntity.X+1 >= 0 && myMovingEntity.X+1 < Utils.Columns && myMovingEntity.Y >= 0 && myMovingEntity.Y < Utils.Rows)
                {
                    if (GridManager.instance.Grid[myMovingEntity.X + 1, myMovingEntity.Y].Height < 1)
                    {
                        myMovingEntity.destination = new Vector2Int(myMovingEntity.X + 1, myMovingEntity.Y);
                        renderer.flipX = false;
                    }
                }
                    
                
            }
            if (CrossPlatformInputManager.GetAxisRaw("Horizontal") == -1f)
            {
                if (myMovingEntity.X-1 >= 0 && myMovingEntity.X-1 < Utils.Columns && myMovingEntity.Y >= 0 && myMovingEntity.Y < Utils.Rows)
                {
                    if (GridManager.instance.Grid[myMovingEntity.X - 1, myMovingEntity.Y].Height < 1)
                    {
                        myMovingEntity.destination = new Vector2Int(myMovingEntity.X - 1, myMovingEntity.Y);
                        renderer.flipX = true;
                    }
                       
                }
                   
            }
            if (CrossPlatformInputManager.GetAxisRaw("Vertical") == 1f)
            {
                if (myMovingEntity.X >= 0 && myMovingEntity.X < Utils.Columns && myMovingEntity.Y+1 >= 0 && myMovingEntity.Y+1 < Utils.Rows)
                {
                    if (GridManager.instance.Grid[myMovingEntity.X, myMovingEntity.Y + 1].Height < 1)
                    {
                        myMovingEntity.destination = new Vector2Int(myMovingEntity.X, myMovingEntity.Y + 1);
                    }
                        
                }
                    
            }
            if (CrossPlatformInputManager.GetAxisRaw("Vertical") == -1f)
            {
                if (myMovingEntity.X >= 0 && myMovingEntity.X < Utils.Columns && myMovingEntity.Y-1 >= 0 && myMovingEntity.Y-1 < Utils.Rows)
                {
                    if (GridManager.instance.Grid[myMovingEntity.X, myMovingEntity.Y - 1].Height < 1)
                    {
                        myMovingEntity.destination = new Vector2Int(myMovingEntity.X, myMovingEntity.Y - 1);
                    }
                        
                }
                    
            }
        }
        

        if(Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Vertical")) == 1f || Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Horizontal")) == 1f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }


    }
}
