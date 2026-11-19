using System;

namespace Servers.DTO
{
    [Serializable]
    public class GetMainMenuUserDataDTO
    {
        public string username;
        public int score;
        public int level;
        public int max_experience;
    }
}