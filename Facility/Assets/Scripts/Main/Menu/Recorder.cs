using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using UnityEngine.UI;

public class Recorder : MonoBehaviour
{
	private string FetchedData;
	private Text RecText;
	/////
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    /////
	private void Start()
	{
		RecText = GameObject.Find("RecText").GetComponent<Text>();
		ConnectDataBase();
		SettleTable();
		GetRecord();
	}
    private void ConnectDataBase()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "recordTable";
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
        Debug.Log("DB connected");
    }
    private void SettleTable()
    {
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS recordTable (id INTEGER PRIMARY KEY, time FLOAT, act FLOAT, enemies INTEGER, won BOOL)";
        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();
    }
    private void CloseDataBase()
    {
        dbcon.Close();
        Debug.Log("DB connection closed");
    }
     private void ClearRecordTable()
    {
        ConnectDataBase();
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "DELETE FROM recordTable;";
        cmnd.ExecuteNonQuery();
        Debug.Log("DB cleared!");
        CloseDataBase();
        RecText.text = "";
    }
    private void DELETETABLE()
    {
        ConnectDataBase();
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "DROP TABLE recordTable;";
        cmnd.ExecuteNonQuery();
        Debug.Log("DB ELIMINATED!");
        CloseDataBase();
    }
    private void GetRecord()
    {
    	int n = 0;
        string report = "";
        List<float> times = new List<float>();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT time FROM recordTable WHERE won = 1";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
          n++;
          report += "Fetched : " + n + " : " + reader[0] + "\n";
          times.Add(Convert.ToSingle(reader[0]));
        }
        Debug.Log(report);
        if (times.Count != 0) RecText.text ="Best time " + GetBestTime(times).ToString();
        else RecText.text = "";
        CloseDataBase();
    }
    private float GetBestTime(List<float> arr)
    {
    	float min = arr[0];
    	for (int i = 0; i < arr.Count; i ++)
    	{
    		if (min > arr[i]) min = arr[i];
    	}
    	return min;
    }
}


