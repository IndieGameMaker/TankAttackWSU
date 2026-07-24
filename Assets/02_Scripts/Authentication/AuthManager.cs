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
        UnityServices.Initialized += () => Debug.Log("유니티 서비스 초기화 완료");

        // UGS 초기화
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => 
        {
            Debug.Log("익명 사용자 로그인 성공");
            Debug.Log($"익명 사용자 Id: {AuthenticationService.Instance.PlayerId}");
        };

        _logInButton.onClick.AddListener(async () => 
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        });
    }
}
