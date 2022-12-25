using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodePad : MonoBehaviour
{
    private Stack<int> DisplayBuffer;
    private int password;
    private TextMeshPro dispText;
    private bool blocked;
    private GameObject VaultDoor;
    private void Start()
    {
    	DisplayBuffer = new Stack<int>();
    	dispText = GameObject.Find("dispText").GetComponent<TextMeshPro>();
        blocked = false;
        VaultDoor = GameObject.Find("Vault Door");
        SetPassword(1234);
    }
    private void SetPassword(int pass = 1234)
    {
       password = pass;
    }
    public void EnterDigit(int digit = 0) // must be from 0 to 9
    {
       if ((digit <= 9) && (digit >= 0) && DisplayBuffer.Count < 4) // anyway we shall check it just in case
       {
           DisplayBuffer.Push(digit);
           RefreshDisplay();
       }
    }
    private void RefreshDisplay()
    {
        string textToOutput = "";
        if (!blocked)
        {
        int[] arr = new int[DisplayBuffer.Count];
        DisplayBuffer.CopyTo(arr, 0);
        for (int i = arr.Length - 1; i >= 0; i --)
        {
        	textToOutput += arr[i].ToString();
        }
        if (arr.Length < 4)
        {
        	int dif = 4 - arr.Length;
        	for (int i = 0; i < dif; i ++)
        	{
        		textToOutput += "_";
        	}
        }
        }
        else 
        {
            textToOutput = "_ok_";
        }
        dispText.text = textToOutput;
    }
    public void DeleteDigit()
    {
    	if (DisplayBuffer.Count > 0)
    	{
    		DisplayBuffer.Pop();
    	    RefreshDisplay();
    	}
    }
    public void Submit()
    {
        if (!blocked)
    {
        string EnteredCode = dispText.text;
        if (EnteredCode == password.ToString())
        {
            Debug.Log("Correct");
            blocked = true;
            RefreshDisplay();
            VaultDoor.SendMessage("CodeOpen");
        }
        else 
        {
            Debug.Log("incorrect");
            RefreshDisplay();
        }
    }
    }
}
