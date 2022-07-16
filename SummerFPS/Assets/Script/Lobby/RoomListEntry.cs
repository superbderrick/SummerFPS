using Com.LGUplus.Homework.Minifps.Utills;
using Photon.Pun;
using Script.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Com.LGUplus.Homework.Minifps
{
    public class RoomListEntry : MonoBehaviour
    {
        public Text RoomNameText;
        public Text RoomPlayersText;
        public Button JoinRoomButton;
        public Text RoomStatusText;
        
        private string roomName;
        private bool isFullPlayers;

        public void Start()
        {
            JoinRoomButton.onClick.AddListener(() =>
            {
                if (!isFullPlayers && RoomStatusText.text.Equals(SummerFPSGame.NOT_PLAYING_GAME))
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

            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
            RoomStatusText.text = gameStatus;

            CheckFullPlayerCount(currentPlayers, maxPlayers);
            
        }

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