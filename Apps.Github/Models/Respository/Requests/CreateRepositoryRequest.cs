using Apps.Github.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Octokit;

namespace Apps.Github.Models.Respository.Requests
{
    public class CreateRepositoryRequest
    {
        [Display("Auto initial commit")]
        public bool? AutoInit { get; set; }
        public string Description { get; set; }

        [Display("Has downloads")]
        public bool? HasDownloads { get; set; }

        [Display("Has issues")]
        public bool? HasIssues { get; set; }

        [Display("Has projects")]
        public bool? HasProjects { get; set; }

        [Display("Has wiki")]
        public bool? HasWiki { get; set; }

        [Display("Is template")]
        public bool? IsTemplate { get; set; }
        
        public string? Homepage { get; set; }

        [Display("Gitignore template")]
        public string? GitignoreTemplate { get; set; }

        [Display("License template")]
        public string? LicenseTemplate { get; set; }

        public string Name { get; set; }

        public bool? Private { get; set; }

        [Display("Team ID")]
        public int? TeamId { get; set; }

        [Display("Delete branch on merge")]
        public bool? DeleteBranchOnMerge { get; set; }

        [Display("Visibility")]
        [DataSource(typeof(RepoVisibilityDataHandler))]
        public string? Visibility { get; set; }

        [Display("Allow rebase merge")]
        public bool? AllowRebaseMerge { get; set; }

        [Display("Allow squash merge")]
        public bool? AllowSquashMerge { get; set; }
        
        [Display("Allow merge commit")]
        public bool? AllowMergeCommit { get; set; }

        [Display("Allow auto merge")]
        public bool? AllowAutoMerge { get; set; }

        [Display("Use squash PR title as default")]
        public bool? UseSquashPrTitleAsDefault { get; set; }

        public NewRepository GetNewRepositoryRequest()
        {
            return new NewRepository(Name)
            {
                AutoInit = AutoInit,
                Description = Description,
                HasDownloads = HasDownloads,
                HasIssues = HasIssues,
                HasProjects = HasProjects,
                HasWiki = HasWiki,
                IsTemplate = IsTemplate,
                Homepage = Homepage,
                GitignoreTemplate = GitignoreTemplate,
                LicenseTemplate = LicenseTemplate,
                Private = Private,
                TeamId = TeamId,
                DeleteBranchOnMerge = DeleteBranchOnMerge,
                Visibility = Visibility != null ? (RepositoryVisibility) int.Parse(Visibility) : RepositoryVisibility.Public,
                AllowRebaseMerge = AllowRebaseMerge,
                AllowSquashMerge = AllowSquashMerge,
                AllowMergeCommit = AllowMergeCommit,
                AllowAutoMerge = AllowAutoMerge,
                UseSquashPrTitleAsDefault = UseSquashPrTitleAsDefault
            };
        }
    }
}
