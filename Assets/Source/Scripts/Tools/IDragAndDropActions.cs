namespace Tools
{
    public interface IDragAndDropActions
    {
        public void StartDrag();
        public bool TryDrop();
        public void Drop();
        public void StartEndDragActions();
    }
}