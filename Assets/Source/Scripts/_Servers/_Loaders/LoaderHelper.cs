using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Servers.Loaders
{
    internal class LoaderHelper<T> where T : class
    {
        // private readonly Dictionary<string, T> _memoryCache = new Dictionary<string, T>();
        private readonly Dictionary<string, UniTask<T>> _loadingTasks = new Dictionary<string, UniTask<T>>(); // защита от дублей
        private readonly string _extension;
        private readonly string _diskCachePath;
        private readonly Func<string, CancellationToken, UniTask<T>> _tryFindInDiskMethod;
        private readonly Func<UnityWebRequest, T> _getFileByWebRequestMethod;
        private readonly Func<string, UnityWebRequest> _getUnityWebRequestByUrlMethod;
        private readonly string _entityStringType;

        public LoaderHelper(string extension, string diskCachePath, Func<string, CancellationToken, UniTask<T>> tryFindInDiskMethod,
        Func<UnityWebRequest, T> getFileByWebRequestMethod, string entityStringType, Func<string, UnityWebRequest> getUnityWebRequestByUrlMethod)
        {
            _extension = extension;
            _diskCachePath = diskCachePath;
            _tryFindInDiskMethod = tryFindInDiskMethod;
            _getFileByWebRequestMethod = getFileByWebRequestMethod;
            _entityStringType = entityStringType;
            _getUnityWebRequestByUrlMethod = getUnityWebRequestByUrlMethod;
        }

        public async UniTask<T> GetFileAsync(string url, CancellationToken token)
        {
            Debug.Log($"НАЧАЛО {url}");
            // 1. Память — по URL
            // if (_memoryCache.TryGetValue(url, out T cached))
            // {
            //     Debug.Log($"ПОЛУЧЕНО ИЗ КЭША {url}");
            //     return cached;
            // }

            // 2. Уже качается?
            if (_loadingTasks.TryGetValue(url, out UniTask<T> existingTask))
            {
                Debug.Log($"ПОЛУЧЕНО ИЗ _loadingTasks {url}");
                return await existingTask;
            }

            // 3. Загружаем
            UniTask<T> task = LoadInternalAsync(url, token);
            _loadingTasks[url] = task;

            try
            {
                T file = await task;
                // _memoryCache[url] = file;
                Debug.Log($"ПОЛУЧЕНО ИЗ СЕРВЕРА {url}");
                return file;
            }
            finally
            {
                _loadingTasks.Remove(url);
            }
        }

        private async UniTask<T> LoadInternalAsync(string url, CancellationToken token)
        {
            string fileName = HashUrlToFileName(url);
            string localPath = Path.Combine(_diskCachePath, fileName);

            // 1. Диск
            if (File.Exists(localPath))
            {
                T file = await _tryFindInDiskMethod.Invoke(localPath, token);

                if (file != null)
                    return file;
            }

            // 2. Сервер
            using UnityWebRequest req = _getUnityWebRequestByUrlMethod.Invoke(url);
            await req.SendWebRequest().ToUniTask(cancellationToken: token);

            if (req.result != UnityWebRequest.Result.Success)
                throw new Exception($"{_entityStringType} load failed: {req.error}");

            T downloaded = _getFileByWebRequestMethod.Invoke(req);
            await File.WriteAllBytesAsync(localPath, req.downloadHandler.data, token);
            return downloaded;
        }

        private string HashUrlToFileName(string url)
        {
            // Простой способ: заменить недопустимые символы
            // Или использовать SHA256 от URL
            using SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(url));
            return BitConverter.ToString(hash).Replace("-", "") + _extension;
        }
    }
}