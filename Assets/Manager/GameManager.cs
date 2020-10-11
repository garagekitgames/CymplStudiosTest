using RoboRyanTron.Unite2017.Sets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SO;
using garagekitgames;
public class GameManager : MonoBehaviour
{
    #region Instance
    public static GameManager instance = null;
    public ThingRuntimeSet coinRuntimeSet;
    public UnityEvent allCoinsCollected;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
    #endregion
    public GameObject playerPrefab;
    //public GameObject smallEnemyPrefab;
    //public GameObject normalEnemyPrefab;
    //public GameObject bigEnemyPrefab;

    public GameObject[] enemies;

    //public GameObject goalObject;
    public Entity player;

    public List<Entity> entities = new List<Entity>();
    public GridManager g_Manager;
    public IntVariable noOfTimesPlayed;

    public IntVariable level;

    public int enemyCount;

    public enum EnemyType
    {
        SMALL = 0,
        NORMAL = 1,
        BIG = 2
    }
    // Start is called before the first frame update
    void Start()
    {


        PersistableSO.Instance.LoadVersion();
        noOfTimesPlayed.value = noOfTimesPlayed.value + 1;
        Debug.Log("Game Launch : " + noOfTimesPlayed.value);
        if (noOfTimesPlayed.value > 0)
        {
            PersistableSO.Instance.Load();
            Debug.Log("Game Launch Times : " + noOfTimesPlayed.value);
            PersistableSO.Instance.SaveVersion();
        }
        else
        {


            Debug.Log("Game Launch First Time : " + noOfTimesPlayed.value);
            PersistableSO.Instance.Save();
            PersistableSO.Instance.SaveVersion();
        }


        enemyCount = Mathf.Clamp((int)level.value / 2, 1, 3);
        //var random = new Random();
        

        g_Manager = GridManager.instance;
        player = Instantiate(playerPrefab).GetComponent<Entity>();
        player.PlaceOnGrid(g_Manager.Grid, 0, 0);
        entities.Add(player);



        List<GridManager.Coord> freeCoords = g_Manager.GetAllOpenCoords();

        //for (int i = 0; i < freeCoords.Count; i++)
        //{
        //    // Debug.Log("X : " + freeCoords[i].x + " | Y : " + freeCoords[i].y);
        //    entities.Add(Instantiate(smallEnemyPrefab).GetComponent<Entity>());
        //    entities[entities.Count - 1].PlaceOnGrid(g_Manager.Grid, freeCoords[i].x, freeCoords[i].y);
        //}

        for (int i = 0; i < enemyCount; i++)
        {
            int index = Random.Range(0, enemies.Length);

            entities.Add(Instantiate(enemies[index]).GetComponent<Entity>());
            entities[entities.Count - 1].PlaceOnGrid(g_Manager.Grid, freeCoords[i].x, freeCoords[i].y);


        }
    }


    public void OnCoinCollected()
    {
        if (coinRuntimeSet.Items.Count <= 0)
        {
            allCoinsCollected.Invoke();
            Debug.Log("All Coins Collected !");
        }
    }


    private void Update()
    {
        
    }

}
