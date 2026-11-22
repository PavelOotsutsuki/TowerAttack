using System;

namespace Servers.DTO
{
    [Serializable]
    public class FightProcessDBActionDTO
    {
        public string counter;
        public string turn;
        public string id_fight;
        public string person_id;
        public string action_target;
        public string action_type;
        public string action_subtype;
    }
}