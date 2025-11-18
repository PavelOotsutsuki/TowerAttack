using Tools;

namespace GameFields.Persons.SelectMenues.Commons
{
    public interface ISelectResultHandler: ICompletable
    {
        void SetResult(SetSelectResultData data);
    }
}