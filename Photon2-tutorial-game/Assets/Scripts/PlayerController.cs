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
    public Image playerHealthBar;

    public float playerMaxHealth;
    public float playerCurrentHealth;
    public GameObject playerBullet;
    public GameObject bulletSpawn;
    public PhotonView playerView;
    public Vector2 bulletDirection;
    public Text playerName;

    public bool isAlive;
    public bool isGrounded = false;
    public float jumpForce;
    public GameObject playerCanvas;

    public bool canInput;
    
    void Awake()
    {
        bulletDirection = new Vector2(1,0);
        jumpForce=800f;
        isAlive = true;
        canInput = true;
        playerMaxHealth = 100;
        playerCurrentHealth = playerMaxHealth;
        if(playerView.IsMine){
            GameManagerScript.instance.localPlayer = this.gameObject;
            playerName.text = PhotonNetwork.NickName;
            playerName.color = Color.cyan;
            playerCamera.SetActive(true);
        }else{
            playerName.text = playerView.Owner.NickName;
            playerName.color = Color.red;
            playerCamera.SetActive(false);
        }
    }



    private void PlayerAtZeroHP(){
        if(playerView.IsMine && playerCurrentHealth <= 0){
            GameManagerScript.instance.EnableRespawn();
            playerView.RPC("KillPlayer",RpcTarget.AllBuffered);
        }
    }

    [PunRPC]

    public void KillPlayer(){
        isAlive = false;
        playerSprite.enabled = false;
        rigid.gravityScale = 0;
        gameObject.GetComponent<Collider2D>().enabled = false;
        playerCanvas.SetActive(false);
        canInput = false;
    }

    [PunRPC]
    public void RevivePlayer(){
        isAlive = true;
        playerSprite.enabled = true;
        rigid.gravityScale = 1;
        gameObject.GetComponent<Collider2D>().enabled = true;
        playerCanvas.SetActive(true);
        canInput = true;
        playerHealthBar.fillAmount = 1;
        playerCurrentHealth = 100;
    }
    
    [PunRPC]
    public void TakeDamage(float damage){
        playerHealthBar.fillAmount -= damage/100;
        playerCurrentHealth -= damage;
        PlayerAtZeroHP();
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
        if(playerView.IsMine && canInput){
            PlayerShoot();
            PlayerMove();
            PlayerJump();
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
        bool isPressingMovementKey = Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.RightArrow);

        if(isPressingMovementKey)
            playerAnimation.SetBool("isMoving",true);
        if(rightMovementPressed){
            
            photonView.RPC("PlayerFlipRPC_Right",RpcTarget.AllBuffered,null);
            bulletDirection = new Vector2(1,0);
        } 
        else if(rightMovementUnpressed){
                playerAnimation.SetBool("isMoving",false);
        }
        
        
        if(leftMovementPressed){
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

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Ground"){
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col){
        if(col.gameObject.tag == "Ground"){
            isGrounded = false;
        }
    }

    void PlayerJump(){
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded){
             rigid.AddForce(new Vector2(0,jumpForce),ForceMode2D.Force);
             playerAnimation.SetBool("isJumping",true);
        } else {
             playerAnimation.SetBool("isJumping",false);
        }
    }
}
