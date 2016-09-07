using System;
using System.IO;

public class FileUtils
{
	public static readonly char[] FOLDER_SEPARATOR_CHARS = new char[]
	{
		'/',
		'\\'
	};

	public static string MakeSourceAssetPath(DirectoryInfo folder)
	{
		return FileUtils.MakeSourceAssetPath(folder.get_FullName());
	}

	public static string MakeSourceAssetPath(FileInfo fileInfo)
	{
		return FileUtils.MakeSourceAssetPath(fileInfo.get_FullName());
	}

	public static string MakeSourceAssetPath(string path)
	{
		string text = path.Replace("\\", "/");
		int num = text.IndexOf("/Assets", 5);
		return text.Remove(0, num + 1);
	}

	public static string MakeMetaPathFromSourcePath(string path)
	{
		return string.Format("{0}.meta", path);
	}

	public static string MakeSourceAssetMetaPath(string path)
	{
		string path2 = FileUtils.MakeSourceAssetPath(path);
		return FileUtils.MakeMetaPathFromSourcePath(path2);
	}

	public static string GameToSourceAssetPath(string path, string dotExtension = ".prefab")
	{
		return string.Format("{0}{1}", path, dotExtension);
	}

	public static string GameToSourceAssetName(string folder, string name, string dotExtension = ".prefab")
	{
		return string.Format("{0}/{1}{2}", folder, name, dotExtension);
	}

	public static string SourceToGameAssetPath(string path)
	{
		int num = path.LastIndexOf('.');
		if (num < 0)
		{
			return path;
		}
		return path.Substring(0, num);
	}

	public static string SourceToGameAssetName(string path)
	{
		int num = path.LastIndexOf('/');
		if (num < 0)
		{
			return path;
		}
		int num2 = path.LastIndexOf('.');
		if (num2 < 0)
		{
			return path;
		}
		return path.Substring(num + 1, num2);
	}

	public static string GameAssetPathToName(string path)
	{
		int num = path.LastIndexOf('/');
		if (num < 0)
		{
			return path;
		}
		return path.Substring(num + 1);
	}

	public static string GetOnDiskCapitalizationForFile(string filePath)
	{
		return filePath;
	}

	public static string GetOnDiskCapitalizationForDir(string dirPath)
	{
		DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
		return FileUtils.GetOnDiskCapitalizationForDir(dirInfo);
	}

	public static string GetOnDiskCapitalizationForFile(FileInfo fileInfo)
	{
		DirectoryInfo directory = fileInfo.get_Directory();
		string name = directory.GetFiles(fileInfo.get_Name())[0].get_Name();
		string onDiskCapitalizationForDir = FileUtils.GetOnDiskCapitalizationForDir(directory);
		return Path.Combine(onDiskCapitalizationForDir, name);
	}

	public static string GetOnDiskCapitalizationForDir(DirectoryInfo dirInfo)
	{
		DirectoryInfo parent = dirInfo.get_Parent();
		if (parent == null)
		{
			return dirInfo.get_Name();
		}
		string name = parent.GetDirectories(dirInfo.get_Name())[0].get_Name();
		string onDiskCapitalizationForDir = FileUtils.GetOnDiskCapitalizationForDir(parent);
		return Path.Combine(onDiskCapitalizationForDir, name);
	}

	public static bool GetLastFolderAndFileFromPath(string path, out string folderName, out string fileName)
	{
		folderName = null;
		fileName = null;
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		int num = path.LastIndexOfAny(FileUtils.FOLDER_SEPARATOR_CHARS);
		if (num > 0)
		{
			int num2 = path.LastIndexOfAny(FileUtils.FOLDER_SEPARATOR_CHARS, num - 1);
			int num3 = (num2 >= 0) ? (num2 + 1) : 0;
			int num4 = num - num3;
			folderName = path.Substring(num3, num4);
		}
		if (num < 0)
		{
			fileName = path;
		}
		else if (num < path.get_Length() - 1)
		{
			fileName = path.Substring(num + 1);
		}
		return folderName != null || fileName != null;
	}

	public static bool SetFolderWritableFlag(string dirPath, bool writable)
	{
		string[] files = Directory.GetFiles(dirPath);
		for (int i = 0; i < files.Length; i++)
		{
			string path = files[i];
			FileUtils.SetFileWritableFlag(path, writable);
		}
		string[] directories = Directory.GetDirectories(dirPath);
		for (int j = 0; j < directories.Length; j++)
		{
			string dirPath2 = directories[j];
			FileUtils.SetFolderWritableFlag(dirPath2, writable);
		}
		return true;
	}

	public static bool SetFileWritableFlag(string path, bool setWritable)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		try
		{
			FileAttributes attributes = File.GetAttributes(path);
			FileAttributes fileAttributes = (!setWritable) ? (attributes | 1) : (attributes & -2);
			if (setWritable && Environment.get_OSVersion().get_Platform() == 6)
			{
				fileAttributes |= 128;
			}
			bool result;
			if (fileAttributes == attributes)
			{
				result = true;
				return result;
			}
			File.SetAttributes(path, fileAttributes);
			FileAttributes attributes2 = File.GetAttributes(path);
			if (attributes2 != fileAttributes)
			{
				result = false;
				return result;
			}
			result = true;
			return result;
		}
		catch (DirectoryNotFoundException)
		{
		}
		catch (FileNotFoundException)
		{
		}
		catch (Exception)
		{
		}
		return false;
	}
}
