using System;
using System.Text.Json.Serialization;

namespace Servers.DTO
{
    //[Serializable]
    //public class UserDTO
    //{
    //    //[JsonPropertyName("id")]
    //    public int Id;

    //    //[JsonPropertyName("username")]
    //    public string Username;

    //    //[JsonPropertyName("score")]
    //    public int Score;

    //    //[JsonPropertyName("level")]
    //    public int Level;

    //    //[JsonPropertyName("last_login")]
    //    public string LastLogin;
    //}

    [Serializable]
    public class GetMainMenuUserDataDTO
    {
        //public int id;
        public string username;
        //public string password;
        public int score;
        public int level;
        public int max_experience;
        //public string lastLogin;  // DateTime заменил на string, об этом ниже
    }
}