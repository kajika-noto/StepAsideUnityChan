using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour {

    //走るアニメーション
    private Animator myAnimator;
    private Rigidbody myRigidbody;
    private float forwardForce = 800.0f;

    //左右移動力
    private float turnForce = 500.0f;
    //ジャンプ力
    private float upForce = 500.0f;
    //左右移動範囲
    private float movableRange = 3.4f;

    //減速力
    private float coefficient = 0.95f;

    //ゲーム終了判定
    private bool isEnd = false;

    //ゲーム終了時テキスト
    private GameObject stateText;

    //スコア表示
    private GameObject scoreText;
    private int score = 0;

    //ボタン判定
    private bool isLButtonDown = false;
    private bool isRBottonDown = false;


    // Use this for initialization
    void Start () {
        this.myAnimator = GetComponent<Animator>();

        this.myAnimator.SetFloat("Speed", 1);

        this.myRigidbody = GetComponent<Rigidbody>();

        this.stateText = GameObject.Find("GameResultText");

        this.scoreText = GameObject.Find("ScoreText");
	}
	

	// Update is called once per frame
	void Update () {

        //ゲーム終了→減速
        if (this.isEnd)
        {
            this.forwardForce *= this.coefficient;
            this.turnForce *= this.coefficient;
            this.upForce *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }

        //前進運動
        this.myRigidbody.AddForce(this.transform.forward * this.forwardForce);
		

        //左右移動
        if ((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x)
        {
            this.myRigidbody.AddForce(-this.turnForce, 0, 0);
        } else if ((Input.GetKey(KeyCode.RightArrow) || this.isRBottonDown) && this.transform.position.x < this.movableRange)
        {
            this.myRigidbody.AddForce(this.turnForce, 0, 0);
        }

        //ジャンプ
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && this.transform.position.y < 0.5f)
        {
            this.myAnimator.SetBool("Jump", true);
            this.myRigidbody.AddForce(this.transform.up * this.upForce);
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        //障害物衝突時
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }

        //ゴール到達時
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }

        //コイン衝突時
        if (other.gameObject.tag == "CoinTag")
        {
            this.score += 10;
            this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt"; 
            GetComponent<ParticleSystem>().Play();
            Destroy(other.gameObject);
        }
    }

    //ジャンプ（ボタン）
    public void GetMyJumpButtonDown()
    {
        if (this.transform.position.y < 0.5f)
        {
            this.myAnimator.SetBool("Jump", true);
            this.myRigidbody.AddForce(this.transform.up * this.upForce);
        }
    }

    public void GetMyLeftButtonDown()
    {
        this.isLButtonDown = true;
    }

    public void GetMyLeftBottuonUp()
    {
        this.isLButtonDown = false;
    }

    public void GetMyRightBottonDown()
    {
        this.isRBottonDown = true;
    }

    public void GetMyRightBottonUp()
    {
        this.isRBottonDown = false;
    }

}
