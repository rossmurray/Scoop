using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scoop
{
	public class FileWatcher : IFileWatcher, IDisposable
	{
		public event EventHandler FileChanged;

		private FileSystemWatcher fsWatcher;
		private FileInfo file;
		private bool disposed;
		private object startStopLock = new object();
		private object fileChangedLock = new object();
		private DateTime fileChangedWhen;

		public FileWatcher(FileInfo file)
		{
			if (file == null) throw new ArgumentNullException("file");
			this.file = file;
			var fsWatcher = new FileSystemWatcher();
			fsWatcher.NotifyFilter = NotifyFilters.LastWrite;
			fsWatcher.Path = file.DirectoryName;
			fsWatcher.Filter = file.Name;
			fsWatcher.Changed += FileChangedHandler;
			this.fsWatcher = fsWatcher;
		}

		public void Start()
		{
			lock (this.startStopLock)
			{
				if (this.disposed) throw new ObjectDisposedException(typeof(FileWatcher).FullName);
				this.fsWatcher.EnableRaisingEvents = true;
			}
		}

		public void Stop()
		{
			lock (this.startStopLock)
			{
				if (this.disposed) throw new ObjectDisposedException(typeof(FileWatcher).FullName);
				this.fsWatcher.EnableRaisingEvents = false;
			}
		}

		private void FileChangedHandler(object sender, FileSystemEventArgs e)
		{
			lock (this.fileChangedLock)
			{
				var executeCallback = false;
				try
				{
					Thread.Sleep(200);
					using(var s = this.file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						//var now = DateTime.UtcNow;
						//if (modified > this.fileChangedWhen)
						//{
						//	this.fileChangedWhen = DateTime.UtcNow;
							
						//}
						executeCallback = true;
						Console.WriteLine(this.file.LastWriteTimeUtc.ToString("O"));
					}
				}
				catch (IOException ex)
				{
					Console.WriteLine("IOEception: " + ex);
				}
				if (executeCallback)
				{
					NotifySubscribers();
				}
			}
		}

		private void NotifySubscribers()
		{
			var subs = this.FileChanged;
			if(subs != null)
			{
				subs(this, EventArgs.Empty);
			}
		}

		public void Dispose()
		{
			if (this.disposed) return;
			this.disposed = true;
			this.fsWatcher.EnableRaisingEvents = false;
			this.fsWatcher.Changed -= FileChangedHandler;
			this.fsWatcher.Dispose();
		}
	}
}
