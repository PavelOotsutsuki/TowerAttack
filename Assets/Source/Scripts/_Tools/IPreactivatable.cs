using Cysharp.Threading.Tasks;
using System.Threading;

namespace Tools
{
    public interface IPreactivatable: IActivatable  // Интерфейс для загрузки данных заранее ДО активации
    {
        public bool IsPreactive { get; }

        public UniTask Preactivate(CancellationToken token);
    }

    public interface IPreactivatable<R> where R : IData
    {
        public void Preactivate(R data);
    }
}