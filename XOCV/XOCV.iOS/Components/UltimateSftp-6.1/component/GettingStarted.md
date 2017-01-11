There are two Ultimate SFTP components for use with Xamarin, Ultimate SFTP for iOS and Ultimate SFTP for Android. Both use the same API.

Ultimate SFTP for .NET is a 100%-managed .NET class library that allows you to easily add SSH Secure File Transfer (SFTP) capabilities to your .NET and ASP.NET applications. It provides access to the complete functionality of the FTP protocol through a straightforward, intuitive object model. The library offers the flexibility, ease of use and rapid development features of a component without the complexities of working with the native socket class or in-depth knowledge of how the Secure File Transfer Protocols are implemented. It also provides a new way of working with files and folders based on ComponentPro's own powerful FileSystem classes. More flexible and object-oriented, this approach lets developers manipulate filesystem elements on the SFTP server just as easily as if they were local files and folders.

## Generating your Trial License Key
To use the component, please generate your 30-day trial key at: (http://www.componentpro.com/download/trial.aspx?product=Sftp)

This document provides a short guide to getting started.

Xamarin.iOS
-----------

<b>Upload multiple files to an SFTP server in an iOS application</b> 

In this getting started example we'll connect to an SFTP server, authenticate the user and download files to the server.

	using ComponentPro.Net;
	using ComponentPro.IO;

	...

	// Create a new instance.
	Sftp client = new Sftp();

	// Connect to the SFTP server.
	client.Connect("myserver");

	// Authenticate.
	client.Authenticate("userName", "password");

	// ... 
	 
	// Get all directories, subdirectories, and files from remote folder '/myfolder' to 'c:\myfolder'.
	client.DownloadFiles("/myfolder", "c:\\myfolder");

	// Get all directories, subdirectories, and files that match the specified search pattern from remote folder '/myfolder2' to 'c:\myfolder2'.
	client.DownloadFiles("/myfolder2", "c:\\myfolder2", "*.cs");

	// or you can simply put wildcard masks in the source path, our component will automatically parse it. 
	// download all *.css files from remote folder '/myfolder2' to local folder 'c:\myfolder2'.
	client.DownloadFiles("/myfolder2/*.css", "c:\\myfolder2");

	// Download *.cs and *.vb files from remote folder '/myfolder2' to local folder 'c:\myfolder2'.
	client.DownloadFiles("/myfolder2/*.cs;*.vb", "c:\\myfolder2");

	// Get files in the folder '/myfolder2' only.
	TransferOptions opt = new TransferOptions(true, RecursionMode.None, false, (SearchCondition)null, FileExistsResolveAction.OverwriteAll, SymlinksResolveAction.Skip);
	client.DownloadFiles("/myfolder2", "c:\\myfolder2", opt);

	// ... 
	 
	// Disconnect.
	client.Disconnect();


Xamarin.Android
---------------

<b>Deleting remote directory on an SFTP server in an Android application</b> 

	using System;
	using ComponentPro;
	using ComponentPro.IO;
	using ComponentPro.Net;

	...

	// Create a new instance.
	Sftp client = new Sftp();

	// Connect to the SFTP server.
	client.Connect("localhost");

	// Authenticate.
	client.Authenticate("test", "test");

	// ... 
	 
	// Remove an empty directory. If you wish to remove an entire directory, simply add a boolean value of true as the second parameter to the DeleteDirectoryAsync method. 
	await client.DeleteDirectoryAsync("/temp");

	// ... 
	 
	// Remove all .tmp files recursively. Empty directories are to be removed as well. 
	await client.DeleteDirectoryAsync("/test", true, "*.tmp");  

	// ... 
	 
	// Disconnect.
	client.Disconnect();


## Other Resources

* [ComponentPro Support forums](http://www.componentpro.com/forums/)
* [Product Documentation](http://www.componentpro.com/doc/sftp/)
* [Product Page](http://www.componentpro.com/sftp.netcf/)
