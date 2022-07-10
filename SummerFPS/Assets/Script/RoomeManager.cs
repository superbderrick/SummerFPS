
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class RoomeManager : MonoBehaviourPunCallbacks
{
    public static RoomeManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate size: " + roomList.Count);
        // foreach(Transform trans in roomListContent)
        // {
        //     Destroy(trans.gameObject);
        // }
        //
        // for(int i = 0; i < roomList.Count; i++)
        // {
        //     if(roomList[i].RemovedFromList)
        //         continue;
        //     Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        // }
    }
    
    public override void OnJoinedRoom()
    {
        // MenuManager.Instance.OpenMenu("room");
        // roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        //
        // Player[] players = PhotonNetwork.PlayerList;
        //
        // foreach(Transform child in playerListContent)
        // {
        //     Destroy(child.gameObject);
        // }
        //
        // for(int i = 0; i < players.Count(); i++)
        // {
        //     Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        // }
        //
        // startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    
    public void CreateRoom()
    {
        // if(string.IsNullOrEmpty(roomNameInputField.text))
        // {
        //     return;
        // }
        // PhotonNetwork.CreateRoom(roomNameInputField.text);
        // MenuManager.Instance.OpenMenu("loading");
    }
    
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
