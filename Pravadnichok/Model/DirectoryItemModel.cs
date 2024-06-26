﻿using Pravadnichok.FileExplorer;

namespace Pravadnichok.Model
{
    public class DirectoryItemModel
    {
        public DirectoryItemType Type { get; set; }

        public string FullPath { get; set; }

        public string Name
        {
            get
            {
                return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFileFolderName(this.FullPath);
            }
        }
    }
}
