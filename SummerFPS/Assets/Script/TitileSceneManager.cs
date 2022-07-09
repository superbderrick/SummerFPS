using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class TitileSceneManager : MonoBehaviourPunCallbacks
{

	public static TitileSceneManager Instance;
	
	#region Private Serializable Fields
	
	[Tooltip("The Ui Text to inform the user about the connection progress")]
	[SerializeField]
	private Text connectionStatusText;

	[Tooltip("The Ui Text to inform Network Erroe message")]
	[SerializeField]
	private Text networkErrorText;


	#endregion

	private string gameVersion = "1";
	private bool isConnected = false;

	
    void Awake()
    {
	    Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    void Update()
    {
        if (Input.anyKeyDown) 
        {
            Debug.Log("Inputted any keys");
            
            connectionStatusText.text = "Connecting";
            
			ConnectLobbyRoom();
        }
    }

    private void ConnectLobbyRoom()
    {
	    Debug.Log("Connecting to Server");
	    PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
	    Debug.Log("Connected to Server");
	    isConnected = true;
	    
	    UpdateUIStatus();
	    
	    PhotonNetwork.JoinLobby();
	    PhotonNetwork.AutomaticallySyncScene = true;
	    
	    SceneManager.LoadScene("LobbyScene");
    }

    private void UpdateUIStatus()
    {
	    connectionStatusText.text = "Connected";
	    
    }
    

}