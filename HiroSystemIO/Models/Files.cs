using System;
using System.Collections.Generic;

namespace HiroSystemIO.Models
{
    public class Files
    {
        [Serializable]
        public class ItemFile
        {
            public string Extension { get; set; }
            public string Directory { get; set; }
            public long Length { get; set; }
            public string Attributes { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
        }

        [Serializable]
        public class RootFiles
        {
            public List<ItemFile> Items { get; set; }
        }
    }
}
