using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Http;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour {

    public static NetworkManager instance;

    public string serverAddress;

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
    }

    public async Task<Response> Login(string username, string password)
    {
        Request request = new Request("POST", "http://" + serverAddress + "/api/User/" + username)
        {
            Text = "\"" + password + "\""
        };
        request.AddHeader("Content-type", "application/json");

        return (Response)await request.Send();
    }

    public async Task<Response> Register(string username, string password)
    {
        Request request = new Request("PUT", "http://" + serverAddress + "/api/User/" + username)
        {
            Text = "\"" + password + "\""
        };
        request.AddHeader("Content-type", "application/json");

        return (Response)await request.Send();
    }

    public async Task<Response> GetPlayer(string username, string connectionToken)
    {
        Request request = new Request("GET", "http://" + serverAddress + "/api/Player/" + username);
        request.AddHeader("Content-type", "application/json");
        request.AddHeader("x-token", connectionToken);

        return (Response)await request.Send();
    }

    public async Task<Response> CreatePlayer(string username, string connectionToken)
    {
        Request request = new Request("PUT", "http://" + serverAddress + "/api/Player/" + username);
        request.AddHeader("Content-type", "application/json");
        request.AddHeader("Content-length", "0");
        request.AddHeader("x-token", connectionToken);

        return (Response)await request.Send();
    }

    public async Task<Response> ClicksCheck(string username, string connectionToken, int nbClicks)
    {
        Request request = new Request("POST", "http://" + serverAddress + "/api/Gameplay/ClicksCheck/" + username)
        {
            Text = "\"" + nbClicks.ToString() + "\""
        };
        request.AddHeader("Content-type", "application/json");
        request.AddHeader("x-token", connectionToken);

        return (Response)await request.Send();
    }

    public async Task<Response> BreakThrough(string username, string connectionToken)
    {
        Request request = new Request("POST", "http://" + serverAddress + "/api/Gameplay/BreakThrough/" + username);
        request.AddHeader("Content-type", "application/json");
        request.AddHeader("Content-length", "0");
        request.AddHeader("x-token", connectionToken);

        return (Response)await request.Send();
    }
}
