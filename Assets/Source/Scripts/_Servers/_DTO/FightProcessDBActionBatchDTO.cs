using System;
using System.Collections.Generic;

namespace Servers.DTO
{
    [Serializable]
    public class FightProcessDBActionBatchDTO
    {
        public List<FightProcessDBActionDTO> batchList;
    }
}