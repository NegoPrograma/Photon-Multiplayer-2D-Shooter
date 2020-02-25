using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
public class ChatManager : MonoBehaviourPun,IPunObservable
{

    public PhotonView playerView;
    public GameObject bubbleSpeech;
    public Text chatText;
    public InputField chatInput;
    public PlayerController player;
    private  bool disableSending;

    void Start()
    {
        chatInput = GameObject.Find("ChatField").GetComponent<InputField>();
        bubbleSpeech.SetActive(false);
        chatText.text = "";

    }

    void Update()
    {
        if(playerView.IsMine){
            if(chatInput.isFocused){
                player.canInput = false;
            }
            else {
                if(player.isAlive)
                    player.canInput = true;
            }
            if(!disableSending && chatInput.isFocused){
                if(chatInput.text  != "" && chatInput.text.Length > 1 &&  Input.GetKeyDown(KeyCode.RightControl)){
                    //
                    playerView.RPC("SendMessage",RpcTarget.AllBuffered,chatInput.text);
                    chatInput.text = "";
                    disableSending = true;
                    
                }
            }
        }
        
    }

    [PunRPC]
    void SendMessage(string msg){
        bubbleSpeech.SetActive(true);
        chatText.text = msg;
        StartCoroutine("hideBubbleSpeech");
    }

//delay the next msg
    IEnumerator hideBubbleSpeech(){
        yield return  new WaitForSeconds(3);
        bubbleSpeech.SetActive(false);
        disableSending = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(bubbleSpeech.activeSelf);
        } 
        else if(stream.IsReading){
            bubbleSpeech.SetActive((bool) stream.ReceiveNext());
        }
    }


}
