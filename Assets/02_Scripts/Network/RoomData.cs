using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomCaption;

    private RoomInfo _roomInfo;

    // GetComponent<RoomData>().RoomInfo = room;

    // Property
    public RoomInfo RoomInfo
    {
        get => _roomInfo;
        set
        {
            _roomInfo = value;
            _roomCaption.text = $"{_roomInfo.Name} : ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";

            // TODO: 버튼 클릭 이벤트 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                () => PhotonNetwork.JoinRoom(_roomInfo.Name)
            );
        }
    }
}
