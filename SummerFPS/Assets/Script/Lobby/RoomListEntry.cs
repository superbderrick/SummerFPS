using Jinyoung.dev.summerfps.Utills;
using Photon.Pun;
using Script.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Jinyoung.dev.summerfps.lobby
{
    public class RoomListEntry : MonoBehaviour
    {
        public Text RoomNameText;
        public Text RoomPlayersText;
        public Button JoinRoomButton;
        public Text RoomStatusText;
        
        private string roomName;
        private bool isFullPlayers;
        private bool isPlaying = false;

        public void Start()
        {
            JoinRoomButton.onClick.AddListener(() =>
            {
                // Check the total number of players in the game and check the progress of the game
                if (!isFullPlayers && !isPlaying)
                {
                    if (PhotonNetwork.InLobby)
                    {
                        PhotonNetwork.LeaveLobby();
                    }
                    
                    PhotonNetwork.JoinRoom(roomName);
                }
                else
                {
                    Debug.Log("can't use a room");
                    //dialog
                }
 
            });
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers , string gameStatus)
        {
            roomName = name;

            RoomNameText.text = CommonUtils.GetStringMessage("RoomName: ", name);
            RoomPlayersText.text = CommonUtils.GetStringMessage(currentPlayers.ToString(),maxPlayers.ToString() , "/" , "Members : ");
            RoomStatusText.text = CommonUtils.GetStringMessage("Game Status: ", gameStatus);

            CheckRoomStatus(gameStatus);

            CheckFullPlayerCount(currentPlayers, maxPlayers);
            
        }

        private void CheckRoomStatus(string gameStatus)
        {
            if (gameStatus.Equals(SummerFPSGame.NOT_PLAYING_GAME))
            {
                isPlaying = false;
            }
            else
            {
                isPlaying = true;
            }
        }

        // Check the total number of players
        private void CheckFullPlayerCount(byte currentPlayers, byte maxPlayers)
        {
            if (currentPlayers == maxPlayers)
            {
                isFullPlayers = true;
            }
            else
            {
                isFullPlayers = false;
            }
        }
    }
}