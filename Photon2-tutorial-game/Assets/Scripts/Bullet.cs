/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BulletController : MonoBehaviourPun
{

    public Rigidbody2D rigid;
    public float bulletSpeed;
    public float bulletLifeTime;
    public float bulletTimeCount;
    public float bulletDamage;
    public Vector2 bulletDirection;
    public Vector2 initialPos;

    //public PhotonView bulletView;


    public void SetDirection(Vector2 bulletDirection,Vector2 initialPos){
        this.bulletDirection = bulletDirection;
        this.initialPos = initialPos;

    }
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
      //  bulletView = gameObject.GetComponent<PhotonView>();
        bulletSpeed = 100f;
        bulletLifeTime = 5f;
        bulletDamage = 10;
        moveBullet();
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletTimeCount > bulletLifeTime){
            Destroy(gameObject);
        }
        bulletTimeCount += Time.deltaTime;            
    }

    void OnTriggerEnter2D(Collider2D collision){
        //check if collided with a player prefab that isn't yours
        PlayerController collidedPlayer = collision.gameObject.GetComponent<PlayerController>();
        if(collidedPlayer != null){
            if(!collidedPlayer.playerView.IsMine){
                Debug.Log("Triggered!");
                collidedPlayer.takeDamage(-bulletDamage);
                Destroy(gameObject);
            }
        }
    }

    public void moveBullet(){

        rigid.AddForce(transform.up*bulletSpeed,ForceMode2D.Force);

    }
}
*/