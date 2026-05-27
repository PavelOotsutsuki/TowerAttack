using System;

namespace Servers.DTO
{
    [Serializable]
    public class LogInDTO
    {
        public string id_User;
        public string dte_last_login;  // DateTime заменил на string, об этом ниже
    }
}