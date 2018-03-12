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
        return await DoConnectRequest("POST", username, password);
    }

    public async Task<Response> Register(string username, string password)
    {
        return await DoConnectRequest("PUT", username, password);
    }

    public async Task<Response> UpdatePlayer(string username, string connectionToken)
    {
        return await DoGameplayRequest("POST", username, connectionToken);
    }

    private async Task<Response> DoConnectRequest(string method, string username, string password)
    {
        Request loginRequest = new Request(method, "http://" + serverAddress + "/api/connect/" + username)
        {
            Text = "\"" + password + "\""
        };
        loginRequest.AddHeader("Content-type", "application/json");

        return (Response)await loginRequest.Send();
    }

    private async Task<Response> DoGameplayRequest(string method, string username, string connectionToken)
    {
        Request loginRequest = new Request(method, "http://" + serverAddress + "/api/connect/" + username);
        loginRequest.AddHeader("Content-type", "application/json");
        loginRequest.AddHeader("x-token", connectionToken);

        return (Response) await loginRequest.Send();
    }
}
