using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject[] spawnPoints;
    public int count;
    public Text countDisplay;
    private void Start()
    {
        StartCoroutine(CountdownToStart());

    }

    IEnumerator CountdownToStart()
    {
        while (count > 0)
        {
            countDisplay.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        countDisplay.text = "Go";

        
        yield return new WaitForSeconds(1f);


        countDisplay.gameObject.SetActive(false);

        respon();
    }
    private void respon() {


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
