﻿using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.PullRequest.Requests;

public class ListPullFilesRequest
{
    [Display("Pull request number")]
    public string PullRequestNumber { get; set; }
}