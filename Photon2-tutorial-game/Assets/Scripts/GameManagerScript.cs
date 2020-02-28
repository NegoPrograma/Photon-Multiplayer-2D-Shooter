using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerScript : MonoBehaviourPunCallbacks{
    public GameObject playerPrefab;

    public GameObject connectedPlayersCanvas;
    public ConnectedPlayersScript connectedPlayers;
    public GameObject exitCanvas;
    public GameObject sceneCamera;

    public GameObject feedbox;
    public GameObject feedText_Prefab;

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
        connectedPlayers.AddLocalPlayer();
        connectedPlayers.photonView.RPC("UpdatePlayerList",RpcTarget.AllBuffered,PhotonNetwork.NickName);
        SpawnPlayer();
    }

    void Update(){
        if(startCountdown)
            StartRespawn();
        ToggleExitButton();

        if(Input.GetKey(KeyCode.Tab)){
            connectedPlayersCanvas.SetActive(true);
        }else{
            connectedPlayersCanvas.SetActive(false);
        }
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


    //PunCallbacks

    public override void OnPlayerEnteredRoom(Player newPlayer){
        GameObject feed = Instantiate(feedText_Prefab, new Vector2(0f,0f),Quaternion.identity);
        feed.transform.SetParent(feedbox.transform);
        feed.GetComponent<Text>().text = newPlayer.NickName + " has joined the game";
        Destroy(feed, 3);
    }

    [PunRPC]
    public void PlayerGotKilledBy(string victim, string assassin){
        GameObject feed = Instantiate(feedText_Prefab, new Vector2(0f,0f),Quaternion.identity);
        feed.transform.SetParent(feedbox.transform);
        feed.GetComponent<Text>().text = victim + " got killed by " + assassin;
        Destroy(feed, 3);
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer){
        connectedPlayers.RemovePlayerList(otherPlayer.NickName);
        GameObject feed = Instantiate(feedText_Prefab, new Vector2(0f,0f),Quaternion.identity);
        feed.transform.SetParent(feedbox.transform);
        feed.GetComponent<Text>().text = otherPlayer.NickName + " has left the game";
        Destroy(feed, 3);
    
    }



}