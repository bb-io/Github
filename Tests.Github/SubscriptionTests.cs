using Apps.Github.Webhooks;
using Apps.Github.Webhooks.Handlers;
using Apps.Github.Webhooks.Payloads;
using Apps.GitHub.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Tests.Github.Base;

namespace Tests.Github;

[TestClass]
public class SubscriptionTests : TestBase
{
    private WebhookRequest GetWebhookRequest()
    {
        // This includes a modified file "/lang/en.json"
        var realPayload = "{\r\n  \"ref\": \"refs/heads/main\",\r\n  \"before\": \"6be945f6eb13c83b34e4cb7cd772e030d934fd2b\",\r\n  \"after\": \"955679060a1006a5f2f7d9dc9704d73c06cba067\",\r\n  \"repository\": {\r\n    \"id\": 1116720412,\r\n    \"node_id\": \"R_kgDOQo_NHA\",\r\n    \"name\": \"intl-example\",\r\n    \"full_name\": \"bb-io/intl-example\",\r\n    \"private\": true,\r\n    \"owner\": {\r\n      \"name\": \"bb-io\",\r\n      \"email\": null,\r\n      \"login\": \"bb-io\",\r\n      \"id\": 131261637,\r\n      \"node_id\": \"O_kgDOB9LkxQ\",\r\n      \"avatar_url\": \"https://avatars.githubusercontent.com/u/131261637?v=4\",\r\n      \"gravatar_id\": \"\",\r\n      \"url\": \"https://api.github.com/users/bb-io\",\r\n      \"html_url\": \"https://github.com/bb-io\",\r\n      \"followers_url\": \"https://api.github.com/users/bb-io/followers\",\r\n      \"following_url\": \"https://api.github.com/users/bb-io/following{/other_user}\",\r\n      \"gists_url\": \"https://api.github.com/users/bb-io/gists{/gist_id}\",\r\n      \"starred_url\": \"https://api.github.com/users/bb-io/starred{/owner}{/repo}\",\r\n      \"subscriptions_url\": \"https://api.github.com/users/bb-io/subscriptions\",\r\n      \"organizations_url\": \"https://api.github.com/users/bb-io/orgs\",\r\n      \"repos_url\": \"https://api.github.com/users/bb-io/repos\",\r\n      \"events_url\": \"https://api.github.com/users/bb-io/events{/privacy}\",\r\n      \"received_events_url\": \"https://api.github.com/users/bb-io/received_events\",\r\n      \"type\": \"Organization\",\r\n      \"user_view_type\": \"public\",\r\n      \"site_admin\": false\r\n    },\r\n    \"html_url\": \"https://github.com/bb-io/intl-example\",\r\n    \"description\": null,\r\n    \"fork\": false,\r\n    \"url\": \"https://api.github.com/repos/bb-io/intl-example\",\r\n    \"forks_url\": \"https://api.github.com/repos/bb-io/intl-example/forks\",\r\n    \"keys_url\": \"https://api.github.com/repos/bb-io/intl-example/keys{/key_id}\",\r\n    \"collaborators_url\": \"https://api.github.com/repos/bb-io/intl-example/collaborators{/collaborator}\",\r\n    \"teams_url\": \"https://api.github.com/repos/bb-io/intl-example/teams\",\r\n    \"hooks_url\": \"https://api.github.com/repos/bb-io/intl-example/hooks\",\r\n    \"issue_events_url\": \"https://api.github.com/repos/bb-io/intl-example/issues/events{/number}\",\r\n    \"events_url\": \"https://api.github.com/repos/bb-io/intl-example/events\",\r\n    \"assignees_url\": \"https://api.github.com/repos/bb-io/intl-example/assignees{/user}\",\r\n    \"branches_url\": \"https://api.github.com/repos/bb-io/intl-example/branches{/branch}\",\r\n    \"tags_url\": \"https://api.github.com/repos/bb-io/intl-example/tags\",\r\n    \"blobs_url\": \"https://api.github.com/repos/bb-io/intl-example/git/blobs{/sha}\",\r\n    \"git_tags_url\": \"https://api.github.com/repos/bb-io/intl-example/git/tags{/sha}\",\r\n    \"git_refs_url\": \"https://api.github.com/repos/bb-io/intl-example/git/refs{/sha}\",\r\n    \"trees_url\": \"https://api.github.com/repos/bb-io/intl-example/git/trees{/sha}\",\r\n    \"statuses_url\": \"https://api.github.com/repos/bb-io/intl-example/statuses/{sha}\",\r\n    \"languages_url\": \"https://api.github.com/repos/bb-io/intl-example/languages\",\r\n    \"stargazers_url\": \"https://api.github.com/repos/bb-io/intl-example/stargazers\",\r\n    \"contributors_url\": \"https://api.github.com/repos/bb-io/intl-example/contributors\",\r\n    \"subscribers_url\": \"https://api.github.com/repos/bb-io/intl-example/subscribers\",\r\n    \"subscription_url\": \"https://api.github.com/repos/bb-io/intl-example/subscription\",\r\n    \"commits_url\": \"https://api.github.com/repos/bb-io/intl-example/commits{/sha}\",\r\n    \"git_commits_url\": \"https://api.github.com/repos/bb-io/intl-example/git/commits{/sha}\",\r\n    \"comments_url\": \"https://api.github.com/repos/bb-io/intl-example/comments{/number}\",\r\n    \"issue_comment_url\": \"https://api.github.com/repos/bb-io/intl-example/issues/comments{/number}\",\r\n    \"contents_url\": \"https://api.github.com/repos/bb-io/intl-example/contents/{+path}\",\r\n    \"compare_url\": \"https://api.github.com/repos/bb-io/intl-example/compare/{base}...{head}\",\r\n    \"merges_url\": \"https://api.github.com/repos/bb-io/intl-example/merges\",\r\n    \"archive_url\": \"https://api.github.com/repos/bb-io/intl-example/{archive_format}{/ref}\",\r\n    \"downloads_url\": \"https://api.github.com/repos/bb-io/intl-example/downloads\",\r\n    \"issues_url\": \"https://api.github.com/repos/bb-io/intl-example/issues{/number}\",\r\n    \"pulls_url\": \"https://api.github.com/repos/bb-io/intl-example/pulls{/number}\",\r\n    \"milestones_url\": \"https://api.github.com/repos/bb-io/intl-example/milestones{/number}\",\r\n    \"notifications_url\": \"https://api.github.com/repos/bb-io/intl-example/notifications{?since,all,participating}\",\r\n    \"labels_url\": \"https://api.github.com/repos/bb-io/intl-example/labels{/name}\",\r\n    \"releases_url\": \"https://api.github.com/repos/bb-io/intl-example/releases{/id}\",\r\n    \"deployments_url\": \"https://api.github.com/repos/bb-io/intl-example/deployments\",\r\n    \"created_at\": 1765791215,\r\n    \"updated_at\": \"2025-12-15T14:45:45Z\",\r\n    \"pushed_at\": 1765828881,\r\n    \"git_url\": \"git://github.com/bb-io/intl-example.git\",\r\n    \"ssh_url\": \"git@github.com:bb-io/intl-example.git\",\r\n    \"clone_url\": \"https://github.com/bb-io/intl-example.git\",\r\n    \"svn_url\": \"https://github.com/bb-io/intl-example\",\r\n    \"homepage\": null,\r\n    \"size\": 46,\r\n    \"stargazers_count\": 0,\r\n    \"watchers_count\": 0,\r\n    \"language\": \"CSS\",\r\n    \"has_issues\": true,\r\n    \"has_projects\": true,\r\n    \"has_downloads\": true,\r\n    \"has_wiki\": false,\r\n    \"has_pages\": false,\r\n    \"has_discussions\": false,\r\n    \"forks_count\": 0,\r\n    \"mirror_url\": null,\r\n    \"archived\": false,\r\n    \"disabled\": false,\r\n    \"open_issues_count\": 0,\r\n    \"license\": null,\r\n    \"allow_forking\": false,\r\n    \"is_template\": false,\r\n    \"web_commit_signoff_required\": false,\r\n    \"topics\": [],\r\n    \"visibility\": \"private\",\r\n    \"forks\": 0,\r\n    \"open_issues\": 0,\r\n    \"watchers\": 0,\r\n    \"default_branch\": \"main\",\r\n    \"stargazers\": 0,\r\n    \"master_branch\": \"main\",\r\n    \"organization\": \"bb-io\",\r\n    \"custom_properties\": {}\r\n  },\r\n  \"pusher\": {\r\n    \"name\": \"mathijs-bb\",\r\n    \"email\": \"131260901+mathijs-bb@users.noreply.github.com\"\r\n  },\r\n  \"organization\": {\r\n    \"login\": \"bb-io\",\r\n    \"id\": 131261637,\r\n    \"node_id\": \"O_kgDOB9LkxQ\",\r\n    \"url\": \"https://api.github.com/orgs/bb-io\",\r\n    \"repos_url\": \"https://api.github.com/orgs/bb-io/repos\",\r\n    \"events_url\": \"https://api.github.com/orgs/bb-io/events\",\r\n    \"hooks_url\": \"https://api.github.com/orgs/bb-io/hooks\",\r\n    \"issues_url\": \"https://api.github.com/orgs/bb-io/issues\",\r\n    \"members_url\": \"https://api.github.com/orgs/bb-io/members{/member}\",\r\n    \"public_members_url\": \"https://api.github.com/orgs/bb-io/public_members{/member}\",\r\n    \"avatar_url\": \"https://avatars.githubusercontent.com/u/131261637?v=4\",\r\n    \"description\": \"\"\r\n  },\r\n  \"sender\": {\r\n    \"login\": \"mathijs-bb\",\r\n    \"id\": 131260901,\r\n    \"node_id\": \"U_kgDOB9Lh5Q\",\r\n    \"avatar_url\": \"https://avatars.githubusercontent.com/u/131260901?v=4\",\r\n    \"gravatar_id\": \"\",\r\n    \"url\": \"https://api.github.com/users/mathijs-bb\",\r\n    \"html_url\": \"https://github.com/mathijs-bb\",\r\n    \"followers_url\": \"https://api.github.com/users/mathijs-bb/followers\",\r\n    \"following_url\": \"https://api.github.com/users/mathijs-bb/following{/other_user}\",\r\n    \"gists_url\": \"https://api.github.com/users/mathijs-bb/gists{/gist_id}\",\r\n    \"starred_url\": \"https://api.github.com/users/mathijs-bb/starred{/owner}{/repo}\",\r\n    \"subscriptions_url\": \"https://api.github.com/users/mathijs-bb/subscriptions\",\r\n    \"organizations_url\": \"https://api.github.com/users/mathijs-bb/orgs\",\r\n    \"repos_url\": \"https://api.github.com/users/mathijs-bb/repos\",\r\n    \"events_url\": \"https://api.github.com/users/mathijs-bb/events{/privacy}\",\r\n    \"received_events_url\": \"https://api.github.com/users/mathijs-bb/received_events\",\r\n    \"type\": \"User\",\r\n    \"user_view_type\": \"public\",\r\n    \"site_admin\": false\r\n  },\r\n  \"created\": false,\r\n  \"deleted\": false,\r\n  \"forced\": false,\r\n  \"base_ref\": null,\r\n  \"compare\": \"https://github.com/bb-io/intl-example/compare/6be945f6eb13...955679060a10\",\r\n  \"commits\": [\r\n    {\r\n      \"id\": \"955679060a1006a5f2f7d9dc9704d73c06cba067\",\r\n      \"tree_id\": \"2bdbd5ca18a9e7622c99ef49a9d75e5d77c49c30\",\r\n      \"distinct\": true,\r\n      \"message\": \"more marks\",\r\n      \"timestamp\": \"2025-12-15T21:01:18+01:00\",\r\n      \"url\": \"https://github.com/bb-io/intl-example/commit/955679060a1006a5f2f7d9dc9704d73c06cba067\",\r\n      \"author\": {\r\n        \"name\": \"mathijs-bb\",\r\n        \"email\": \"mathijs.sonnemans@blackbird.io\",\r\n        \"date\": \"2025-12-15T21:01:18+01:00\",\r\n        \"username\": \"mathijs-bb\"\r\n      },\r\n      \"committer\": {\r\n        \"name\": \"mathijs-bb\",\r\n        \"email\": \"mathijs.sonnemans@blackbird.io\",\r\n        \"date\": \"2025-12-15T21:01:18+01:00\",\r\n        \"username\": \"mathijs-bb\"\r\n      },\r\n      \"added\": [],\r\n      \"removed\": [],\r\n      \"modified\": [\r\n        \"lang/en.json\"\r\n      ]\r\n    }\r\n  ],\r\n  \"head_commit\": {\r\n    \"id\": \"955679060a1006a5f2f7d9dc9704d73c06cba067\",\r\n    \"tree_id\": \"2bdbd5ca18a9e7622c99ef49a9d75e5d77c49c30\",\r\n    \"distinct\": true,\r\n    \"message\": \"more marks\",\r\n    \"timestamp\": \"2025-12-15T21:01:18+01:00\",\r\n    \"url\": \"https://github.com/bb-io/intl-example/commit/955679060a1006a5f2f7d9dc9704d73c06cba067\",\r\n    \"author\": {\r\n      \"name\": \"mathijs-bb\",\r\n      \"email\": \"mathijs.sonnemans@blackbird.io\",\r\n      \"date\": \"2025-12-15T21:01:18+01:00\",\r\n      \"username\": \"mathijs-bb\"\r\n    },\r\n    \"committer\": {\r\n      \"name\": \"mathijs-bb\",\r\n      \"email\": \"mathijs.sonnemans@blackbird.io\",\r\n      \"date\": \"2025-12-15T21:01:18+01:00\",\r\n      \"username\": \"mathijs-bb\"\r\n    },\r\n    \"added\": [],\r\n    \"removed\": [],\r\n    \"modified\": [\r\n      \"lang/en.json\"\r\n    ]\r\n  }\r\n}";
        var webhookRequest = new WebhookRequest();
        webhookRequest.Body = realPayload;
        return webhookRequest;
    }

    [TestMethod]
    public async Task Subscribe_and_unsubscribe()
    {
        var payloadUrl = "https://webhook.site/4014dcdc-90cc-40d5-8dbd-4d05f858294c";
        var handler = new PushActionHandler(InvocationContext, new WebhookRepositoryInput { RepositoryId = "1116720412" });

        var webhooks = await handler.GetAllHooks();    
        Assert.AreEqual(0, webhooks.Count);
        await handler.SubscribeAsync(Creds, new Dictionary<string, string> { { "payloadUrl", payloadUrl } });

        webhooks = await handler.GetAllHooks();
        Assert.AreEqual(1, webhooks.Count);

        await handler.UnsubscribeAsync(Creds, new Dictionary<string, string> { { "payloadUrl", payloadUrl } });

        webhooks = await handler.GetAllHooks();
        Assert.AreEqual(0, webhooks.Count);
    }

    [TestMethod]
    public async Task Push_no_variables()
    {
        var webhooks = new PushWebhooks();
        var result = await webhooks.FilesAddedAndModifiedHandler(GetWebhookRequest(), new FolderInput { }, new BranchInput { });

        Assert.IsTrue(result.ReceivedWebhookRequestType == WebhookRequestType.Default);
        Assert.AreEqual(1, result.Result.Files.Count());        
    }

    [TestMethod]
    public async Task Push_with_folder_variables()
    {
        var webhooks = new PushWebhooks();
        var result = await webhooks.FilesAddedAndModifiedHandler(GetWebhookRequest(), new FolderInput { Folder = "/lang" }, new BranchInput { });

        Assert.IsTrue(result.ReceivedWebhookRequestType == WebhookRequestType.Default);
        Assert.AreEqual(1, result.Result?.Files.Count());
    }

    [TestMethod]
    public async Task Push_with_wrong_folder_variables()
    {
        var webhooks = new PushWebhooks();
        var result = await webhooks.FilesAddedAndModifiedHandler(GetWebhookRequest(), new FolderInput { Folder = "/not-lang" }, new BranchInput { });

        Assert.IsTrue(result.ReceivedWebhookRequestType == WebhookRequestType.Preflight);
    }

    [TestMethod]
    public async Task Push_with_pattern_variables()
    {
        var webhooks = new PushWebhooks();
        var result = await webhooks.FilesAddedAndModifiedHandler(GetWebhookRequest(), new FolderInput { FolderPath = "/**/*.json"}, new BranchInput { });

        Assert.IsTrue(result.ReceivedWebhookRequestType == WebhookRequestType.Default);
        Assert.AreEqual(1, result.Result?.Files.Count());
    }

    [TestMethod]
    public async Task Push_with_wrong_pattern_variables()
    {
        var webhooks = new PushWebhooks();
        var result = await webhooks.FilesAddedAndModifiedHandler(GetWebhookRequest(), new FolderInput { Folder = "/**/test/en.json" }, new BranchInput { });

        Assert.IsTrue(result.ReceivedWebhookRequestType == WebhookRequestType.Preflight);
    }

    [TestMethod]
    public async Task Push_with_folder_and_pattern_variables()
    {
        var webhooks = new PushWebhooks();
        var result = await webhooks.FilesAddedAndModifiedHandler(GetWebhookRequest(), new FolderInput { Folder = "/lang", FolderPath = "/**/en.json" }, new BranchInput { });

        Assert.IsTrue(result.ReceivedWebhookRequestType == WebhookRequestType.Default);
        Assert.AreEqual(1, result.Result?.Files.Count());
    }
}
