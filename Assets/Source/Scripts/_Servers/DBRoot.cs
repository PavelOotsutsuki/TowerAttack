using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Servers.DTO;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

namespace Servers
{
    public class DBRoot// : MonoBehaviour
    {
        private const string RootUri = "https://localhost:7206/api/Players";
        private const string CreateUserUriFeature = "CreateUser";
        private const string CreateUserUri = RootUri + "/" + CreateUserUriFeature;
        private const string GetUserUriFeature = "GetUser";
        private const string GetUserUri = RootUri + "/" + GetUserUriFeature;



        public DBRoot()
        { }

        public async UniTask<GetUserDTO> CreateUser(string name, string password, CancellationToken token)
        {
            CreateUserDTO user = new CreateUserDTO()
            {
                //id = 7,
                username = name,
                password = password
                //score = 0,
                //level = 1,
                //lastLogin = DateTime.Now.ToString()
            };

            string userJson = JsonUtility.ToJson(user);
            Debug.Log(userJson);

            using (UnityWebRequest request = new UnityWebRequest(CreateUserUri, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(userJson);

                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.certificateHandler = new BypassCertificate();

                await request.SendWebRequest().ToUniTask(cancellationToken: token);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.downloadHandler.text);
                    //TestDTO data = JsonUtility.FromJson<TestDTO>(request.downloadHandler.text);
                    //Debug.Log(data.id);
                    //Debug.Log(data.name);
                    //Debug.Log(data.score);
                    //string arrayDTO = "{\"items\":" + request.downloadHandler.text + "}";
                    //Debug.Log(arrayDTO);
                    //UserDTOWrapper data = JsonUtility.FromJson<UserDTOWrapper>(arrayDTO);
                    //UserDTO userDTO = new UserDTO()
                    //{
                    //    Id = data.items[0].Id,
                    //    Username = data.items[0].Username,
                    //    Score = data.items[0].Score,
                    //    Level = data.items[0].Level,
                    //    LastLogin = data.items[0].LastLogin
                    //};
                    GetUserDTO userDTO = await GetUser(name, password, token);

                    Debug.Log(userDTO.username);
                    return userDTO;
                }
                else
                {
                    Debug.LogError(request.error);
                    throw new Exception();
                }
            }
        }

        public async UniTask<GetUserDTO> GetUser(string name, string password, CancellationToken token)
        {
            try
            {
                string currentUri = $"{GetUserUri}?name={name}&password={password}";

                using (UnityWebRequest request = UnityWebRequest.Get(currentUri))
                {
                    request.certificateHandler = new BypassCertificate();
                    await request.SendWebRequest().ToUniTask(cancellationToken: token);

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        Debug.Log(request.downloadHandler.text);
                        //TestDTO data = JsonUtility.FromJson<TestDTO>(request.downloadHandler.text);
                        //Debug.Log(data.id);
                        //Debug.Log(data.name);
                        //Debug.Log(data.score);


                        //string arrayDTO = "{\"items\":" + request.downloadHandler.text + "}";
                        //Debug.Log(arrayDTO);
                        //UserDTOWrapper data = JsonUtility.FromJson<UserDTOWrapper>(arrayDTO);
                        GetUserDTO userDTO = JsonUtility.FromJson<GetUserDTO>(request.downloadHandler.text);
                        //UserDTO userDTO = new UserDTO()
                        //{
                        //    Id = data.items[0].Id,
                        //    Username = data.items[0].Username,
                        //    Score = data.items[0].Score,
                        //    Level = data.items[0].Level,
                        //    LastLogin = data.items[0].LastLogin
                        //};

                        Debug.Log(userDTO.username);
                        return userDTO;
                    }
                    else
                    {
                        Debug.LogError(request.error);
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка {nameof(DBRoot)}-->{nameof(GetUser)}: {ex.Message}");
                return null;
            }
        }
    }
}
