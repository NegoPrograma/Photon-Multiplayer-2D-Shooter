using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public void OnButtonClick(){
        if(!SetNickname.nickname.Equals("")){
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Welcome, " + SetNickname.nickname + ". You're now connecting to our servers.");
        }
    }

    
    

}
