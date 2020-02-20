using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameModeController : MonoBehaviourPunCallbacks
{

    public static Hashtable gameMode = new Hashtable();
    public static byte playerLimitPerRoom = 2; 
    public static string gameModeKey = "gamemode";


    public void EnterPvPMatch(){
        gameMode.Add(gameModeKey,"PvP");
        PhotonNetwork.JoinRandomRoom(gameMode,playerLimitPerRoom);
    }

    
    public void EnterPvAIMatch(){
        gameMode.Add(gameModeKey,"PvAI");
        PhotonNetwork.JoinRandomRoom(gameMode,playerLimitPerRoom);
    }

    public void EnterRandomRoom(){

        string[] gameModeList = new string[]{
            "PvP",
            "PvAI"
        };
        string randomGameMode = gameModeList[Random.Range(0,gameModeList.Length-1)];
        gameMode.Add(gameModeKey,randomGameMode);
        PhotonNetwork.JoinRandomRoom(gameMode,playerLimitPerRoom);
    }
}
