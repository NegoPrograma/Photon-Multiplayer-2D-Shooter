using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public void OnButtonClick(){
        if(!PhotonNetwork.LocalPlayer.NickName.Equals("")){
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Welcome, " + PhotonNetwork.LocalPlayer.NickName + ". You're now connecting to our servers.");
        }
    }

    
    

}
