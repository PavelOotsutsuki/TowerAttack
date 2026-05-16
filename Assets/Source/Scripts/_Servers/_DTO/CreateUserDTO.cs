using System;

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
    public class CreateUserDTO
    {
        //public int id;
        public string username;
        public string password;
        //public int score;
        //public int level;
        //public string lastLogin;  // DateTime заменил на string, об этом ниже
    }
}