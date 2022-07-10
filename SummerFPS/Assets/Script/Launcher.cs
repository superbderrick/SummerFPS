
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.LGUplus.Homework.Minifps
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
	    public static Launcher Instance;
    	
    	#region Private Serializable Fields
    	
    	[Tooltip("The Ui Text to inform the user about the connection progress")]
    	[SerializeField]
    	private Text connectionStatusText;
        
        [Tooltip("The Ui Text to inform Network error message")]
        [SerializeField]
        private Text networkErrorText;
        
        [Tooltip("The Error Message title")]
        [SerializeField]
        private Text networkErrorTitleText;
        
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
	            UpdateConnectionStatus("Connecting");
	            Connect();
            }
        }
        
        public void Connect()
        {
	        Debug.Log("Connect");
	        
	        if (PhotonNetwork.IsConnected)
	        {
		        PhotonNetwork.JoinLobby();
	        }
	        else
	        {
		        PhotonNetwork.GameVersion = gameVersion;
		        PhotonNetwork.ConnectUsingSettings();
	        }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
	        Debug.Log("OnJoinRandomFailed");
	        UpdateConnectionStatus("Failed");
	        UpdateErrorStatus(message);
        }

        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
	        Debug.Log("OnCustomAuthenticationFailed");
	        UpdateConnectionStatus("Failed");
	        UpdateErrorStatus(debugMessage);
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
	        Debug.Log("OnDisconnected");
	        UpdateConnectionStatus("Failed");
	        UpdateErrorStatus(cause.ToString());
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
	        Debug.Log("OnJoinRoomFailed");
	        UpdateConnectionStatus("Failed");
	        UpdateErrorStatus(message);
	        
        }

        public override void OnConnectedToMaster()
        {
    	    Debug.Log("Connected to Server");
            
    	    isConnected = true;
    	    
    	    UpdateConnectionStatus("Connected");
    	    
    	    PhotonNetwork.JoinLobby();

        }
        
        private void UpdateConnectionStatus(string message)
        {
    	    connectionStatusText.text = message;
        }

        private void UpdateErrorStatus(string message)
        {
	        string guideText = "네트워크 문제로 접속 실패 \n";
	        networkErrorTitleText.text = "Error Message : ";
	        
	        StringBuilder sb = new StringBuilder();

	        sb.Append(guideText);
	        sb.Append(message);

	        networkErrorText.text = sb.ToString();
        }
        
        public override void OnJoinedLobby()
        {
    	    SceneManager.LoadScene("LobbyScene");
    	    Debug.Log("Joined Lobby");
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
	        
        }



    }
}

