﻿using System;
using System.IO;

namespace Photon
{
    public class FileLoader : ContentLoader
    {
        string _path;

        string NormalizeFileName(string filename)
        {
            filename = filename.ToLower();

            string final;

            if (Path.IsPathRooted(filename))
            {
                if (filename.IndexOf(_path) == 0)
                {
                    final = filename.Substring(_path.Length);
                }
                else
                {
                    throw new Exception("file should under PHOPATH");
                }
            }
            else
            {
                final = filename;
            }

            return final.Replace('\\', '/');
        }

        // 工作路径
        public FileLoader(string phoPath)
        {
            _path = phoPath.ToLower() + "/";
        }

        public override void Load(string sourceName, ImportMode mode)
        {
            switch (mode)
            {
                case ImportMode.Directory:
                    {
                        var files = Directory.GetFiles(sourceName, "*.pho", SearchOption.TopDirectoryOnly);

                        foreach (var filename in files)
                        {
                            Load(filename, ImportMode.File);
                        }
                    }
                    break;
                case ImportMode.File:
                    {
                        var content = System.IO.File.ReadAllText(sourceName);

                        AddSource(content, NormalizeFileName(sourceName));
                    }
                    break;
            }
        }
    }
}