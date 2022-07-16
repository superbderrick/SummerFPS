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
        private bool isPlaying = false;

        public void Start()
        {
            JoinRoomButton.onClick.AddListener(() =>
            {
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