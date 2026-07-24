using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // a1b2a733-b17c-4d01-91d9-54adfce2e00b

    // Game Version
    private readonly string _gameVersion = "1.0";
    // Nick Name
    private string _userId = "Zackiller";

    private void Awake()
    {
        // 접속 정보 설정
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.NickName = _userId;

        // 방장이 새로운 씬을 로딩했을 때 자동으로 씬을 로딩시켜주는 기능(옵션)
        PhotonNetwork.AutomaticallySyncScene = true;

        // 포톤 서버(포톤클라우드) 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    #region 포톤 콜백
    // 포톤 서버에 접속했을 때 호출되는 콜백(Callback Method, Callback Function, Event)
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 성공!");
        
        // 로비 접속 요청
        PhotonNetwork.JoinLobby();
    }

    // 로비에 접속 완료됐을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 완료.");

        // 랜덤한 방에 입장 요청
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤조인 실패했을 때 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 입장 실패 code={returnCode}: {message}");

        // 룸 속성을 정의
        RoomOptions ro = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 20
        };

        PhotonNetwork.CreateRoom("MyRoom", ro);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");

        // 방장만이 새로운 씬을 로딩할 수 있어야 함.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }

        // PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);
    }
    #endregion
}
