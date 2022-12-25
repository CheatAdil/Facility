using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
    [SerializeField] private int BtnValue;
    [SerializeField] private string BtnMode;
    private CodePad codePad;
    private void Start()
    {
    	codePad = GetComponentInParent<CodePad>();
    }
    private void Press()
    {
    	if (codePad != null)
    	{
    		switch(BtnMode)
    		{
    			case "digit":
    			codePad.EnterDigit(BtnValue);
    			break;
    			case "delete":
    			codePad.DeleteDigit();
    			break;
    			case "submit":
    			codePad.Submit();
    			break;

    		}
    	}
    }
}
