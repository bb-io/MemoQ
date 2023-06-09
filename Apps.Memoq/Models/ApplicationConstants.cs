﻿namespace Apps.Memoq.Models;

public static class ApplicationConstants
{
    public const string ProjectServiceUrl = "/serverproject/serverprojectservice";

    public const string FileServiceUrl = "/filemanager/filemanagerservice";

    public const string SecurityServiceUrl = "/security/securityservice";

    public const string TaskServiceUrl = "/tasks/tasksService";

    public const string TranslationMemoryServiceUrl = "/tm/tmservice";

    public static readonly Guid AdminGuid = new Guid("00000000-0000-0000-0001-000000000001");
}