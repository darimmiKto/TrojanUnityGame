using System;
using System.Collections.Generic;

namespace HiroSystemIO.Models
{
    public class Directories
    {
        [Serializable]
        public class ItemDirectories
        {
            public string Parent { get; set; }
            public string Root { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
        }

        [Serializable]
        public class RootDirectories
        {
            public List<ItemDirectories> Items { get; set; }
        }
    }
}
