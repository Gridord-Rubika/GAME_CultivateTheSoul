using Http;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    public static LoginManager instance;

    public InputField usernameInputField;
    public InputField passwordInputField;

    public string connectionToken = "";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [ContextMenu("test")]
    public void Test()
    {
        SceneManager.LoadScene(0);
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
        SceneManager.LoadScene(1);
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
        SceneManager.LoadScene(1);
    }

    #region Validators

    private bool ValidateInputs(string username, string password)
    {
        string message = "";
        if (username.Length == 0)
        {
            message = "Username field is empty";
            return false;
        }
        else if (username.Length > 30)
        {
            message = "Username must be under 30 characters";
            return false;
        }
        else if (!Regex.IsMatch(username, "^[A-Za-z0-9]+$"))
        {
            message = "Username must be alphanumerical characters only";
            return false;
        }
        else if (password.Length == 0)
        {
            message = "Password is empty";
            return false;
        }

        if(message != "")
        {
            GameObject obj = GameObject.FindGameObjectWithTag("DisplayMessage");
            if(obj != null)
            {
                obj.GetComponent<Text>().text = message;
            }
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
