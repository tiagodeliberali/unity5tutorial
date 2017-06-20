using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;

namespace Assets.Scripts
{
    public class NetworkManager : MonoBehaviour
    {
        static SocketIOController socket;

        public GameObject playerPrefab;
        public GameObject enemyPrefab;

        Dictionary<string, GameObject> playerList = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> enemyList = new Dictionary<string, GameObject>();

        void Start()
        {
            socket = GetComponent<SocketIOController>();

            socket.On("mothership", OnMothershipEmit);
            socket.On("session", OnSessionStarted);
            socket.On("open", OnConnected);
            socket.On("spawn", OnSpawned);
            socket.On("unspawn", OnUnspawned);
            socket.On("move", OnPlayerMovement);
            socket.On("enemy", OnEnemyMovement);

            socket.Connect();
        }

        private void OnSessionStarted(SocketIOEvent obj)
        {
            Debug.Log("OnSessionStarted: " + obj.data);

            var data = JsonUtility.FromJson<NetworkBasicObject>(obj.data);

            Debug.Log("Connected sessionId: " + data.SessionId);

            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<LocalCharacterMovement>().sessionId = data.SessionId;

            var enemy = GameObject.FindGameObjectWithTag("Enemy");
            enemy.GetComponent<LocalEnemyMovement>().sessionId = data.SessionId;

            var mothership = GameObject.FindGameObjectWithTag("Mothership");
            mothership.GetComponent<MotherShip>().collectedEnergy = data.EnergyCount;
        }

        private void OnMothershipEmit(SocketIOEvent obj)
        {
            var data = JsonUtility.FromJson<NetworkBasicObject>(obj.data);

            var mothership = GameObject.FindGameObjectWithTag("Mothership");
            mothership.GetComponent<MotherShip>().collectedEnergy += data.EnergyCount;
        }

        private void OnPlayerMovement(SocketIOEvent obj)
        {
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

            newEnemy.GetComponent<NetworkEnemyMovement>().player = newPlayer;

            playerList.Add(sessionId, newPlayer);
            enemyList.Add(sessionId, newEnemy);
        }

        void OnConnected(SocketIOEvent obj)
        {
            Debug.Log("Connected");
        }

        private string GetSessionId(SocketIOEvent obj)
        {
            return JsonUtility.FromJson<NetworkBasicObject>(obj.data).SessionId;
        }
    }
}
