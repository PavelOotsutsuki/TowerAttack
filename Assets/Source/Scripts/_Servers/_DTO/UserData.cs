namespace Servers.DTO
{
    public class UserData
    {
        private string _userName;
        private int _level;
        private int _score;

        public UserData()
        { }

        public void SetUserData(GetUserDTO userDTO)
        {
            _userName = userDTO.username;
            _level = userDTO.level;
            _score = userDTO.score;
        }

        public string UserName => _userName;
        public string Level => _level.ToString();
        public string Score => _score.ToString();
    }
}