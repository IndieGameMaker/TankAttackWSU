using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomInfo;
    public TextMeshProUGUI chatMsgList;
    public TMP_InputField chatInput;
    public TextMeshProUGUI playerList;

    private PhotonView _pv;

    public override void OnEnable()
    {
        base.OnEnable();

        chatInput.onEndEdit.AddListener( _ => OnSendMessage());
    }

    private void Start()
    {
        _pv = GetComponent<PhotonView>();

        // DisplayRoomInfo();
    }

    private void DisplayRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;

        string msg = $"{currentRoom.Name} ({currentRoom.PlayerCount}/{currentRoom.MaxPlayers})";
        roomInfo.text = msg;

        // Player List
        string playerListStr = "";

        foreach(var player in PhotonNetwork.PlayerList)
        {
            string color = (player.IsMasterClient) ? "#ff0000" : "#00ff00";
            playerListStr += $"<color={color}>{player.NickName}</color>\n";
        }

        playerList.text = playerListStr;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DisplayRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayRoomInfo();
    }

    private void OnSendMessage()
    {
        // Message
        string msg = $"<color=green>[{PhotonNetwork.NickName}]</color> {chatInput.text}";
        _pv.RPC(nameof(ChatMessage), RpcTarget.AllBufferedViaServer, msg);
        chatInput.text = "";
    }

    [PunRPC]
    public void ChatMessage(string msg)
    {
        chatMsgList.text += msg + "\n";
    }
}
