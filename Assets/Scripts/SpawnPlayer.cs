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
        int randomNumber = 0;
        Vector2 spawnPoint = spawnPoints[randomNumber].GetComponent<Transform>().position;
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity);
        randomNumber++;
    }
}
