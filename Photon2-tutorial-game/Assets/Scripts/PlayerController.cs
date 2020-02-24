using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{

    public GameObject playerCamera;
    public Rigidbody2D rigid;
    public SpriteRenderer playerSprite;
    public Animator playerAnimation;
    public float playerSpeed = 8;

    public GameObject playerBullet;
    public GameObject bulletSpawn;
    public PhotonView playerView;
    public Vector2 bulletDirection;
    public Text playerName;
    
    void Awake()
    {
        if(playerView.IsMine){
            playerName.text = PhotonNetwork.NickName;
            playerName.color = Color.cyan;
            playerCamera.SetActive(true);
        }else{
            playerName.text = playerView.Owner.NickName;
            playerName.color = Color.red;
            playerCamera.SetActive(false);
        }
    }
    
    


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(playerName.text);
        } 
        else if(stream.IsReading){
            string nick = (string) stream.ReceiveNext();
            if(playerName.text.Equals(nick)){
                this.playerName.text = PhotonNetwork.NickName;
                this.playerName.color = Color.cyan;
            } 
            if(!playerName.text.Equals(nick)){
                playerName.text = nick;
                playerName.color = Color.red;
            }
        }
    }
    void Update()
    {
        if(playerView.IsMine){
            PlayerShoot();
            PlayerMove();
           //playerView.RPC("PlayerShoot",RpcTarget.All,null);
           
        }
    }

    public void PlayerMove(){
        if(playerAnimation.GetBool("isShooting") == false){
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"),0);
            transform.position += movement*playerSpeed*Time.deltaTime;
            PlayerFlip();
        }
    }

    public void PlayerFlip(){
        bool rightMovementPressed =Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool rightMovementUnpressed =Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow);
        bool leftMovementPressed =Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        bool leftMovementUnpressed =Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow);
       
       
        if(rightMovementPressed){
            playerAnimation.SetBool("isMoving",true);
            photonView.RPC("PlayerFlipRPC_Right",RpcTarget.AllBuffered,null);
            bulletDirection = new Vector2(1,0);
        } 
        else if(rightMovementUnpressed){
                playerAnimation.SetBool("isMoving",false);
        }
        
        
        if(leftMovementPressed){
            playerAnimation.SetBool("isMoving",true);
            photonView.RPC("PlayerFlipRPC_Left",RpcTarget.AllBuffered,null);
            bulletDirection = new Vector2(-1,0);
        }
        else if(leftMovementUnpressed){
                playerAnimation.SetBool("isMoving",false);
        }
    }


    [PunRPC]
    private void PlayerFlipRPC_Right(){
            playerSprite.flipX = false;
    }

    [PunRPC]
    
    private void PlayerFlipRPC_Left(){
            playerSprite.flipX = true;

    }


    private void PlayerShoot(){
            if(Input.GetKeyDown(KeyCode.O) && canShoot()){
                this.playerAnimation.SetBool("isShooting",true);
                GameObject bullet = PhotonNetwork.Instantiate(this.playerBullet.name,this.bulletSpawn.transform.position,Quaternion.identity,0,setBulletDirection());
            }else if(Input.GetKeyUp(KeyCode.O)){
                this.playerAnimation.SetBool("isShooting",false);
            }
    }

    private object[] setBulletDirection(){
        object[] dir = new object[1];
        dir[0]= this.bulletDirection;
        return dir;
    }
    private bool canShoot(){
        if(playerAnimation.GetBool("isMoving") == false)
            return true;
        return false;            
    }
}
