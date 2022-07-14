
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Com.LGUplus.Homework.Minifps
{
    public class LobbyMainPanel : MonoBehaviourPunCallbacks
    {
        [Header("Selection Panel")]
        public GameObject SelectionPanel;
        
        [Header("Room List Panel")]
        public GameObject RoomListPanel;

        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;
        
        public Button StartGameButton;
        public Button RoomNameButton;
        public GameObject PlayerListEntryPrefab;

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
            Debug.Log("OnRoomListUpdate");
            ClearRoomListView();
            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnJoinedLobby()
        {
            // whenever this joins a new lobby, clear any previous room lists
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
        public override void OnLeftLobby()
        {
            Debug.Log("OnLeftLobby");
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }
        
        public override void OnJoinedRoom()
        {

            cachedRoomList.Clear();

            //이동 시점
            RoomNameButton.GetComponentInChildren<Text>().text = "Room Name : \n" + PhotonNetwork.CurrentRoom.Name;
            
            //이동 시점 
            // SetActivePanel(InsideRoomPanel.name);
            //
            // if (playerListEntries == null)
            // {
            //     playerListEntries = new Dictionary<int, GameObject>();
            // }

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
            RoomOptions options = new RoomOptions {MaxPlayers = 4, PlayerTtl = 10000 };
            options.CustomRoomProperties = new Hashtable (){{"summer", "derrick"}};
            options.CustomRoomPropertiesForLobby = new string[] {"summer"};
            
            PhotonNetwork.CreateRoom(options.GetHashCode().ToString(), options, null);
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
            Debug.Log("ClearRoomListView");
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }
        
        private void SetActivePanel(string activePanel)
        {
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
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
            Debug.Log("UpdateRoomListView" + cachedRoomList.Count);
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
            
                
                // PhotonNetwork.InLobby
                // Room room = PhotonNetwork.roomin;
                // if (room == null) {
                //     return;
                // }
                // // 룸의 커스텀 프로퍼티를 취득
                // Hashtable cp = room.CustomProperties;
                // GUILayout.Label ((string)cp ["CustomProperties"], GUILayout.Width (150));
                //
                
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers,"test");
                
                
                roomListEntries.Add(info.Name, entry);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}