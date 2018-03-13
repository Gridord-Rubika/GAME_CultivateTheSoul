using Http;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    public static LoginManager instance;

    public GameObject loginCanvas;

    public InputField usernameInputField;
    public InputField passwordInputField;

    public string connectionToken = "";

    public Player player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        loginCanvas.SetActive(true);
    }

    public async void TryLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (!ValidateInputs(username, password))
        {
            return;
        }

        Response response = await NetworkManager.instance.Login(username, password);

        if (!ValidateLoginStatus(response.status))
        {
            return;
        }

        connectionToken = response.ReadAsString();
        TryGetPlayer();
    }

    public async void TryRegister()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (!ValidateInputs(username, password))
        {
            return;
        }

        Response response = await NetworkManager.instance.Register(username, password);

        if (!ValidateRegisterStatus(response.status)){
            return;
        }

        connectionToken = response.ReadAsString();
        TryGetPlayer();
    }


    private async void TryGetPlayer()
    {
        string username = usernameInputField.text;
        bool succeeded = false;

        Response response = await NetworkManager.instance.GetPlayer(username, connectionToken);
        
        //Player found
        if (response.status == 200)
        {
            player = JsonUtility.FromJson<Player>(response.Text);
            succeeded = true;
            loginCanvas.SetActive(false);
            GameManager.instance.gameCanvas.SetActive(true);
            GameManager.instance.Init();
        }

        //No player found but no problem, need to create a player
        else if (response.status == 404)
        {
            Response responseCreate = await NetworkManager.instance.CreatePlayer(username, connectionToken);

            //Player successfully created
            if (responseCreate.status == 200)
            {
                player = JsonUtility.FromJson<Player>(response.Text);
                succeeded = true;
                loginCanvas.SetActive(false);
                GameManager.instance.gameCanvas.SetActive(true);
                GameManager.instance.Init();
            }
        }

        if (!succeeded)
        {
            //If we are here, then it means there is problems and we can't do much
            GameObject obj = GameObject.FindGameObjectWithTag("DisplayMessage");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = "A problem occured with the server and no player could be created or found.";
            }
        }
    }

    #region Validators

    private bool ValidateInputs(string username, string password)
    {
        string message = "";
        if (username.Length == 0)
        {
            message = "Username field is empty";
        }
        else if (username.Length > 30)
        {
            message = "Username must be under 30 characters";
        }
        else if (!Regex.IsMatch(username, "^[A-Za-z0-9]+$"))
        {
            message = "Username must be alphanumerical characters only";
        }
        else if (password.Length == 0)
        {
            message = "Password is empty";
        }

        if(message != "")
        {
            GameObject obj = GameObject.FindGameObjectWithTag("DisplayMessage");
            if(obj != null)
            {
                obj.GetComponent<Text>().text = message;
            }
            return false;
        }

        return true;
    }

    private bool ValidateLoginStatus(int status)
    {
        string message = "";
        if (status == 200)
        {
            return true;
        }
        else if (status == 400)
        {
            message = "Request is not correct";
        }
        else if (status == 403)
        {
            message = "Bad password";
        }
        else if (status == 404)
        {
            message = "Username not found";
        }
        else
        {
            message = "Unkonwn status response : " + status.ToString();
        }

        if (message != "")
        {
            GameObject obj = GameObject.FindGameObjectWithTag("DisplayMessage");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = message;
            }
        }

        return false;
    }

    private bool ValidateRegisterStatus(int status)
    {
        string message = "";
        if (status == 200)
        {
            return true;
        }
        else if (status == 400)
        {
            message = "Request is not correct";
        }
        else if (status == 403)
        {
            message = "Username already used";
        }
        else if (status == 404)
        {
            message = "Username not found";
        }
        else
        {
            message = "Unkonwn status response : " + status.ToString();
        }

        if (message != "")
        {
            GameObject obj = GameObject.FindGameObjectWithTag("DisplayMessage");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = message;
            }
        }

        return false;
    }

    #endregion
}
