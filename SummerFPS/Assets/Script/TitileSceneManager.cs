using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class TitileSceneManager : MonoBehaviourPunCallbacks
{
	#region Private Serializable Fields

	
	[Tooltip("The Ui Text to inform the user about the connection progress")]
	[SerializeField]
	private Text feedbackText;

	[Tooltip("The Ui Text to inform Network Erroe message")]
	[SerializeField]
	private Text networkErrorText;


	#endregion

	private string gameVersion = "1";



	// Start is called before the first frame update
	void Start()
    {

    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) 
        {
            Debug.Log("anyKeyDown inputted");

        }

    }

    private void ConnectLobbyRoom()
    {

    }

	public void Connect()
	{
		// we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
		feedbackText.text = "";

		
		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.IsConnected)
		{
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{



			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = this.gameVersion;
		}
	}

}