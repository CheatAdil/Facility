using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class statistics : MonoBehaviour
{
	private float ElapsedTime;
	private float ItemElapsedTime;
    private int ItemsUsed;
    private int ItemsFound;
    private int EnemiesCount;
    private GameObject Player;
    private bool running;
    private int l;
    private bool[,] itemTable;
    private float[,] itemTimeTable;
    private bool hasWon;

    /////
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    /////
    private void Awake()
    {
        running = false;
        ItemsUsed = 0;
        ItemsFound = 0;
        ElapsedTime = 0;
        ItemElapsedTime = 0;
        hasWon = false;
    }
    private void Start()
    {   
         /// ClearRecordTable(); /// clear database for debug purposes
         ///  DELETETABLE();     /// delete database for debug purposes
    }
    public void StartRecording(int enemies) 
    {
        Debug.Log("Starting Statistics...");
        Player = GameObject.Find("Player");       
        if (Player != null)    
        {
            running = true;
            Debug.Log("Running Statistics...");
        }
        EnemiesCount = enemies;
    }
    private void Update()
    {
        if (running) 
        {
            ElapsedTime += Time.deltaTime;
            ItemElapsedTime += Time.deltaTime;
        }
    }
    public void StopRecord(bool won) 
    {
        running = false;
        hasWon = won;
        ReportRecord();
    }
    private void ReportRecord()
    {
        SummarizeTable(itemTable);
        ConnectDataBase();
        SettleTable();
        RecordData(GetNewId(), ElapsedTime, AverageActivityTime(itemTimeTable), EnemiesCount, hasWon);
        FetchData();
    }
    public void InitializeItemTable(int count) 
    {
        itemTable = new bool[count, 2];
        itemTimeTable = new float[count , 2];
        Debug.Log(itemTable.Length / 2 + " objects written");
        l = count;
    }
    public void WriteItem(InteractableObject io, int mode) /// 0 for found items, 1 for used items 
    {
        if (mode != 0 && mode != 1) 
        {
            Debug.LogError("CI mode error");
            return; 
        }
        int id = io.id;
        if (itemTable[id, mode] == false)
        {
            itemTable[id, mode] = true;
            itemTimeTable[id, mode] = ItemElapsedTime;
            Debug.Log("Item: [" + id + ", " + mode + "]::" + itemTimeTable[id, mode]);
        }
        else itemTable[id, mode] = true;
    }
    private void SummarizeTable(bool[,] arr) 
    {
        for (int i = 0; i < l; i++) 
        {
            for (int j = 0; j < 2; j++) 
            {
                if (arr[i, j] == true) 
                {
                    switch (j) 
                    {
                        case 0: ItemsFound++; break;
                        case 1: ItemsUsed++; break;
                        default: Debug.LogError("CI mode error"); break;
                    }
                }
            }
        }
    }
    private void ClearItemTable() 
    {
        for (int i = 0; i < l; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                itemTable[i, j] = false;
                itemTimeTable[i, j] = 0.0f;
            }
        }
    }
    private float FindValueInArray(float[,] arr, float val)
    {
        float index = 0; /// result
        int min = 0;
        int max = arr.Length - 1;

        return index;
    }
    private float AverageActivityTime(float [,] arr)
    {
        float result = 0;
        int unusedItems = 0 ;
        for (int i = 0; i < l; i ++)
        {
            for (int j = 0; j < 2; j ++)
            {
            	if (arr[i,j] != 0f)
            	{
            		result += arr[i, j];
            	}
            	else
            	{
                   unusedItems++;
            	}
            }
        }
        if (result == 0) return 0;
        else return result / (arr.Length - unusedItems);
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
    private void RecordData(int id, float time, float act, int enemies, bool won)
    {
        IDbCommand cmnd = dbcon.CreateCommand();
        string stime = time.ToString();
        string sact = act.ToString();
        int w = (won == true) ? (1) : (0);      
        string values = id +"," + FormatStringToSql(stime) +"," + FormatStringToSql(sact)+"," + enemies + "," + w;
        cmnd.CommandText = "INSERT INTO recordTable (id, time, act, enemies, won) VALUES ( "+ values + ")";
        cmnd.ExecuteNonQuery();
    }
    private string FormatStringToSql(string s)
    {
      string st = "";
      for (int i = 0 ; i < s.Length; i ++)
      {
        if (s[i]!= ',') st += s[i];
        else st += '.';
      }
      return st;
    }
    private void ClearRecordTable()
    {
        ConnectDataBase();
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "DELETE FROM recordTable;";
        cmnd.ExecuteNonQuery();
        Debug.Log("DB cleared!");
        CloseDataBase();
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
    private void FetchData()
    {
        string report = "";
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query ="SELECT * FROM recordTable";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            report += ("//DATABASE DATA!//\n ");
            report += ("Won?: " + reader[4] +"\n");
            report += ("ID: " + reader[0] + "\n");
            report += ("Survival time: " + reader[1] + "\n");
            report += ("Average activity: " + reader[2] + "\n");
            report += ("Enemies: " + reader[3] +"\n");
        }
        Debug.Log(report);
        CloseDataBase();
    }
    private int GetNewId()
    {
        int count;
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT COUNT(*) FROM recordTable";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        return Convert.ToInt32(reader[0]);
    }
    private void CloseDataBase()
    {
        dbcon.Close();
        Debug.Log("DB connection closed");
    }
}
