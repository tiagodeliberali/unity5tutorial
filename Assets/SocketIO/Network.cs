using System.Collections.Generic;
using Assets.Scripts;
using SocketIO;
using UnityEngine;

public class Network : MonoBehaviour
{
    static SocketIOComponent socket;

    public GameObject playerPrefab;

    Dictionary<string, GameObject> playerList = new Dictionary<string, GameObject>();

	void Start ()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("session", OnSessionStarted);
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("unspawn", OnUnspawned);
        socket.On("move", OnMovement);
    }

    private void OnSessionStarted(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);

        Debug.Log("Connected sessionId: " + sessionId);

        var player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<LocalCharacterMovement>().sessionId = sessionId;
    }

    private void OnMovement(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);

        playerList[sessionId].GetComponent<NetworkCharacterMovement>().OnMovement(obj);
    }

    private void OnUnspawned(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);

        Debug.Log("Unspawned sessionid: " + sessionId);

        if (playerList.ContainsKey(sessionId))
        {
            Destroy(playerList[sessionId]);
        }
    }

    private void OnSpawned(SocketIOEvent obj)
    {
        string sessionId = GetSessionId(obj);

        Debug.Log("Spawned sessionid: " + sessionId);

        var player = GameObject.FindGameObjectWithTag("Player");

        var playerRigidBody = player.GetComponent<Rigidbody>();

        var gameObject = 
            Instantiate(playerPrefab, new Vector3(120 + playerList.Count, 0, 130 + playerList.Count), playerRigidBody.rotation);

        playerList.Add(sessionId, gameObject);
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
