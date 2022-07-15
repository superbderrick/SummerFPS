﻿using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using Com.LGUplus.Homework.Minifps.Utills;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;
using UnityEngine.UI;

namespace Com.LGUplus.Homework.Minifps
{
    public class GameRoomManager : MonoBehaviourPunCallbacks
    {
        [Header("Inside Room Panel")]
        public GameObject InsideRoomPanel;

        public Button StartGameButton;
        public Button RoomNameButton;
        
        public GameObject PlayerListEntryPrefab;
        private Dictionary<int, GameObject> playerListEntries;

        #region UNITY

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            SetupList();
        }
        
        private void SetupList()
        {
            RoomNameButton.GetComponentInChildren<Text>().text = "Room Name : \n" + PhotonNetwork.CurrentRoom.Name;

            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerListEntryPrefab);
                entry.transform.SetParent(InsideRoomPanel.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<SummerPlayerListEntry>().Initialize(p.ActorNumber, p.NickName);
            
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<SummerPlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
                }
            
                playerListEntries.Add(p.ActorNumber, entry);
            }
            
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
            
            Hashtable props = new Hashtable
            {
                { AsteroidsGame.PLAYER_LOADED_LEVEL, false }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        #endregion

        #region PUN CALLBACKS
        

        public override void OnLeftRoom()
        {
            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
            
            CommonUtils.LoadScene("LobbyScene");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<SummerPlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

            playerListEntries.Add(newPlayer.ActorNumber, entry);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                object isPlayerReady;
                if (changedProps.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<SummerPlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
                }
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        #endregion

        #region UI CALLBACKS
        
        public void OnLeaveGameButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        public void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            
            Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;

            print(CP["키1"]);

            CP["키1"] = "summerderrick";
           
            
            PhotonNetwork.LoadLevel("TargetGame");
        }

        #endregion

        private bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
                {
                    if (!(bool) isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public void LocalPlayerPropertiesUpdated()
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            CommonUtils.LoadScene("TitleScene");
        }
    }
}