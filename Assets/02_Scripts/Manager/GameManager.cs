using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomInfo;

    private void Start()
    {
        DisplayRoomInfo();
    }

    private void DisplayRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;

        string msg = $"{currentRoom.Name} ({currentRoom.PlayerCount}/{currentRoom.MaxPlayers})";
        roomInfo.text = msg;
    }
}
