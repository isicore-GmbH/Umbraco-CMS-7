﻿using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace Umbraco.Cms.Core.Extensions;

public static class ReadOnlyUserGroupExtensions
{
    /// <summary>
    /// Checks if a readonly user group has access to a given language
    /// </summary>
    /// <remarks> If allowed languages on the user group is empty, it means that we have access to all languages, and thus have access to the language</remarks>
    public static bool HasAccessToLanguage(this IReadOnlyUserGroup readOnlyUserGroup, int languageId)
    {

        if (readOnlyUserGroup.AllowedLanguages.Any() is false || readOnlyUserGroup.AllowedLanguages.Contains(languageId))
        {
            return true;
        }

        return false;
    }
}