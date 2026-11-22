using UnityEngine;

namespace Cards.Sounds
{
    public class CardSoundConfigDB : ICardSoundConfig
    {
        // private readonly string _apiUrl = "https://localhost:7206";
        private readonly AudioClip[] _sounds;
        public CardSoundConfigDB(AudioClip[] sounds)
        {
            _sounds = sounds;
            // string fullUrl = _apiUrl + "/currentCards/" + url + "/image.jpeg";

            // int counter = 1;

            // try
            // {
            //     while (true)
            //     {
            //         fullUrl = $"{_apiUrl}/currentCards/{url}/sound{counter}.jpeg";


            //         using var request = UnityWebRequest.Get(fullUrl);
            //         request.SendWebRequest().GetAwaiter().GetResult();
            //         byte[] audioBytes = request.downloadHandler.data;

            //         // Превращаем в AudioClip
            //         UnityWebRequestMultimedia.GetAudioClip(fullUrl, AudioType.WAV)
            //         AudioClip clip = WavUtility.ToAudioClip(audioBytes);
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Debug.Log($"Ошибка загрузки аудио файла: {ex.Message}");
            // }

        }
        public AudioClip[] Sounds => _sounds;
    }
}