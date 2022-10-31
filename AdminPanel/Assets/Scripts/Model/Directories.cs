using System;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Directories
    {
        public DateTime CreationTime;
        public DateTime LastAccessTime;
        public DateTime LastWriteTime;

        public string Parent;
        public string Root;

        public string Name;
        public string FullName;
    }
}
