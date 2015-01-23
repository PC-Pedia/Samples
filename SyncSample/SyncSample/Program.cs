using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;

namespace SyncSample
{
	class Program
	{
		static void Main(string[] args)
		{
			string fileSourcePath = @"C:\Users\davamix\Downloads\pruebas\source";
			string fileTargetPath = @"C:\Users\davamix\Downloads\pruebas\target";

			//try
			//{
				FileSyncOptions options = FileSyncOptions.ExplicitDetectChanges | FileSyncOptions.RecycleDeletedFiles | FileSyncOptions.RecyclePreviousFileOnUpdates | FileSyncOptions.RecycleConflictLoserFiles;

				FileSyncScopeFilter filter = new FileSyncScopeFilter();
				filter.FileNameExcludes.Add("*.info");

				DetectChangesOnFileSystemReplica(fileSourcePath, filter, options);
				DetectChangesOnFileSystemReplica(fileTargetPath, filter, options);

				SyncFileSystemReplicaOneWay(fileSourcePath, fileTargetPath, null, options);
				SyncFileSystemReplicaOneWay(fileTargetPath, fileSourcePath, null, options);

			//}
			//catch (Exception e)
			//{
			//        Console.WriteLine("Exception for FileSync provider\n-> {0}", e.Message);
			//}

			Console.WriteLine("FIN");
			Console.Read();
		}

		private static void DetectChangesOnFileSystemReplica(string replicaPath, FileSyncScopeFilter filter, FileSyncOptions options)
		{
			FileSyncProvider provider = null;

			try
			{
				provider = new FileSyncProvider(replicaPath, filter, options);
				provider.DetectChanges();
			}
			finally
			{
				if (provider != null)
					provider.Dispose();
			}
		}

		private static void SyncFileSystemReplicaOneWay(string replicaSourcePath, string replicaTargetPath, FileSyncScopeFilter filter, FileSyncOptions options)
		{
			FileSyncProvider sourceProvider = null;
			FileSyncProvider targetProvider = null;

			try
			{
				sourceProvider = new FileSyncProvider(replicaSourcePath, filter, options);
				targetProvider = new FileSyncProvider(replicaTargetPath, filter, options);

				targetProvider.AppliedChange += new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
				targetProvider.SkippedChange += new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);

				SyncCallbacks targetCallbacks = targetProvider.DestinationCallbacks;
				targetCallbacks.ItemConflicting += new EventHandler<ItemConflictingEventArgs>(OnItemConflicting);
				targetCallbacks.ItemConstraint += new EventHandler<ItemConstraintEventArgs>(OnItemConstraint);

				SyncOrchestrator agent = new SyncOrchestrator();
				agent.LocalProvider = sourceProvider;
				agent.RemoteProvider = targetProvider;
				agent.Direction = SyncDirectionOrder.Upload;

				Console.WriteLine("Sincronizando cambios: {0}", targetProvider.RootDirectoryPath);

				agent.Synchronize();
			}
			finally
			{
				if (sourceProvider != null) { sourceProvider.Dispose(); }
				if (targetProvider != null) { targetProvider.Dispose(); }
			}
		}

		private static void OnAppliedChange(object sender, AppliedChangeEventArgs args)
		{
			switch (args.ChangeType)
			{
				case ChangeType.Create:
					Console.WriteLine("Applied CREATE for file {0}", args.NewFilePath);
					break;
				case ChangeType.Delete:
					Console.WriteLine("Applied DELETE for file {0}", args.OldFilePath);
					break;
				case ChangeType.Update:
					Console.WriteLine("Applied UPDATE for file {0}", args.OldFilePath);
					break;
				case ChangeType.Rename:
					Console.WriteLine("Applied RENAME for file {0} as {1}", args.OldFilePath, args.NewFilePath);
					break;
			}
		}

		private static void OnSkippedChange(object sender, SkippedChangeEventArgs args)
		{
			Console.WriteLine("Skipped applying {0} for {1} due to error", args.ChangeType.ToString().ToUpper(), (!string.IsNullOrEmpty(args.CurrentFilePath) ? args.CurrentFilePath : args.NewFilePath));

			if(args.Exception != null)
				Console.WriteLine("--> {0}", args.Exception.Message);
		}

		private static void OnItemConflicting(object sender, ItemConflictingEventArgs args)
		{
			args.SetResolutionAction(ConflictResolutionAction.Merge);
			Console.WriteLine("Concurrency conflict detected form item {0}", args.DestinationChange.ItemId.ToString());
		}

		private static void OnItemConstraint(object sender, ItemConstraintEventArgs args)
		{
			args.SetResolutionAction(ConstraintConflictResolutionAction.SourceWins);
			Console.WriteLine("Constraint conflict detected for item {0}", args.DestinationChange.ItemId.ToString());
		}

	}
}
