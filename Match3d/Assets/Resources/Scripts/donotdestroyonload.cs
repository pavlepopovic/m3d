using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class donotdestroyonload : MonoBehaviour {
	public  static donotdestroyonload d=null;
//	static public donotdestroyonload d{
//		get{ 
//			if(d==null){
//				d = UnityEngine.Object.FindObjectOfType (typeof(donotdestroyonload)) as donotdestroyonload;
//				if (d == null) {
//					
//				}
//			}
//		}
//	}
	void Awake(){
		
		if (d == null) {
			d = this.gameObject.GetComponent<donotdestroyonload>();
			DontDestroyOnLoad (gameObject);
		} else {
		//	gameObject.transform.GetComponent<admobads> ().OnDestroybanner ();
			if (this != d) {
				print ("Occur");
				Destroy (this.gameObject);
			}
		}
	}
	// Use this for initialization
	
}
