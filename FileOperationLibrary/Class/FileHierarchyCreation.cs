using System;
using System.Xml;
using ModelLibrary;
using System.IO;
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

        public bool CreateApplicationFolder(int appId, string categoryName, string projectName, string fileName,string bundleIdentifier,ModelLibrary.ApplicationFile appFile)
        {


            string applicationPath = rootPath + "/" + projectName + "/" + categoryName + "/" + Convert.ToString(appId);
            Console.WriteLine(applicationPath);
            CreateCategoryFolder(projectName, categoryName);
            if (FileExists(applicationPath) == false)
            {
                System.IO.Directory.CreateDirectory(applicationPath);
                if(UploadApplicationFile(applicationPath,appFile))
                {
                FileOperationLibrary.ManifestPlist manifest = new FileOperationLibrary.ManifestPlist();
                manifest.CreateManifest(applicationPath, applicationPath + "/" + fileName, bundleIdentifier, Convert.ToString(appId), projectName);
                }
                else
                {
                    Console.WriteLine("file upload error");
                    return false;
                }
                return true;


            }
            return false;

        }

        public bool UploadApplicationFile(string route,ModelLibrary.ApplicationFile appFile)
        {       
            if(appFile.files.Length >0)
            {
                try{

                    using(FileStream filestream = System.IO.File.Create(route+"/"+appFile.files.FileName))
                    {
                        appFile.files.CopyTo(filestream);
                        filestream.Flush();
                        return true;
                       
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Upload Application File catch"+e);
                }
            }
            return false;
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