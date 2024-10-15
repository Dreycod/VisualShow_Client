using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class DAO_FTP
{
    string ftpServer = "https://ftp-drey.alwaysdata.net/getEtages";
    string username;
    string password;

    public DAO_FTP()
    {
    }

    public async Task<List<string>> ListFilesAsync(string directoryPath)
    {
        List<string> fileList = new List<string>();
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri($"{ftpServer}/{directoryPath}"));
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(username, password);

            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // Filter only image files, for example, jpg, png
                    if (line.EndsWith(".jpg") || line.EndsWith(".png"))
                    {
                        fileList.Add(line);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing files: {ex.Message}");
        }

        return fileList;
    }

    public async Task<bool> DownloadFileAsync(string remoteFileName, string localFilePath)
    {
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri($"{ftpServer}/{remoteFileName}"));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UseBinary = true;

            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
            {
                await responseStream.CopyToAsync(fileStream);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
            return false;
        }
    }

    public async Task DownloadImagesFromDirectoryAsync(string directoryPath, string localDirectoryPath)
    {
        var files = await ListFilesAsync(directoryPath);
        foreach (var file in files)
        {
            string localFilePath = Path.Combine(localDirectoryPath, file);
            // Create directory if it does not exist
            Directory.CreateDirectory(localDirectoryPath);
            await DownloadFileAsync(file, localFilePath);
        }
    }
}


