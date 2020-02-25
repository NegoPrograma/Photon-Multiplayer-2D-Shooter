using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerScript : MonoBehaviour{

    public GameObject playerPrefab;
    public GameObject exitCanvas;
    public GameObject sceneCamera;

    public int timesClicked = -1;

    [Header("Respawn")]
    public Text respawnText;
    public GameObject respawnScreenEffect;
    public float respawnCountdown = 5f;
    public bool startCountdown;
    public static GameManagerScript instance = null;
    public GameObject localPlayer;


    void Awake(){
        instance = this;
        respawnScreenEffect.SetActive(false);
        exitCanvas.SetActive(false);
    }
    void Start()
    {
        
        SpawnPlayer();
    }

    void Update(){
        if(startCountdown)
            StartRespawn();
        ToggleExitButton();
        }

    public void ToggleExitButton(){
         if(Input.GetKeyDown(KeyCode.Escape)){
            timesClicked+=1;
            if(timesClicked%2 == 0)
                exitCanvas.SetActive(true);
            else
                exitCanvas.SetActive(false);
        }
    }
   public void StartRespawn(){
        respawnCountdown -=Time.deltaTime;
        respawnText.text = "Respawning in: " + respawnCountdown.ToString("0");

        if(respawnCountdown <=0){
            respawnScreenEffect.SetActive(false);
            startCountdown=false;
            localPlayer.GetComponent<PlayerController>().playerView.RPC("RevivePlayer",RpcTarget.AllBuffered);
        }
    }

   public void EnableRespawn(){
        respawnCountdown = 5f;
        startCountdown = true;
        respawnScreenEffect.SetActive(true);
    }


    public void SpawnPlayer(){
        float randomValue = Random.Range(-5,5);
        PhotonNetwork.Instantiate(playerPrefab.name,new Vector2(playerPrefab.transform.position.x*randomValue,playerPrefab.transform.position.y),Quaternion.identity,0);
        /*
        All scenes by default have a camera.
        if they are removed, unity itself searches for a new camera! 
        since the playerprefab already has one, the camera will change automatically to the player's.
        */
        sceneCamera.SetActive(false);
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel(0);
        
    }


}
