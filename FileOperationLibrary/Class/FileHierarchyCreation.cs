using System;
using System.Xml;
using ModelLibrary;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace FileOperationLibrary
{
    public class FileHierarchyCreation
    {
        string rootPath,ipaRootPath;
        public FileHierarchyCreation()
        {
            ipaRootPath = ConfigurationManager.AppSettings["iparootpath"];
            rootPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Main").Replace('\\','/');
           // Console.WriteLine(rootPath);

        }
        public void CreateProjectFolder(string fileName, ProjectIcon icon)
        {
            string pathString = System.IO.Path.Combine(rootPath, fileName);
            if (FileExists(pathString) == false)
            {
                System.IO.Directory.CreateDirectory(pathString);
                Directory.CreateDirectory(Path.Combine(pathString, "Assets"));
                uploadProjectIcon(Path.Combine(pathString, "Assets"), icon);
            }
        }

        private bool uploadProjectIcon(string route, ProjectIcon icon)
        {
            if (icon.Icon.Length > 0)
            {
                try
                {
                    using (FileStream filestream = System.IO.File.Create(route + "/" + "icon.png"))
                    {
                        icon.Icon.CopyTo(filestream);
                        filestream.Flush();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Upload Icon File catch" + e);
                }
            }
            return false;
        }

        public bool CreateApplicationFolder(int appId, string categoryName, string projectName, string fileName, string bundleIdentifier, ApplicationFile appFile)
        {

            string applicationPath = rootPath + "/" + projectName + "/" + categoryName + "/" + Convert.ToString(appId);
            string ipaPath = ipaRootPath+ "/" + projectName + "/" + categoryName + "/" + Convert.ToString(appId);
           // Console.WriteLine(applicationPath);
            CreateCategoryFolder(projectName, categoryName);
            if (FileExists(applicationPath) == false)
            {
                System.IO.Directory.CreateDirectory(applicationPath);
                if (UploadApplicationFile(applicationPath, appFile))
                {
                    FileOperationLibrary.ManifestPlist manifest = new FileOperationLibrary.ManifestPlist();
                    manifest.CreateManifest(applicationPath, ipaPath + "/" + appFile.File.FileName, bundleIdentifier, Convert.ToString(appId), projectName);
                }
                else
                {
                   // Console.WriteLine("file upload error");
                    return false;
                }
                return true;


            }
            return false;

        }

        public bool UploadApplicationFile(string route, ApplicationFile appFile)
        {
            if (appFile.File.Length > 0)
            {
                try
                {
                    using (FileStream filestream = System.IO.File.Create(route + "/" + appFile.File.FileName))
                    {
                        appFile.File.CopyTo(filestream);
                        filestream.Flush();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Upload Application File catch" + e);
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

        public void EditProjectIcon(string route, EditProjectIcon icon)
        {
            try
            {
                if (icon.Icon.Length > 0)
                {
                    //Console.WriteLine("not null");
                    File.Delete(route + "/Assets/" + "icon.png");
                    try
                    {
                        using (FileStream filestream = System.IO.File.Create(route + "/Assets/" + "icon.png"))
                        {
                            icon.Icon.CopyTo(filestream);
                            filestream.Flush();
                           
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Edit icon File catch" + e);
                    }
                }
            }
            catch (Exception)//icon file null exception 
            {
               // Console.WriteLine("null");
            }
        }
        public void EditProjectFolder(string oldProjectName, string newProjectName, string bundleIdentifier, EditProjectIcon icon)
        {
            string[] dirs = System.IO.Directory.GetDirectories(rootPath);
            char[] spearator = { '\\' };

            try
            {
                foreach (var dir in dirs)
                {

                    if (dir.Split(spearator)[1] == oldProjectName)
                    {
                        //Console.WriteLine("Edited :" + dir.Split(spearator)[1]);
                        EditProjectIcon(rootPath + "/" + oldProjectName, icon);
                        System.IO.Directory.Move(rootPath + "/" + oldProjectName, rootPath + "/" + newProjectName);
                        break;
                    }

                }
            }
            catch (Exception)
            {
               //Console.WriteLine("old and new project names are same");
                //  Console.WriteLine("icon "+icon.Icon.Length);
            }
            string[] categoryDirs = System.IO.Directory.GetDirectories(rootPath + "/" + newProjectName);
            foreach (var category in categoryDirs)
            {

                string[] appDirs = System.IO.Directory.GetDirectories(category);
                foreach (var app in appDirs)
                {
                    //Console.WriteLine(app);
                    string[] appFile = Directory.GetFiles(app, "*.ipa");
                    if (appFile.Length == 1)
                    {
                        string[] details = appFile[0].Split(spearator);
                        File.Delete(app + "/manifest.plist");
                        string ipaFilePath="";
                        string toBeSearched = "Main";
                        int ix = appFile[0].IndexOf(toBeSearched);
                        if (ix != -1) 
                        {
                         ipaFilePath = ipaRootPath+appFile[0].Substring(ix + toBeSearched.Length);
                        }
                        string tempId = appFile[0].Substring(ix + toBeSearched.Length);
                        FileOperationLibrary.ManifestPlist manifest = new FileOperationLibrary.ManifestPlist();
                        manifest.CreateManifest(app.Replace('\\', '/'), ipaFilePath.Replace('\\', '/'), bundleIdentifier, Convert.ToString(tempId.Split(spearator)[2]), newProjectName);
                
                    }


                }
            }
        }



        public void EditCategoryFolder(string oldCategoryName, string newCategoryName)
        {
            string[] dirs = System.IO.Directory.GetDirectories(rootPath);
            char[] separator = { '\\' };
            foreach (var dir in dirs)
            {
                string[] categoryDirs = System.IO.Directory.GetDirectories(dir);
                foreach (var category in categoryDirs)
                {
                    if (category.Split(separator)[2] == oldCategoryName)
                    {
                        Directory.Move(category, dir + "/dummy");
                        Directory.Move(dir + "/dummy", dir + "/" + newCategoryName);
                        string[] appDirs = Directory.GetDirectories(dir + "/" + newCategoryName);
                        foreach (var app in appDirs)
                        {

                            string[] appFile = Directory.GetFiles(app, "*.ipa");
                            if (appFile.Length == 1)
                            {
                                string[] details = appFile[0].Split(separator);
                                FileOperationLibrary.ManifestPlist manifest = new FileOperationLibrary.ManifestPlist();
                                manifest.EditManifest(app, ipaRootPath+"/"+details[1]+"/"+details[2]+"/"+details[3]);

                            }

                        }
                    }
                }
            }

        }

        public void DeleteProjectFolder(string projectName)
        {
            string[] dirs = System.IO.Directory.GetDirectories(rootPath);
            char[] spearator = { '\\' };
            foreach (var dir in dirs)
            {

                if (dir.Split(spearator)[1] == projectName)
                {
                    //Console.WriteLine("Deleted project "+ dir.Split(spearator)[1]);
                    System.IO.Directory.Delete(rootPath + "/" + projectName, true);
                }

            }
        }

        public void DeleteCategoryFolder(string categoryName)
        {
            string[] dirs = System.IO.Directory.GetDirectories(rootPath);
            char[] separator = { '\\' };
            foreach (var dir in dirs)
            {
                string[] categoryDirs = System.IO.Directory.GetDirectories(dir);
                foreach (var category in categoryDirs)
                {
                    if (category.Split(separator)[2] == categoryName)
                    {
                        //Console.WriteLine(category);
                        Directory.Delete(category, true);

                    }
                }
            }
        }

        public void DeleteSpecificCategoryFolder(string projectName, string categoryName)
        {
            string[] dirs = System.IO.Directory.GetDirectories(rootPath);
            char[] separator = { '\\' };
            foreach (var dir in dirs)
            {
                if (dir.Split(separator)[1] == projectName)
                {
                    string[] categoryDirs = System.IO.Directory.GetDirectories(dir);
                    foreach (var category in categoryDirs)
                    {
                        if (category.Split(separator)[2] == categoryName)
                        {
                            //Console.WriteLine(category);
                            Directory.Delete(category, true);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void DeleteApplicationFolder(int appId)
        {
            string[] dirs = System.IO.Directory.GetDirectories(rootPath);
            char[] separator = { '\\' };
            foreach (var dir in dirs)
            {
                string[] categoryDirs = System.IO.Directory.GetDirectories(dir);
                foreach (var category in categoryDirs)
                {


                    string[] appDirs = Directory.GetDirectories(category);

                    foreach (var app in appDirs)
                    {
                        //Console.WriteLine(app);
                        string[] details = app.Split(separator);

                        if (int.Parse(details[3]) == appId)
                        {
                            //Console.WriteLine(details[0]+" "+details[1]+" "+details[2]+" "+details[3]);
                            Directory.Delete(app, true);
                        }
                        appDirs = Directory.GetDirectories(category);
                        if (appDirs.Length == 0)
                        {
                            Directory.Delete(category, true);
                        }

                    }
                }
            }

        }

        public bool FileExists(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                // Console.WriteLine("folder exist");
                return true;
            }
            return false;
        }
    }
}