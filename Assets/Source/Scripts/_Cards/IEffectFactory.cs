using System;
using Cysharp.Threading.Tasks;

namespace Cards
{
    public interface IEffectFactory
    {
        //public Effect Create(CardEffectConfig effectConfig, Action<int> callback);
        public void Create(CardEffectConfigPair effectConfigPair);
    }
}