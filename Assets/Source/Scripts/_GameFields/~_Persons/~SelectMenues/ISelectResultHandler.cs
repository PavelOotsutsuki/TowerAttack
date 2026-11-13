using Tools;

namespace GameFields.Persons.SelectMenues
{
    public interface ISelectResultHandler: ICompletable
    {
        void SetResult(SetSelectResultData data);
    }
}