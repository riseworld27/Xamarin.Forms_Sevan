Optimized for speed and minimal memory usage, the purely written in C# Ultimate SFTP component is proudly the best SFTP component for Xamarin Android, iOS and .NET Compact Framework. With just a few lines of code, your application is ready to connect, upload and download files and directories to and from the SFTP server.

The example below shows how to connect to an SFTP server and upload multiple files.

	using ComponentPro.Net;

	...

	// Create a new instance.
	Sftp client = new Sftp();

	// Connect to the SFTP server.
	client.Connect("localhost");

	// Authenticate.
	client.Authenticate("test", "test");

	// ... 
	 
	// Upload all files and subdirectories from local folder 'temp' to the remote dir '/temp'
	client.UploadFiles("temp", "/temp");

	// Upload all directories, subdirectories, and files that match the specified search pattern from local folder 'myfolder2' to remote folder '/myfolder2'.
	client.UploadFiles("myfolder2", "/myfolder2", "*.cs");

	// or you can simply put wildcard masks in the source path, our component will automatically parse it. 
	// upload all *.css files from local folder 'myfolder2' to remote folder '/myfolder2'.
	client.UploadFiles("myfolder2\\*.css", "/myfolder2");

	// Upload *.cs and *.vb files from local folder 'myfolder2' to remote folder '/myfolder2'.
	client.UploadFiles("myfolder2\\*.cs;*.vb", "/myfolder2");

	// ... 
	 
	// Disconnect.
	client.Disconnect();