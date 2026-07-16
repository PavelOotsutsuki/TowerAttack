using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Servers.DTO;
using Tools.Loads;
using UnityEngine;
using UnityEngine.Networking;

namespace Servers
{
    public class DBRoot
    {
        private const string RootUri = "https://localhost:7206/api/Players";

        private const string CreateUserUriFeature = "CreateUser";
        private const string GetMainMenuUserDataUriFeature = "GetMainMenuUserData";
        private const string LogInUserUriFeature = "LogInUser";
        private const string StartFightWithBotUriFeature = "StartFightWithBot";
        private const string FinishFightWithBotUriFeature = "FinishFightWithBot";
        private const string GetLastAddedExpUriFeature = "GetLastAddedExp";

        private const string CreateUserUri = RootUri + "/" + CreateUserUriFeature;
        private const string GetMainMenuUserDataUri = RootUri + "/" + GetMainMenuUserDataUriFeature;
        private const string LogInUserUri = RootUri + "/" + LogInUserUriFeature;
        private const string StartFightWithBotUri = RootUri + "/" + StartFightWithBotUriFeature;
        private const string FinishFightWithBotUri = RootUri + "/" + FinishFightWithBotUriFeature;
        private const string GetLastAddedExpUri = RootUri + "/" + GetLastAddedExpUriFeature;

        private readonly LoadRoot _loadRoot;

        private Guid _currentIdUser;
        private DateTime? _lastLogInDate;

        private Guid _currentIdFight;
        private Guid _currentEnemyIdUser;

        public DBRoot(LoadRoot loadRoot)
        {
            _loadRoot = loadRoot;
            _currentIdUser = Guid.Empty;
            _lastLogInDate = null;
            _currentIdFight = Guid.Empty;
            _currentEnemyIdUser = Guid.Empty;
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
                try
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
                        throw new Exception(request.downloadHandler.text);
                    }
                }
                catch
                {
                    Debug.LogError("CreateUser: " + request.error + " " + request.downloadHandler.text);
                    throw new Exception(request.downloadHandler.text);
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
                try
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

                        Debug.Log($"Id_User: {_currentIdUser}");
                    }
                    else
                    {
                        Debug.LogError(request.error);
                        throw new Exception(request.downloadHandler.text);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("LogInUser -> выполнение CATCH");
                    throw new Exception(request.downloadHandler.text);
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

        public async UniTask<GetMainMenuUserDataDTO> GetMainMenuUserData(CancellationToken token)
        {
            try
            {
                Guid id = _currentIdUser;

                string currentUri = $"{GetMainMenuUserDataUri}?id={id}";

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
                        GetMainMenuUserDataDTO userDTO = JsonUtility.FromJson<GetMainMenuUserDataDTO>(request.downloadHandler.text);
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
                Debug.LogError($"Ошибка {nameof(DBRoot)}-->{nameof(GetMainMenuUserData)}: {ex.Message}");
                return null;
            }
        }

        public async UniTask StartFightWithBot(int id_mode, CancellationToken token)
        {
            //LoadSession loadSession = new LoadSession();

            try
            {
                //_loadRoot.AddSession(loadSession);
                string currentUri = $"{StartFightWithBotUri}";
                Guid id_User = _currentIdUser;

                if (id_User == Guid.Empty)
                {
                    Debug.LogError("Невозможно начать матч резарегестрированному пользователю!");
                    return;
                }

                WWWForm WWWForm = new WWWForm();
                WWWForm.AddField("id_User", id_User.ToString());
                WWWForm.AddField("id_Mode", id_mode);

                using (UnityWebRequest request = UnityWebRequest.Post(currentUri, WWWForm))
                {
                    request.certificateHandler = new BypassCertificate();
                    await request.SendWebRequest().ToUniTask(cancellationToken: token);

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        Debug.Log(request.downloadHandler.text);
                        StartFightWithBotDTO startFightWithBotDTO = JsonUtility.FromJson<StartFightWithBotDTO>(request.downloadHandler.text);


                        //_currentIdFight = Guid.Parse(request.downloadHandler.text.Trim('"'));
                        _currentIdFight = Guid.Parse(startFightWithBotDTO.id_fight);
                        _currentEnemyIdUser = Guid.Parse(startFightWithBotDTO.id_bot);

                        Debug.Log($"_currentIdFight: {_currentIdFight}");
                        Debug.Log($"_currentEnemyIdUser: {_currentEnemyIdUser}");
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
                Debug.LogError($"Ошибка {nameof(DBRoot)}-->{nameof(StartFightWithBot)}: {ex.Message}");
            }
            finally
            {
                //loadSession.Complete();
            }
        }

        public async UniTask FinishFightWithBot(bool? isYouWinner, CancellationToken token)
        {
            string currentUri = $"{FinishFightWithBotUri}";
            Guid id_Fight = _currentIdFight;
            Guid id_winner;

            if (isYouWinner == null)
            {
                id_winner = Guid.Empty;
            }
            else
            {
                id_winner = isYouWinner.Value ? _currentIdUser : _currentEnemyIdUser;
            }

            //if (id_Fight == Guid.Empty)
            //{
            //    Debug.LogError("Невозможно закончить матч пустому бою!");
            //    return;
            //}

            if (_currentIdUser == Guid.Empty || _currentEnemyIdUser == Guid.Empty)
            {
                Debug.LogError("Невозможно закончить матч пустому игроку!");
                return;
            }

            WWWForm WWWForm = new WWWForm();
            WWWForm.AddField("id_Fight", id_Fight.ToString());
            WWWForm.AddField("id_winner", id_winner.ToString());

            using (UnityWebRequest request = UnityWebRequest.Post(currentUri, WWWForm))
            {
                request.certificateHandler = new BypassCertificate();
                await request.SendWebRequest().ToUniTask(cancellationToken: token);

                //if (request.result == UnityWebRequest.Result.Success)
                //{
                //    _currentIdFight = Guid.Empty;
                //    _currentEnemyIdUser = Guid.Empty;
                //}
                //else
                //{
                //    Debug.LogError(request.error);
                //    throw new Exception();
                //}
            }
        }

        public async UniTask<int> GetLastAddedExp(CancellationToken token)
        {
            try
            {
                Guid id_user = _currentIdUser;
                Guid id_fight = _currentIdFight;

                //_currentEnemyIdUser = Guid.Empty;
                //_currentIdFight = Guid.Empty;

                string currentUri = $"{GetLastAddedExpUri}?id_user={id_user}&id_fight={id_fight}";

                using (UnityWebRequest request = UnityWebRequest.Get(currentUri))
                {
                    request.certificateHandler = new BypassCertificate();
                    await request.SendWebRequest().ToUniTask(cancellationToken: token);

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        Debug.Log(request.downloadHandler.text);

                        int result = Convert.ToInt32(request.downloadHandler.text);
  
                        return result;
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
                Debug.LogError($"Ошибка {nameof(DBRoot)}-->{nameof(GetMainMenuUserData)}: {ex.Message}");
                return -1;
            }
        }
    }
}
