
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.LGUPlus.HomeWork.MINIFPS
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
    	    
        }
    
        public override void OnDisconnected(DisconnectCause cause)
        {
    	    
        }
    
        private void UpdateUIStatus()
        {
    	    connectionStatusText.text = "Connected";
    	    
        }
        
        public override void OnJoinedLobby()
        {
    	    SceneManager.LoadScene("LobbyScene");
    	    Debug.Log("Joined Lobby");
        }
    
    }
}

