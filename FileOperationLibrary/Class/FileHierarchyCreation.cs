using System;
using System.Xml;
namespace FileOperationLibrary
{
    public class FileHierarchyCreation
    {
        string rootPath;
        public FileHierarchyCreation()
        {
            rootPath = @"C:/Users/Workstation/Desktop/AppConsole/Main";
        }
        public void CreateProjectFolder(string fileName)
        {
            string pathString = System.IO.Path.Combine(rootPath, fileName);
            if (FileExists(pathString) == false)
            {
                System.IO.Directory.CreateDirectory(pathString);
            }
        }

        public void CreateApplicationFolder(int appId, string categoryName, string projectName, string fileName,string bundleIdentifier)
        {


            string applicationPath = rootPath + "/" + projectName + "/" + categoryName + "/" + Convert.ToString(appId);
            Console.WriteLine(applicationPath);
            CreateCategoryFolder(projectName, categoryName);
            if (FileExists(applicationPath) == false)
            {
                System.IO.Directory.CreateDirectory(applicationPath);
                FileOperationLibrary.ManifestPlist manifest = new FileOperationLibrary.ManifestPlist();
                manifest.CreateManifest(applicationPath, applicationPath + "/" + fileName, bundleIdentifier, Convert.ToString(appId), projectName);

            }

        }


        public void CreateCategoryFolder(string projectName, string categoryName)
        {
            string categoryPath = rootPath + "/" + projectName + "/" + categoryName;
            if (FileExists(categoryPath) == false)
            {
                System.IO.Directory.CreateDirectory(categoryPath);
            }
        }

        public bool FileExists(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                Console.WriteLine("folder exist");
                return true;
            }
            return false;
        }


    }
}