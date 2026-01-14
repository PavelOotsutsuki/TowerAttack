namespace Tools.Utils.FillComponents
{
    public struct ComponentAttachInfo
    {
        public string Parent;
        public string Target;
        public int ReturnValue;

        public ComponentAttachInfo(string parent, string target, int returnValue)
        {
            Parent = parent;
            Target = target;
            ReturnValue = returnValue;
        }

        public string ComponentInfo => Target + " ....IN.... " + Parent;
    }
}