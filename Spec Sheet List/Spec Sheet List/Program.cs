using System;
using System.IO;
using System.Linq;

namespace ListFile
{
    class Program
    {
        static string line;

        static void Main(string[] args)
        {
            line = "Customer,Pattern,Approved,Folder Location,File Name,Date Last Modified,Date Created" + Environment.NewLine;
            string inPath = "P:\\Accumark Spec Sheets";
            string outPath = @"G:\\DTS\\in\\Spec_Location_File.csv";

            getFolder(inPath);

            File.WriteAllText(outPath, line);

            return;
        }

        static void getFolder(string inPath)
        {
            bool approved = true;
            bool watchCus = false;
            string customer = "-";

            string fileName;
            string[] filePath;

            try
            {
                filePath = inPath.Split('\\');

                foreach (string _folder in filePath)
                {
                    // The "NOT APPROVED" folder does not universally preceed the target directory
                    if (_folder == "NOT APPROVED") approved = false;

                    // The Customer directory follows the "Accumark Spec Sheets" folder.
                    if (watchCus)
                    {
                        customer = _folder;
                        watchCus = false;
                    }

                    if (_folder == "Accumark Spec Sheets") watchCus = true;
                }

                foreach (string _file in Directory.GetFiles(inPath))
                {
                    fileName = _file.Split('\\').Last();

                    line += customer + ",";                                     // Customer
                    line += fileName.TrimStart(' ').Split(' ').First() + ",";   // Pattern
                    line += approved + ",";                                     // Approval
                    line += inPath + ",";                                       // Folder Location
                    line += fileName + ",";                                     // File Name
                    line += File.GetLastWriteTime(_file) + ",";                 // Modifiction Date
                    line += File.GetCreationTime(_file);                        // Creation Date
                    line += Environment.NewLine;
                }

                foreach (string _dir in Directory.GetDirectories(inPath))
                {
                    getFolder(_dir);
                }
            }
            catch (System.Exception e) { }

            return;
        }
    }
}