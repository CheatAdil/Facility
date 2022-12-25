using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
	private Recorder rc;
	[SerializeField]private GameObject main;
	[SerializeField]private GameObject confirm;
	private void Start()
	{
		rc = GetComponent<Recorder>();
		confirm.SetActive(false);
	}
    public void StartGame() 
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void Settings() 
    {

    }
    public void TryDelete()
    {
        main.SetActive(false);
        confirm.SetActive(true);
    }
    public void Back()
    {
        main.SetActive(true);
        confirm.SetActive(false);
    }
    public void YesDelete()
    {
      rc.SendMessage("ClearRecordTable");
      main.SetActive(true);
      confirm.SetActive(false);
    }
}
