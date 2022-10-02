using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    enum GameState
    {
        Starting,
        Game,
        Ended,
        Lose,
        Win
    }

    private GameState state;

    [Header("Camera")]
    [SerializeField] private CameraFollow follow;

    [Header("Settings")]
    [SerializeField] private float maxTime = 10;
    [SerializeField] private float grenadeRange = 10;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float grenadeForce = 100;
    [SerializeField] private bool simultanious = false; 

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject victumPrefab;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject deathPrefab;

    [Header("UI")]
    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject winScreenObject;
    [SerializeField] private GameObject loseScreenObject;

    private GameObject playerSpawn;
    private GameObject[] victumSpawns;
    private GameObject[] grenadeSpawns;

    private GameObject player;
    private List<GameObject> victums;

    private List<GameObject> grenades;
    private int grenadesSpawned = 0;

    private float timer;

    private void Start()
    {
        grenades = new List<GameObject>();

        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        victumSpawns = GameObject.FindGameObjectsWithTag("VictumSpawn");
        grenadeSpawns = GameObject.FindGameObjectsWithTag("GrenadeSpawn");

        //System.Array.Sort(grenadeSpawns, new System.Comparison<GameObject>((i, j) => int.Parse(i.name).CompareTo(int.Parse(j.name))));

        grenadeSpawns = grenadeSpawns.OrderBy(grenade => grenade.name).ToArray();

        player = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);

        victums = new List<GameObject>();
        foreach (var spawn in victumSpawns)
        {
            var v = Instantiate(victumPrefab, spawn.transform.position, Quaternion.identity);
            victums.Add(v);
        }

        state = GameState.Starting;
    }

    void Update()
    {
        switch(state)
        {
            case GameState.Starting:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.GetComponent<PlayerMovement>().CanMove = true;
                    timer = maxTime;

                    SpawnGrenade(grenadeSpawns[0].transform);

                    if(simultanious)
                    {
                        for (int i = 1; i < grenadeSpawns.Length; i++)
                        {
                            SpawnGrenade(grenadeSpawns[i].transform);
                        }
                    }

                    follow.SetTarget(player.transform);
                    startText.SetActive(false);
                    state = GameState.Game;
                }
                break;

            case GameState.Game:
                Game();
                break;
        }
    }

    void Game()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = maxTime;
            ExplodeGrenade();

            if (state == GameState.Lose)
            {
                EndGame();
                return;
            }

            if (grenadesSpawned >= grenadeSpawns.Length)
                EndGame();
            else
                SpawnGrenade(grenadeSpawns[grenadesSpawned].transform);
        }
    }
    void SpawnGrenade(Transform spawn)
    {
        var rot = Random.Range(-360, 360);
        grenades.Add(Instantiate(grenadePrefab, spawn.position, Quaternion.Euler(0, 0, rot)));

        grenadesSpawned++;
    }

    void ExplodeGrenade()
    {
        for (int i = grenades.Count - 1; i >= 0; i--)
        {
            var grenade = grenades[i];
            var gPos = grenade.transform.position;
            Instantiate(explosionPrefab, grenade.transform.position, grenade.transform.rotation);

            ExplodeVictum(gPos);
            ExplodeKnockback(gPos);

            if (player != null)
                ExplodePlayer(gPos);

            Destroy(grenade);
            grenades.RemoveAt(i);
        }
    }

    void ExplodePlayer(Vector3 gPos)
    {
        var pos = player.transform.position;
        var distance = Vector2.Distance(pos, gPos);

        if (distance > grenadeRange) return;

        RaycastHit2D hit = Physics2D.Raycast(gPos, pos - gPos, distance, layerMask);
        if (hit.transform != player.transform) return;

        Instantiate(deathPrefab, pos, Quaternion.identity);
        
        Destroy(player);
        player = null;
        
        state = GameState.Lose;
    }

    void ExplodeVictum(Vector3 gPos)
    {
        for (int i = victums.Count - 1; i >= 0; i--)
        {
            var victum = victums[i];
            var pos = victum.transform.position;
            var distance = Vector2.Distance(pos, gPos);

            if (distance > grenadeRange) continue;

            RaycastHit2D hit = Physics2D.Raycast(gPos, pos - gPos, distance, layerMask);
            if (hit.transform != victum.transform) continue;

            Instantiate(deathPrefab, pos, Quaternion.identity);

            victums.RemoveAt(i);
            Destroy(victum);
        }
    }

    void ExplodeKnockback(Vector3 gPos)
    {
        var rigidbodies = FindObjectsOfType<Rigidbody2D>();
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            var rb = rigidbodies[i];
            var pos = rb.transform.position;

            var distance = Vector2.Distance(pos, gPos);
            if (distance > grenadeRange) continue;

            RaycastHit2D hit = Physics2D.Raycast(gPos, pos - gPos, distance, layerMask);
            if (hit.transform != rb.transform) continue;

            var t = 1 - (distance / grenadeRange);
            rb.AddForce((pos - gPos).normalized * grenadeForce * t);
        }
    }

    void EndGame()
    {
        if (state != GameState.Lose)
        {
            if (victums.Count <= 0)
                state = GameState.Lose;
            else
                state = GameState.Win;
        }

        if (state == GameState.Lose)
        {
            float playerSurvived = (player == null) ? 0 : 1;
            var survivalPercent = ((float)((1 - playerSurvived) + (victumSpawns.Length - victums.Count)) / (victumSpawns.Length + 1)) * 100;

            loseScreenObject.SetActive(true);
            loseScreenObject.GetComponent<LoseScreen>().SetRate(survivalPercent);
        }
        else if (state == GameState.Win)
        {
            var survivalPercent = ((float)(1 + victums.Count) / (victumSpawns.Length + 1)) * 100;

            winScreenObject.SetActive(true);
            winScreenObject.GetComponent<WinScreen>().SetRate(survivalPercent);
        }
    }
}
