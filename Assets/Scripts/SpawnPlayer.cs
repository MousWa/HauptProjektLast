using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject[] spawnPoints;
    public void Start()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Vector2 spawnPoint1 = spawnPoints[0].GetComponent<Transform>().position;
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint1, Quaternion.identity);
        }

        else
        {
            Vector2 spawnPoint2 = spawnPoints[1].GetComponent<Transform>().position;
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint2, Quaternion.identity);
        }
    }
}
