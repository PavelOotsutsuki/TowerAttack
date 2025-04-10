namespace GameFields.Persons.SelectMenues
{
    public class SelectResult
    {
        private bool _isSelectSuccess;

        public SelectResult()
        {
            _isSelectSuccess = false;
        }

        public bool IsSelectSuccess => _isSelectSuccess;

        public void SuccessChoice()
        {
            _isSelectSuccess = true;
        }
    }
}