using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace theDiary.Tools.HideMyWindow
{
    public class AvailableUpdate
    {
        #region Constructors

        public AvailableUpdate(Release releaseInfo)
        {
            this.Date = (releaseInfo.PublishedAt ?? releaseInfo.CreatedAt).LocalDateTime;
            this.Details = releaseInfo.Body;
            this.IsBeta = releaseInfo.Prerelease;
            this.Name = releaseInfo.Name;
        }
    

    public AvailableUpdate(Release releaseInfo, IEnumerable<ReleaseAsset> assets)
            : this(releaseInfo)
        {
            
            this.assets = assets.ToArray();
        }

        private ReleaseAsset[] assets;
        #endregion

        #region Properties
        public string Name
        {
            get;
        }
        public DateTime Date
        {
            get;
        }

        public string Details
        {
            get; private set;
        }

        public bool IsBeta
        {
            get; private set;
        }

        #endregion
    }
}
