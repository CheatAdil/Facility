using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
	private bool CanEscape;
	private GameManager gm;
	private void Start()
	{
		CanEscape = false;
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
    private void EndGame()
    {
        CanEscape = true;
    }
    private void OnTriggerEnter(Collider other)
    {
    	if (other.gameObject.tag == "Player")
    	{    		    	
    	    if (CanEscape)
    	    {
              gm.EndGame(true);
    	    }else 
    	    {
    		  Debug.LogError("Fucking cheater");
    	    }
        }
    }
}
