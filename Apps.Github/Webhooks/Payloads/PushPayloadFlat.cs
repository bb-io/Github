using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Payloads
{
    public class PushPayloadFlat
    {
        public PushPayloadFlat(PushPayload source)
        {
            Ref = source.Ref;
            Before = source.Before;
            After = source.After;
            RepositoryName = source.Repository.Name;
            RepositoryId = source.Repository.Id.ToString();
            RepositoryUrl = source.Repository.Url;
            PusherName = source.Pusher.Name;
            PusherEmail = source.Pusher.Email;
            SenderLogin = source.Sender.Login;
            SenderId = source.Sender.Id;
            SenderUrl = source.Sender.Url;
            Created = source.Created;
            Deleted = source.Deleted;
            Forced = source.Forced;
            Compare = source.Compare;
            Commits = source.Commits.Select(c => new CommitFlat()
            {
                Id = c.Id,
                TreeId = c.TreeId,
                Distinct = c.Distinct,
                Message = c.Message,
                Timestamp = c.Timestamp.ToString(),
                Url = c.Url,
                AuthorName = c.Author.Name,
                AuthorEmail = c.Author.Email,
                AuthorUsername = c.Author.Username,
                CommitterName = c.Committer.Name,
                CommitterEmail = c.Committer.Email,
                CommitterUsername = c.Committer.Username,
                Added = c.Added.Select(c => new FileId() { FilePath = c }).ToList(),
                Removed = c.Removed.Select(c => new FileId() { FilePath = c }).ToList(),
                Modified = c.Modified.Select(c => new FileId() { FilePath = c }).ToList()
            }).ToList();
        }

        public string Ref { get; set; }
        public string Before { get; set; }
        public string After { get; set; }

        public string RepositoryName { get; set; }

        public string RepositoryId { get; set; }

        public string RepositoryUrl { get; set; }

        public string PusherName { get; set; }
        public string PusherEmail { get; set; }


        public string SenderLogin { get; set; }
        public int SenderId { get; set; }

        public string SenderUrl { get; set; }

        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public bool Forced { get; set; }
        public string Compare { get; set; }
        public List<CommitFlat> Commits { get; set; }
    }

    public class CommitFlat
    {
        public string Id { get; set; }
        public string TreeId { get; set; }
        public bool Distinct { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string Url { get; set; }

        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorUsername { get; set; }


        public string CommitterName { get; set; }
        public string CommitterEmail { get; set; }
        public string CommitterUsername { get; set; }

        public List<FileId> Added { get; set; }
        public List<FileId> Removed { get; set; }
        public List<FileId> Modified { get; set; }
    }
}
