using CommitTemplateExtension.ViewModels;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using Commands = LibGit2Sharp.Commands;

namespace CommitTemplateExtension.Git
{
    /// <summary>
    /// Integration of used git lib into a service
    /// </summary>
    public class GitService
    {
        private static GitService instance;
        private Repository repository;
        private string username;
        private string email;
        public string Directory;
        private CredentialsHandler credentialsHandler;

        /// <summary>
        /// Wether the service has found a valid directory with git versioning or not
        /// </summary>
        public bool Initialized = false;

        public static GitService GetInstance(string dir)
        {
            if (instance==null)
                instance = new GitService(dir);

            if (dir != instance.Directory)
                instance.OpenOrInitRepository(dir);

            return instance;
        }

        public PushOptions GetPushOptions()
            => new PushOptions() { CredentialsProvider = credentialsHandler };

        public Branch GetCurrentBranch()
        {
            string curr = repository.Head.FriendlyName;
            return repository.Branches.FirstOrDefault(x => x.FriendlyName == curr);
        }

        public Remote GetRemote()
            => repository.Network.Remotes.FirstOrDefault(x => x.Name == GetCurrentBranch().RemoteName);

        private GitService(string directory)
        {
            if (directory == null)
                return;

            try
            {
                OpenOrInitRepository(directory);
            }
            catch (Exception)
            {
                Initialized = false;
            }
        }

        public bool OpenOrInitRepository(string directory)
        {
            Directory = directory;
            if (Repository.IsValid(Directory))
                return OpenRepository();
            else
                return InitializeRepository();
        }

        private bool OpenRepository()
        {
            bool result;
            try
            {
                repository = new Repository(Directory);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            username = repository.Config.Get<string>(new[] { "user", "name" })?.Value;
            email = repository.Config.Get<string>(new[] { "user", "email" })?.Value;
            credentialsHandler = new CredentialsHandler((_url, _user, _cred) => new UsernamePasswordCredentials() { 
                Username = "",
                Password= "",
            } );
            Initialized = true;
            return result;
        }

        private bool InitializeRepository()
        {
            try
            {
                string path = Repository.Init(Directory);
                OpenRepository();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<FileChangeVM> GetIndexFiles()
        {
            if (repository == null)
                return new List<FileChangeVM>();
            var y = repository.RetrieveStatus(new StatusOptions() { Show = StatusShowOption.IndexOnly, IncludeIgnored=false,IncludeUntracked=false })
             .Select(x => new FileChangeVM() { Name = x.FilePath });
            return y;
        }

        public IEnumerable<FileChangeVM> GetWorkspaceFiles(bool showUntracked = false)
        {
            if (repository == null)
                return new List<FileChangeVM>();
            var y = repository.RetrieveStatus(new StatusOptions() { Show = StatusShowOption.WorkDirOnly, IncludeIgnored = false, IncludeUntracked = showUntracked })
                .Select(x => new FileChangeVM() { Name = x.FilePath });
            return y;
        }
        public bool Stage(string path)
        {
            try
            {
                Commands.Stage(repository, path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
            

        public bool Unstage(string path)
        {
            try
            {
                Commands.Unstage(repository, path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Commit(string message)
        {
            if (repository == null)
                return false;

            new DefaultCredentials();
            var authorSignature = new Signature(username, email, DateTime.Now);
            try
            {
                repository.Commit(message, authorSignature, authorSignature);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void GitFetch()
        {
            var creds = new UsernamePasswordCredentials()
            {
                Username = "user",
                Password = "pass"
            };
            CredentialsHandler credHandler = (_url, _user, _cred) => creds;
            var fetchOpts = new FetchOptions { CredentialsProvider = credHandler };
            var remote = repository.Network.Remotes.FirstOrDefault(x => x.Name == GetCurrentBranch().RemoteName);
            repository.Network.Fetch(remote.Url,remote.RefSpecs.Select(x=>x.Destination));
        }

        public PushResult Push()
        {
            if (repository == null)
                return PushResult.FAIL;
            try
            {
                if (!CurrentBranchHasRemote())
                    return PushResult.REQUIRES_REMOTE;

                repository.Network.Push(GetCurrentBranch(), GetPushOptions());
            }
            catch (Exception)
            {
                return PushResult.FAIL;
            }
            return PushResult.SUCCESS;
        }

        public bool CurrentBranchHasRemote()
             => GetCurrentBranch().RemoteName != null;
        
        /// <summary>
        /// Creates a remote for the repository and also sets the current branch to its remote
        /// </summary>
        /// <param name="name">name of the remote</param>
        /// <param name="remoteUrl">http url to the remote</param>
        /// <returns></returns>
        public bool CreateRemote(string name, string remoteUrl)
        {
            try
            {
                Remote remote = repository.Network.Remotes.Add(name, remoteUrl);
                Branch branch = GetCurrentBranch();
                repository.Branches.Update(branch,
                    b => b.Remote = remote.Name,
                    b => b.UpstreamBranch = branch.CanonicalName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public enum PushResult
        {
            FAIL = 0,
            SUCCESS = 1,
            REQUIRES_REMOTE,
        }
    }
}
