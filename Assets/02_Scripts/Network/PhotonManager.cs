using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // a1b2a733-b17c-4d01-91d9-54adfce2e00b

    // Game Version
    private readonly string _gameVersion = "1.0";
    // Nick Name
    private string _userId = "Zackiller";

    [Header("Login UI")]
    [SerializeField] private TMP_InputField _userIdInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _signUpButton;
    [SerializeField] private Button _logInButton;
    [SerializeField] private Button _randomJoinButton;

    [Header("Lobby UI")]
    [SerializeField] private Button _lobbyButton;


    [Header("방생성 UI")]
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private Button _createRoomButton;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup _loginCG;
    [SerializeField] private CanvasGroup _inRoomCG;

    [Header("Room List")]
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private Transform _contentTr;

    private void Awake()
    {
        // 접속 정보 설정
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.NickName = _userId;

        // 방장이 새로운 씬을 로딩했을 때 자동으로 씬을 로딩시켜주는 기능(옵션)
        PhotonNetwork.AutomaticallySyncScene = true;

        // 포톤 서버(포톤클라우드) 접속
        PhotonNetwork.ConnectUsingSettings();

        _inRoomCG.alpha = 0.0f;
        _inRoomCG.interactable = false;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _randomJoinButton.onClick.AddListener(RandomJoinRoom);
        _createRoomButton.onClick.AddListener(CreateRoom);
        _lobbyButton.onClick.AddListener(RequestLeaveRoom);
    }

    private void RequestLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void CreateRoom()
    {
        RoomOptions ro = new RoomOptions
        {
            IsOpen = true, IsVisible = true, MaxPlayers = 10
        };
        PhotonNetwork.CreateRoom(_roomNameInput.text, ro);
    }

    private void RandomJoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #region 포톤 콜백
    public override void OnLeftRoom()
    {
        // UI 변경
        _inRoomCG.alpha = 0.0f;
        _inRoomCG.interactable = false;

        _loginCG.alpha = 1.0f;
        _loginCG.interactable = true;

        // Scene Move
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }


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
        // PhotonNetwork.JoinRandomRoom();
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
        _inRoomCG.alpha = 1.0f;
        _inRoomCG.interactable = true;

        _loginCG.alpha = 0.0f;
        _loginCG.interactable = false;

        PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);

        GameManager.Instance.DisplayRoomInfo();
    }

    // 룸 목록을 저장할 Dictionary 선언
    private Dictionary<string, GameObject> roomDict = new();// new Dictionary<string, GameObject>();

    // 룸 목록이 변경되면 호출되는 콜백
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            Debug.Log($"{room.Name} : ({room.PlayerCount}/{room.MaxPlayers})");
            // 방이 삭제된 경우 room prefab 삭제
            if (room.RemovedFromList == true)
            {
                if (roomDict.TryGetValue(room.Name, out var tempRoom))
                {
                    // 딕셔너리에 저장된 Room Prefab을 삭제
                    Destroy(tempRoom);
                    // 딕셔너리의 데이터도 삭제
                    roomDict.Remove(room.Name);
                }
                continue;
            }

            // 새로 생성된 룸
            if (roomDict.ContainsKey(room.Name) == false)
            {
                // Room 프리팹 생성
                var _room = Instantiate(_roomPrefab, _contentTr);
                // 속성 설정
                _room.GetComponent<RoomData>().RoomInfo = room;
                // 딕셔너리에 저장
                roomDict.Add(room.Name, _room);
            }
            else
            {
                // 룸이 변경된 경우
                // 딕서너리에서 검색후 교체
                if (roomDict.TryGetValue(room.Name, out var tempRoom))
                {
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }
    #endregion
}
