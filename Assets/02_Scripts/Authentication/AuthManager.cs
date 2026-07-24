using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    [Header("Login UI")]
    [SerializeField] private TMP_InputField _userIdInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _signUpButton;
    [SerializeField] private Button _logInButton;

    private async void Awake()
    {
        // UGS 초기화
        await UnityServices.InitializeAsync();

        _logInButton.onClick.AddListener(async () => 
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("익명 사용자 로그인 성공");
        });
    }
}
