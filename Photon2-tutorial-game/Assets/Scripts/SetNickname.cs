using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class SetNickname : MonoBehaviour
{
    public static string nickname = "";
    public Text nicknameInputFieldTextComponent;

    void Awake(){
        if(nickname.Length > 0 || PhotonNetwork.NickName.Equals(""))
             PhotonNetwork.NickName=nickname;
    }
    public void OnButtonClick(){
        nickname= nicknameInputFieldTextComponent.text;
        PhotonNetwork.LocalPlayer.NickName = nickname;
    }
    
}
