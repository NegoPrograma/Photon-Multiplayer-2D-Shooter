using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManagerScript : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject canvas;
    public GameObject sceneCamera;

    void Start()
    {
        canvas.SetActive(true);
        SpawnPlayer();
    }

    public void SpawnPlayer(){
        float randomValue = Random.Range(-5,5);
        PhotonNetwork.Instantiate(playerPrefab.name,new Vector2(playerPrefab.transform.position.x*randomValue,playerPrefab.transform.position.y),Quaternion.identity,0);
        canvas.SetActive(false);
        /*
        All scenes by default have a camera.
        if they are removed, unity itself searches for a new camera! 
        since the playerprefab already has one, the camera will change automatically to the player's.
        */
        sceneCamera.SetActive(false);
    }

}
