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
        private const string GetUserDataUriFeature = "GetUserData";
        private const string LogInUserUriFeature = "LogInUser";

        private const string CreateUserUri = RootUri + "/" + CreateUserUriFeature;
        private const string GetUserDataUri = RootUri + "/" + GetUserDataUriFeature;
        private const string LogInUserUri = RootUri + "/" + LogInUserUriFeature;

        private Guid _currentIdUser;
        private DateTime? _lastLogInDate;

        public DBRoot()
        {
            _currentIdUser = Guid.Empty;
            _lastLogInDate = null;
        }

        public async UniTask CreateUser(string name, string password, CancellationToken token)
        {
            //CreateUserDTO user = new CreateUserDTO()
            //{
            //    //id = 7,
            //    username = name,
            //    password = password
            //    //score = 0,
            //    //level = 1,
            //    //lastLogin = DateTime.Now.ToString()
            //};

            //string currentUri = $"{CreateUserUri}?name={name}&password={password}";
            string currentUri = $"{CreateUserUri}";
            //UnityWebRequest.Post()
            WWWForm WWWForm = new WWWForm();
            WWWForm.AddField("name", name);
            WWWForm.AddField("password", password);

            using (UnityWebRequest request = UnityWebRequest.Post(currentUri, WWWForm))
            {
                request.certificateHandler = new BypassCertificate();
                await request.SendWebRequest().ToUniTask(cancellationToken: token);

                //string userJson = JsonUtility.ToJson(user);
            //Debug.Log(userJson);
            //UnityWebRequest.Post()
            //using (UnityWebRequest request = new UnityWebRequest(CreateUserUri, "POST"))
            //{
            //    byte[] bodyRaw = Encoding.UTF8.GetBytes(userJson);

            //    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            //    request.downloadHandler = new DownloadHandlerBuffer();
            //    request.SetRequestHeader("Content-Type", "application/json");
            //    request.certificateHandler = new BypassCertificate();

            //    await request.SendWebRequest().ToUniTask(cancellationToken: token);

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
                    _currentIdUser = Guid.Parse(request.downloadHandler.text.Trim('"'));
                    _lastLogInDate = DateTime.Now;
                    //GetUserDTO userDTO = await GetUserData(token);

                    Debug.Log(_currentIdUser);
                    //return userDTO;
                }
                else
                {
                    Debug.LogError(request.error);
                    throw new Exception();
                }
            }
        }

        public async UniTask LogInUser(string name, string password, CancellationToken token)
        {
            //CreateUserDTO user = new CreateUserDTO()
            //{
            //    //id = 7,
            //    username = name,
            //    password = password
            //    //score = 0,
            //    //level = 1,
            //    //lastLogin = DateTime.Now.ToString()
            //};

            //string currentUri = $"{CreateUserUri}?name={name}&password={password}";
            string currentUri = $"{LogInUserUri}";
            //UnityWebRequest.Post()
            WWWForm WWWForm = new WWWForm();
            WWWForm.AddField("name", name);
            WWWForm.AddField("password", password);

            using (UnityWebRequest request = new UnityWebRequest(currentUri, "PATCH"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.uploadHandler = new UploadHandlerRaw(WWWForm.data);

                foreach (KeyValuePair<string, string> header in WWWForm.headers)
                    request.SetRequestHeader(header.Key, header.Value);

                request.certificateHandler = new BypassCertificate();
                await request.SendWebRequest().ToUniTask(cancellationToken: token);

                //string userJson = JsonUtility.ToJson(user);
                //Debug.Log(userJson);
                //UnityWebRequest.Post()
                //using (UnityWebRequest request = new UnityWebRequest(CreateUserUri, "POST"))
                //{
                //    byte[] bodyRaw = Encoding.UTF8.GetBytes(userJson);

                //    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                //    request.downloadHandler = new DownloadHandlerBuffer();
                //    request.SetRequestHeader("Content-Type", "application/json");
                //    request.certificateHandler = new BypassCertificate();

                //    await request.SendWebRequest().ToUniTask(cancellationToken: token);

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
                    //_currentIdUser = Guid.Parse(request.downloadHandler.text);
                    //GetUserDTO userDTO = await GetUserData(token);
                    LogInDTO logInDTO = JsonUtility.FromJson<LogInDTO>(request.downloadHandler.text);
                    _currentIdUser = Guid.Parse(logInDTO.id_User);
                    _lastLogInDate = Convert.ToDateTime(logInDTO.dte_last_login);

                    Debug.Log(_currentIdUser);
                }
                else
                {
                    Debug.LogError(request.error);
                    throw new Exception();
                }
            }
        }

        //private async UniTaskVoid GetUserID(string name, string password, CancellationToken token)
        //{
        //    try
        //    {
        //        string currentUri = $"{GetUserIDUri}?name={name}&password={password}";

        //        using (UnityWebRequest request = UnityWebRequest.Get(currentUri))
        //        {
        //            request.certificateHandler = new BypassCertificate();
        //            await request.SendWebRequest().ToUniTask(cancellationToken: token);

        //            if (request.result == UnityWebRequest.Result.Success)
        //            {
        //                Debug.Log(request.downloadHandler.text);
        //                //TestDTO data = JsonUtility.FromJson<TestDTO>(request.downloadHandler.text);
        //                //Debug.Log(data.id);
        //                //Debug.Log(data.name);
        //                //Debug.Log(data.score);


        //                //string arrayDTO = "{\"items\":" + request.downloadHandler.text + "}";
        //                //Debug.Log(arrayDTO);
        //                //UserDTOWrapper data = JsonUtility.FromJson<UserDTOWrapper>(arrayDTO);
        //                //GetUserDTO userDTO = JsonUtility.FromJson<GetUserDTO>(request.downloadHandler.text);
        //                _currentIdUser = Guid.Parse(request.downloadHandler.text);
        //                //UserDTO userDTO = new UserDTO()
        //                //{
        //                //    Id = data.items[0].Id,
        //                //    Username = data.items[0].Username,
        //                //    Score = data.items[0].Score,
        //                //    Level = data.items[0].Level,
        //                //    LastLogin = data.items[0].LastLogin
        //                //};

        //                Debug.Log(_currentIdUser);
        //                //return _currentIdUser;
        //            }
        //            else
        //            {
        //                Debug.LogError(request.error);
        //                throw new Exception();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError($"Ошибка {nameof(DBRoot)}-->{nameof(GetUserID)}: {ex.Message}");
        //        //return Guid.Empty;
        //    }
        //}

        public async UniTask<GetUserDTO> GetUserData(CancellationToken token)
        {
            try
            {
                Guid id = _currentIdUser;

                string currentUri = $"{GetUserDataUri}?id={id}";

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
                        //Guid userDTO = Guid.Parse(request.downloadHandler.text);
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
                Debug.LogError($"Ошибка {nameof(DBRoot)}-->{nameof(GetUserData)}: {ex.Message}");
                return null;
            }
        }
    }
}
