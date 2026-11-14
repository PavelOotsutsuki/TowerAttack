using UnityEngine;
using Npgsql;
using System; // Для исключений

using Debug = UnityEngine.Debug;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    //private string connString;

    private void Start()
    {
        // !!! ЗАМЕНИ ДАННЫЕ НА СВОИ (Username и Password) !!!
        //connString = "Host=host;Port=1234;Username=idinahui;Password=dopboeb;Database=pasholNahuiDB";

        // Тестируем функционал
        //WriteData("Unity_Герой", 500);
        //ReadData("Unity_Герой");

        //StartCoroutine(OnStart());
    }

    public IEnumerator OnStart()
    {
        yield return new WaitForSeconds(5f);

        //using (UnityWebRequest request = UnityWebRequest.Get("https://localhost:7206/WeatherForecast"))
        //WWWForm wWWForm = new WWWForm();
        //wWWForm.AddField("");
        
        //Uri uri = new Uri()
        //UnityWebRequest.Post(()

        using (UnityWebRequest request = UnityWebRequest.Get("https://localhost:7206/api/Players"))
        {
            request.certificateHandler = new BypassCertificate();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                //TestDTO data = JsonUtility.FromJson<TestDTO>(request.downloadHandler.text);
                //Debug.Log(data.id);
                //Debug.Log(data.name);
                //Debug.Log(data.score);
                string arrayDTO = "{\"items\":" + request.downloadHandler.text + "}";
                Debug.Log(arrayDTO);
                PlayersWrapper data = JsonUtility.FromJson<PlayersWrapper>(arrayDTO);
                Debug.Log(data.items[0].id);
                Debug.Log(data.items[0].name);
                Debug.Log(data.items[0].score);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }

    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}

[Serializable]
public class TestDTO
{
    public int id;
    public string name;
    public int score;
}

//[Serializable]
public class PlayersWrapper
{
    public TestDTO[] items;
}