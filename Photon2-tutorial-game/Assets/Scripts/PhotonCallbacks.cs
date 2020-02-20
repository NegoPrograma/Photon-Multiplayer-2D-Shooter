﻿using UnityEngine;
using System.Collections;
using String = System.String;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonCallbacks : MonoBehaviourPunCallbacks
{

    public ScreenSwitch screenSwitch;
    public override void OnConnected(){
        Debug.Log("onConnected stage achieved: next one should be ConnectedToMaster");
    }

    public override void OnDisconnected(DisconnectCause cause){
        Debug.Log("Error! "+ cause);
        Debug.Log("Trying to return you to lobby.");
        PhotonNetwork.Reconnect();
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnectedToMaster(){
        Debug.Log("OnConnectedToMaster stage achieved, you're successfully connected to the server. you may enter the lobby now.");
        Debug.Log("Server hosted at: "+ PhotonNetwork.CloudRegion + ", your ping on connection:" + PhotonNetwork.GetPing());
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby(){
        Debug.Log("You are now at the lobby.");
        screenSwitch.toggleScreenState();

    }

    public override void OnJoinedRoom(){
        Debug.Log("You're now inside a room.");
        //LoadLevel é uma função que carrega uma scene, sendo seu parâmetro a index da scene no build settings 
        PhotonNetwork.LoadLevel(1);
    }


//esse método verifica a existência de novas salas a cada 5 segundos e só executa evidentimente se houverem novas salas
    public override void OnRoomListUpdate(System.Collections.Generic.List<RoomInfo> roomList){

    }

    public override void OnJoinRandomFailed(short returnCode, string message){
        string randomGeneratedRoomName ="Room"+Random.Range(1,5000).ToString()+Random.Range(1,5000).ToString();
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        PhotonNetwork.CreateRoom(randomGeneratedRoomName,options);
    }


}