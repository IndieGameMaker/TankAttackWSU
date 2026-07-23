using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomInfo;

    private void Start()
    {
        Vector3 pos = new Vector3(Random.Range(-100, 100), 3.0f, Random.Range(-100, 100));

        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);

        DisplayRoomInfo();
    }

    private void DisplayRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;

        string msg = $"{currentRoom.Name} ({currentRoom.PlayerCount}/{currentRoom.MaxPlayers})";
        roomInfo.text = msg;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DisplayRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayRoomInfo();
    }
}
