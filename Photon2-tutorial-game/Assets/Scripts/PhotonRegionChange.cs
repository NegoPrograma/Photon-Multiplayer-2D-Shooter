using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRegionChange : MonoBehaviourPunCallbacks
{
    
    public bool isConnected = false;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();        
    }


    void Update()
    {
        if(!isConnected){
            PhotonNetwork.ConnectUsingSettings();        
        }
        
    }


    public override void OnConnected(){
        Debug.Log("onConnected stage achieved: next one should be ConnectedToMaster");
        isConnected = true;
    }


    public override void OnConnectedToMaster(){
        Debug.Log("OnConnectedToMaster stage achieved, you're successfully connected to the server. you may enter the lobby now.");
        Debug.Log("Server hosted at: "+ PhotonNetwork.CloudRegion + ", your ping on connection:" + PhotonNetwork.GetPing());
    }

    public override void OnDisconnected(DisconnectCause cause){

        Debug.Log("You're disconnected from server, because of: " +cause);
        PhotonNetwork.ConnectToRegion("eu");
        isConnected = false;
    
    }

}