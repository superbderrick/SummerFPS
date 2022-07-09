
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
                Debug.Log("Inputted any keys" + PhotonNetwork.IsConnected);
                
                connectionStatusText.text = "Connecting";
                
                
    			Connect();
            }
        }
        
        
        public void Connect()
        {
	        Debug.Log("Connect");
	        
	        if (PhotonNetwork.IsConnected)
	        {
		        PhotonNetwork.JoinRandomRoom();
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
	        UpdateConnectionStatus(message);
        }

        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
	        Debug.Log("OnCustomAuthenticationFailed");
	        UpdateConnectionStatus(debugMessage);
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
	        Debug.Log("OnDisconnected");
	        UpdateConnectionStatus(cause.ToString());
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
	        Debug.Log("OnJoinRoomFailed");
	        UpdateConnectionStatus(message);
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
        
        public override void OnJoinedLobby()
        {
    	    SceneManager.LoadScene("LobbyScene");
    	    Debug.Log("Joined Lobby");
        }
    
    }
}

