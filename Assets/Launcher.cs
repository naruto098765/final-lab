using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public GameObject createRoomScreen;

    public TMP_InputField roomNameInput;

    public GameObject createdRoomScreen;
    public TMP_Text roomNameText;

    public GameObject ErrorPanel;
    public TMP_Text errorText;

    public GameObject roomBrowserScreen;
    public RoomButton theRoomButton;
    public List<RoomButton> allRoomButtons = new List<RoomButton>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting....";

        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        loadingText.text = "jonning Lobby...";
    }
    public override void OnJoinedLobby()
    {
        loadingScreen.SetActive(false);
    }
    public void OpenCreateRoomScreen()
    {
        createRoomScreen.SetActive(true);
    }
    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            RoomOptions roomOption = new RoomOptions();
            roomOption.MaxPlayers = 10;
            PhotonNetwork.CreateRoom(roomNameInput.text, roomOption);
            loadingScreen.SetActive(true);
            loadingText.text = "Creating Room...";
        }
    }
    public override void OnCreatedRoom()
    {
        loadingScreen.SetActive(false);
        createdRoomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void LeaveRoom()
    {
        createdRoomScreen.SetActive(false);
        loadingScreen.SetActive(true);
        loadingText.text = "Leaving Room";
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed to Connect: " + message;
        ErrorPanel.SetActive(true);
    }

    /*public void CloseErrorScreen()
    {
       // menuButtons.setActive(true);
    }*/

    public void OpenRoomBrowser()
    {
        roomBrowserScreen.SetActive(true);
    } 
    
    public void CloseRoomBrowser()
    {
        roomBrowserScreen.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomButton rb in allRoomButtons) {
            Destroy(rb.gameObject);
        }

        allRoomButtons.Clear();

        theRoomButton.gameObject.SetActive(false);

        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newButton = Instantiate(theRoomButton, theRoomButton.transform.parent);
                newButton.SetButtonDetails(roomList[i]);
                newButton.gameObject.SetActive(true);

                allRoomButtons.Add(newButton);
            }
        }
    }

}