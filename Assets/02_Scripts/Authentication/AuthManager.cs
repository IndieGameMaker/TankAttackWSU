using Photon.Pun;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    [Header("Login UI")]
    [SerializeField] private TMP_InputField _userIdInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _signUpButton;
    [SerializeField] private Button _logInButton;
    [SerializeField] private List<Button> _networkButtons;

    private async void Awake()
    {
        foreach (var button in _networkButtons)
        {
            button.interactable = false;
        }

        UnityServices.Initialized += () => Debug.Log("유니티 서비스 초기화 완료");

        // UGS 초기화
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => 
        {
            Debug.Log("익명 사용자 로그인 성공");
            Debug.Log($"익명 사용자 Id: {AuthenticationService.Instance.PlayerId}");
        };

        AuthenticationService.Instance.SignedOut += () => Debug.Log("로그아웃 완료");

        // Login
        _logInButton.onClick.AddListener(async () => 
        {
            // 익명 사용자 로그인
            // await AuthenticationService.Instance.SignInAnonymouslyAsync();
            try
            { 
                // Username & Password 로그인
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(_userIdInput.text, _passwordInput.text);
                Debug.Log("로그인 성공");

                foreach (var button in _networkButtons)
                {
                    button.interactable = true;
                }
                PhotonNetwork.NickName = _userIdInput.text;
            }
            catch (AuthenticationException e)
            {
                Debug.Log(e.Message);
            }
            catch (RequestFailedException e)
            {
                Debug.Log(e.Message);
            }
        });

        // SignUp
        _signUpButton.onClick.AddListener(async () =>
        {
            await SignUpUser(_userIdInput.text, _passwordInput.text);
        });

    }

    // 회원 가입
    // 아이디   : 대소문자 구별 없음, 3자 ~ 20자, [. - @] 허용
    // 비밀번호 : 대소문자 구별, 8자 ~ 30자, 숫자 1, 영문자 대문자 1, 소문자 1, 특수문자 1
    async Task SignUpUser(string userName, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(userName, password);
            Debug.Log("회원가입 성공");
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e.Message);
        }
        catch (RequestFailedException e)
        {
            Debug.Log(e.Message);
        }
    }


    // 로그인 처리



    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            AuthenticationService.Instance.SignOut();
        }
    }
}
