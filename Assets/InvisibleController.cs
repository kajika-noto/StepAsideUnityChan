using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //画面外処理
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
