using System;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Files
    {
        public DateTime CreationTime;
        public DateTime LastAccessTime;
        public DateTime LastWriteTime;

        public string Extension;
        public string Directory;
        public long Length;
        public string Attributes;
        public string Name;
        public string FullName;
    }
}
