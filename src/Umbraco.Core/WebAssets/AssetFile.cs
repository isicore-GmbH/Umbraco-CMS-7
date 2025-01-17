﻿using System.Diagnostics;

namespace Umbraco.Cms.Core.WebAssets
{
    /// <summary>
    /// Represents a dependency file
    /// </summary>
    [DebuggerDisplay("Type: {DependencyType}, File: {FilePath}")]
    public class AssetFile : IAssetFile
    {
        #region IAssetFile Members

        public string? FilePath { get; set; }
        public AssetType DependencyType { get; }

        #endregion

        public AssetFile(AssetType type)
        {
            DependencyType = type;
        }
    }
}
