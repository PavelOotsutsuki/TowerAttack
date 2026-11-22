using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Servers.Loaders
{
    public class ImageLoader
    {
        private const string WarehouseUri = "https://localhost:7206";
        private const string CardsUrlFeature = "currentCards";
        private readonly string _diskCachePath;
        private readonly LoaderHelper<Texture2D> _loaderHelper;

        private readonly Dictionary<string, Sprite> _createdSprites = new Dictionary<string, Sprite>();

        public ImageLoader()
        {
            _diskCachePath = Path.Combine(Application.persistentDataPath, "ImageCache");
            Directory.CreateDirectory(_diskCachePath);
            Debug.Log($"CAHCE DIRECTORY: {_diskCachePath}");

            _loaderHelper = new LoaderHelper<Texture2D>(".png", _diskCachePath, TryFindInDisk, GetFileByWebRequestMethod, "Image", GetUnityWebRequestByUrl);
        }

        public async UniTask<Sprite> GetImageAsync(string url, CancellationToken token)
        {
            if (_createdSprites.ContainsKey(url))
                return _createdSprites[url];

            string fullUrl = $"{WarehouseUri}/{CardsUrlFeature}/{url}/image.png";

            Texture2D texture = await _loaderHelper.GetFileAsync(fullUrl, token);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);

            if (_createdSprites.ContainsKey(url) == false)
                _createdSprites[url] = sprite;

            return sprite;
        }

        private async UniTask<Texture2D> TryFindInDisk(string localPath, CancellationToken token)
        {
            byte[] bytes = await File.ReadAllBytesAsync(localPath, token);
            Texture2D texture = new Texture2D(2, 2);

            if (texture.LoadImage(bytes) == false)
            {
                texture = null;
                File.Delete(localPath); // битый файл
            }

            return texture;
        }

        private UnityWebRequest GetUnityWebRequestByUrl(string url)
        {
            return UnityWebRequestTexture.GetTexture(url);
        }

        private Texture2D GetFileByWebRequestMethod(UnityWebRequest req)
        {
            return DownloadHandlerTexture.GetContent(req);
        }
    }
}