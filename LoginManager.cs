using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;



public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField apiKeyInputField;
    public void OnSubmitLogin()
    {
        string apiKey = apiKeyInputField.text;
        StartCoroutine(SendApi(apiKey));
        Debug.Log("ApiKey: " + apiKey);
    }

    IEnumerator SendApi(string apiKey)
    {
        // Use string interpolation to insert the apiKey variable into the JSON payload
        string jsonPayload = $"{{\"apiKey\":\"{apiKey}\"}}";

        using (UnityWebRequest www = UnityWebRequest.Post("http://20.15.114.131:8080/api/login", jsonPayload, "application/json"))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(www.downloadHandler.text);
                // using this method, token is seperated form the Json string received
                string myToken = tokenResponse.GetToken();

                // Save the token in PlayerPrefs, so can used in other pages
                PlayerPrefs.SetString("Token", myToken);
                Debug.Log(myToken);
                SceneManager.LoadSceneAsync("playerProfile");


            }
        }
    }

}


[System.Serializable]
public class TokenResponse
{
    [SerializeField] private string token;

    public string GetToken()
    {
        return token;
    }
}