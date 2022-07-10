


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
	        CommonUtils.LoadScene("LobbyScene");
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
	        UpdateConnectionStatus("Failed");
	        UpdateErrorStatus(cause.ToString());
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

    }
}

