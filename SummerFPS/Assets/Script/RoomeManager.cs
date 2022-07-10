
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class RoomeManager : MonoBehaviourPunCallbacks
{
    public Chibi.Free.Dialog dialog;
    public static RoomeManager Instance;
    
    void Start()
    {
        Debug.Log("Phonstatus" + PhotonNetwork.IsConnected);
        Debug.Log("CurrentRoom" + PhotonNetwork.CurrentRoom);
        Debug.Log("InLobby" + PhotonNetwork.InLobby);
        
        
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate size: " + roomList.Count);
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom ");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom " );
    }
    
    private void ShowRoomDialog()
    {
        var cancel = new Chibi.Free.Dialog.ActionButton("Cancel", null, new Color(0.9f, 0.9f, 0.9f));
        var ok = new Chibi.Free.Dialog.ActionButton("OK", () =>
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4});
            
        }, new Color(0f, 0.9f, 0.9f));
        Chibi.Free.Dialog.ActionButton[] buttons = { cancel, ok };
        dialog.ShowDialog("Summer FPS Game", "새로운 방을 만들겠습니까?", buttons, () =>
        {
            
        }, true);
    }
    public void CreateRoom()
    {
        ShowRoomDialog();
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed: " + message);
    }
    
    void Awake()
    {
        Instance = this;
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
	        
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
	        
    }
        
    
}
