using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Servers.DTO;
using UnityEngine;
using UnityEngine.Networking;

namespace Servers.Loaders
{
    public class AudioLoader
    {
        private const string WarehouseUri = "https://localhost:7206";
        private const string CardsUrlFeature = "currentCards";
        private readonly string _diskCachePath;
        private readonly LoaderHelper<AudioClip> _loaderHelper;
        private readonly Dictionary<string, AudioClip[]> _cachedClipsData = new Dictionary<string, AudioClip[]>();

        public AudioLoader()
        {
            _diskCachePath = Path.Combine(Application.persistentDataPath, "AudioCache");
            Directory.CreateDirectory(_diskCachePath);
            Debug.Log($"CAHCE DIRECTORY: {_diskCachePath}");

            _loaderHelper = new LoaderHelper<AudioClip>(".wav", _diskCachePath, TryFindInDisk, GetFileByWebRequest, "Audio", GetUnityWebRequestByUrl);
        }

        public async UniTask<AudioClip[]> GetClipsAsync(StringIntPair urlData, CancellationToken token)
        {
            if (_cachedClipsData.ContainsKey(urlData.key))
                return _cachedClipsData[urlData.key];

            string fullUrl = "";
            AudioClip clip;
            AudioClip[] audioClips = new AudioClip[urlData.value];

            try
            {

                for (int counter = 1; counter <= urlData.value; counter++)
                {
                    fullUrl = $"{WarehouseUri}/{CardsUrlFeature}/{urlData.key}/sound{counter}.wav";
                    clip = await _loaderHelper.GetFileAsync(fullUrl, token);
                    audioClips[counter - 1] = clip;

                    Debug.Log($"НАЙДЕН И СКАЧЕН: {fullUrl}");
                    Debug.Log($"clips.Count: {counter}");
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"NOT FOUND: {fullUrl}; {ex.Message}");
            }

            if (_cachedClipsData.ContainsKey(urlData.key) == false)
                _cachedClipsData.Add(urlData.key, audioClips);

            return audioClips;
        }

        private async UniTask<AudioClip> TryFindInDisk(string localPath, CancellationToken token)
        {
            using UnityWebRequest localReq = UnityWebRequestMultimedia.GetAudioClip("file://" + localPath, AudioType.WAV);
            await localReq.SendWebRequest().ToUniTask(cancellationToken: token);

            if (localReq.result == UnityWebRequest.Result.Success)
            {
                File.Delete(localPath);
                return null;
            }

            return DownloadHandlerAudioClip.GetContent(localReq);
        }

        private UnityWebRequest GetUnityWebRequestByUrl(string url)
        {
            return UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        }

        private AudioClip GetFileByWebRequest(UnityWebRequest req)
        {
            return DownloadHandlerAudioClip.GetContent(req);
        }
    }
}