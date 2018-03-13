using Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    public static GameManager instance;

    public GameObject gameCanvas;

    private string _username;
    private string _connectionToken;

    private float _timeBeforeNextCheck = 0;
    public float timeBetweenRegularChecks = 3;

    private int _soulForcePerClick;
    public Text soulForceText;

    private int _maxSoulForce;
    public Text maxSoulForcetext;
    
    public Text soulLevelText;

    private Player _player;

    private bool _logged;

    private int _numberOfClickSinceLastCheck;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        gameCanvas.SetActive(false);
    }

    public void Init()
    {
        _username = LoginManager.instance.usernameInputField.text;
        _connectionToken = LoginManager.instance.connectionToken;
        _player = LoginManager.instance.player;


        _numberOfClickSinceLastCheck = 0;
        _timeBeforeNextCheck = timeBetweenRegularChecks;
        _logged = true;

        Calculate();

        soulForceText.text = _player.soulForce.ToString();
        maxSoulForcetext.text = "/" + _maxSoulForce.ToString();
        soulLevelText.text = "Level : " + _player.soulLevel.ToString();
    }

    private void Calculate()
    {
        _soulForcePerClick = _player.soulLevel * 5;
        _maxSoulForce = Mathf.RoundToInt(Mathf.Exp(_player.soulLevel) * 5);
    }

    public void Click()
    {
        if (_logged)
        {
            _player.soulForce = Mathf.Clamp(_player.soulForce + _soulForcePerClick, 0, _maxSoulForce);
            soulForceText.text = _player.soulForce.ToString();

            _numberOfClickSinceLastCheck++;
        }
    }

    private void Update()
    {
        if (_logged)
        {
            if (_timeBeforeNextCheck - Time.deltaTime <= 0)
            {
                CheckClicks();
            }
            else
            {
                _timeBeforeNextCheck -= Time.deltaTime;
            }
        }
    }

    public async void TryBreakThrough()
    {
        if (_logged)
        {
            bool res = await TrySendLatestClicks();

            if (_player.soulForce > 0 && res)
            {
                Response response = await NetworkManager.instance.BreakThrough(_username, _connectionToken);
                if (response.status == 200)
                {
                    if (response.Text == "true")
                    {
                        _player.soulLevel++;
                        Calculate();
                    }

                    _player.soulForce = 0;
                    soulForceText.text = _player.soulForce.ToString();
                    maxSoulForcetext.text = "/" + _maxSoulForce.ToString();
                    soulLevelText.text = "Level : " + _player.soulLevel.ToString();
                }
            }
        }
    }

    public async void CheckClicks()
    {
        if (_logged)
        {
            _timeBeforeNextCheck = timeBetweenRegularChecks;
            await TrySendLatestClicks();
        }
    }

    public async Task<bool> TrySendLatestClicks()
    {
        if (_logged)
        {
            int tmp = _numberOfClickSinceLastCheck;
            _numberOfClickSinceLastCheck = 0;

            Response response = await NetworkManager.instance.ClicksCheck(_username, _connectionToken, tmp);
            if (response.status == 200)
            {
                return true;
            }
        }

        return false;
    }
}
