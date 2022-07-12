


using System.Text;
using Com.LGUplus.Homework.Minifps.Utills;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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
        
        private static string NETWORK_STATE_CONNECTED = "Connected";
        private static string NETWORK_STATE_DISCONNECTED = "DisConnected";
        private static string NETWORK_STATE_CONNECTING = "Connecting";
        private static string ERRORMESSAGETEXT_TITLE = "Error Message :" ;
        
        void Awake()
        {
	        Instance = this;
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        void Update()
        {
            if (Input.anyKeyDown) 
            {
	            UpdateConnectionStatus(NETWORK_STATE_CONNECTING);
	            if (!isConnected)
	            {
		            Connect();    
	            }
	            else
	            {
		            Debug.Log("Connecting to Server");
	            }
	            
            }
        }
        
        public void Connect()
        {
	        if (PhotonNetwork.IsConnected)
	        {
		        CommonUtils.LoadScene("LobbyScene");
	        }
	        else
	        {
		        PhotonNetwork.GameVersion = gameVersion;
		        PhotonNetwork.ConnectUsingSettings();
	        }
        }

        public override void OnConnectedToMaster()
        {
	        isConnected = true;
	        UpdateConnectionStatus(NETWORK_STATE_CONNECTED);
	        CommonUtils.LoadScene("LobbyScene");
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
	        isConnected = false;
	        UpdateConnectionStatus(NETWORK_STATE_DISCONNECTED);
	        UpdateErrorStatus(cause.ToString());
        }
        
        private void UpdateConnectionStatus(string message)
        {
    	    connectionStatusText.text = message;
        }

        private void UpdateErrorStatus(string message)
        {
	        networkErrorTitleText.text = ERRORMESSAGETEXT_TITLE;
	        networkErrorText.text = CommonUtils.GetErrorMessage(message);
        }
    }
}

