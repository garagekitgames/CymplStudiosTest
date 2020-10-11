using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStates
    {
        IDLE,
        CHASE,
        WANDER,
        PATROL
    }

    public enum EnemyType
    {
        CHASER,
        WANDERER,
        PATROLLER
    }

    public EnemyStates _state; //Local variable that represents our state
    public EnemyType enemyType = EnemyType.CHASER;

    public LayerMask playerLayer;

    public MovingEntity myMovingEntity;

    public Vector2Int patrolPoint1;
    public Vector2Int patrolPoint2;
    // Start is called before the first frame update
    void Start()
    {
        _state = EnemyStates.IDLE;
        myMovingEntity = GetComponent<MovingEntity>();
        //if(enemyType == EnemyType.CHASER)
        //{
            
        //}
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case EnemyStates.IDLE:
                Idle();
                break;
            case EnemyStates.CHASE:
                Chase();
                break;
            case EnemyStates.WANDER:
                Wander();
                break;
            case EnemyStates.PATROL:
                Patrol();
                break;
            default:
                break;
        }
    }

    public void Idle()
    {

        switch (enemyType)
        {
            case EnemyType.CHASER:
                
                if (GameManager.instance.player != null)
                {
                    if ((GameManager.instance.player.transform.position - transform.position).sqrMagnitude <= 2f)
                    {
                        Debug.Log("PlayerDetected!");
                        _state = EnemyStates.CHASE;
                    }
                }
                break;
            case EnemyType.WANDERER:

                _state = EnemyStates.WANDER;
                break;
            case EnemyType.PATROLLER:
                //GridManager.instance.GetRandomOpenTileVector();
                //GridManager.instance.GetRandomOpenTileVector();
                //patrolPoint1 = new Vector2Int(myMovingEntity.X, myMovingEntity.Y);
                patrolPoint1 = GridManager.instance.GetRandomOpenTileVector();
                myMovingEntity.destination = patrolPoint1;
                
                patrolPoint2 = GridManager.instance.GetRandomOpenTileVector();
                _state = EnemyStates.PATROL;
                break;
            default:
                break;
        }
        //Debug.Log("PlayerDetected! : " + (GameManager.instance.player.transform.position - transform.position).sqrMagnitude);
        
            
       
    }

    public void Chase()
    {
        myMovingEntity.destination = new Vector2Int(GameManager.instance.player.X, GameManager.instance.player.Y);

        if(GameManager.instance.player != null)
        {
            if ((GameManager.instance.player.transform.position - transform.position).sqrMagnitude <= 0.5f)
            {
                _state = EnemyStates.IDLE;
            }
        }
        else
        {
            _state = EnemyStates.IDLE;
        }
        

    }

    public void Wander()
    {
        if(myMovingEntity.pathReached)
        {
            //currentWanderDestination = GridManager.instance.GetRandomOpenTileVector();
            myMovingEntity.destination = GridManager.instance.GetRandomOpenTileVector();
        }
        
    }

    public void Patrol()
    {
        if (myMovingEntity.pathReached)
        {
            if((GridManager.instance.Grid[patrolPoint1.x, patrolPoint1.y].gameObject.transform.position - transform.position).sqrMagnitude <= 0.5f)
            {
                myMovingEntity.destination = patrolPoint2;
            }
            else if ((GridManager.instance.Grid[patrolPoint2.x, patrolPoint2.y].gameObject.transform.position - transform.position).sqrMagnitude <= 0.5f)
            {
                myMovingEntity.destination = patrolPoint1;
            }
        }

    }
}
