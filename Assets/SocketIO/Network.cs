using System;
using System.Collections.Generic;
using Assets.Scripts;
using SocketIO;
using UnityEngine;

public class Network : MonoBehaviour
{
    static SocketIOComponent socket;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    Dictionary<string, GameObject> playerList = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> enemyList = new Dictionary<string, GameObject>();

    void Start ()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("mothership", OnMothershipEmit);
        socket.On("session", OnSessionStarted);
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("unspawn", OnUnspawned);
        socket.On("move", OnPlayerMovement);
        socket.On("enemy", OnEnemyMovement);
    }

    private void OnSessionStarted(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);
        int energyCount = int.Parse(obj.data["q"].ToString());

        Debug.Log("Connected sessionId: " + sessionId);

        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<LocalCharacterMovement>().sessionId = sessionId;

        var enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemy.GetComponent<EnemyMovement>().sessionId = sessionId;

        var mothership = GameObject.FindGameObjectWithTag("Mothership");
        mothership.GetComponent<MotherShip>().collectedEnergy = energyCount;
    }

    private void OnMothershipEmit(SocketIOEvent obj)
    {
        var quantity = int.Parse(obj.data["q"].ToString());

        var mothership = GameObject.FindGameObjectWithTag("Mothership");
        mothership.GetComponent<MotherShip>().collectedEnergy += quantity;
    }

    private void OnPlayerMovement(SocketIOEvent obj)
    {
        var movement = new EntityMovement(obj);
        Debug.Log("NotEqualMovement: " + movement.ToString());

        string sessionId = GetSessionId(obj);

        playerList[sessionId].GetComponent<NetworkCharacterMovement>().OnMovement(obj);
    }

    private void OnEnemyMovement(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);

        enemyList[sessionId].GetComponent<NetworkEnemyMovement>().OnMovement(obj);
    }

    private void OnUnspawned(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);

        Debug.Log("Unspawned sessionid: " + sessionId);

        if (playerList.ContainsKey(sessionId))
        {
            Destroy(playerList[sessionId]);
            Destroy(enemyList[sessionId]);

            playerList.Remove(sessionId);
            enemyList.Remove(sessionId);
        }
    }

    private void OnSpawned(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);

        Debug.Log("Spawned sessionid: " + sessionId);

        var newPlayer = 
            Instantiate(playerPrefab, new Vector3(120 + playerList.Count, 0, 130 + playerList.Count), Quaternion.Euler(0, 0, 0));

        var newEnemy =
            Instantiate(enemyPrefab, new Vector3(150 + enemyList.Count, 0, 90 + enemyList.Count), Quaternion.Euler(0, 0, 0));

        playerList.Add(sessionId, newPlayer);
        enemyList.Add(sessionId, newEnemy);
    }

    void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("Connected");
    }

    private string GetSessionId(SocketIOEvent obj)
    {
        return obj.data["s"].ToString().Replace("\"", string.Empty);
    }
}
