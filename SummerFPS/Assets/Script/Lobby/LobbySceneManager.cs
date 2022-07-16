
using Photon.Realtime;
using System.Collections.Generic;
using Com.LGUplus.Homework.Minifps.Utills;
using Photon.Pun;
using Script.Game;
using UnityEngine;

namespace Com.LGUplus.Homework.Minifps
{
    public class LobbySceneManager : MonoBehaviourPunCallbacks
    {
        [Header("Selection Panel")]
        public GameObject SelectionPanel;
        
        [Header("Room List Panel")]
        public GameObject RoomListPanel;
        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;
        
        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;


        #region UNITY

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
        }

        #endregion

        #region PUN CALLBACKS
        
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();
            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnJoinedLobby()
        {
            cachedRoomList.Clear();
            ClearRoomListView();
        }
        
        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }
        
        public override void OnJoinedRoom()
        {
             cachedRoomList.Clear();
             CommonUtils.LoadScene("GameRoomScene");
        }
        
        #endregion

        #region UI CALLBACKS

        public void OnBackButtonClicked()
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            SetActivePanel(SelectionPanel.name);
        }

        public void OnCreateRoomButtonClicked()
        {
            RoomOptions ropts = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(ropts.GetHashCode().ToString(), ropts,null);
        }

        
        public void OnRoomListButtonClicked()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }

            SetActivePanel(RoomListPanel.name);
        }
        
        #endregion
        
        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }
        
        private void SetActivePanel(string activePanel)
        {
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                if (!info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }
                
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }

        private void UpdateRoomListView()
        {
            
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                
                var gamestatus = GetGameStatus(info);
                
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers,gamestatus);
                
                roomListEntries.Add(info.Name, entry);
            }
        }

        private static string GetGameStatus(RoomInfo info)
        {
            string gamestatus = SummerFPSGame.NOT_PLAYING_GAME;

            if (info.IsOpen == false)
            {
                gamestatus = SummerFPSGame.PLAYING_GAME;
            }
            else
            {
                gamestatus = SummerFPSGame.NOT_PLAYING_GAME;
            }

            return gamestatus;
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            CommonUtils.LoadScene("TitleScene");
        }
    }
}