using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBBArkiv
{
    public class LookupRetriever
    {
        public static List<MusicPart> GetPartLookup()
        {
            return EntitiesFactory.AsSingleton().MusicParts.ToList();
        }

        public static List<SheetMusicCategory> GetCategoryLookup()
        {
            return EntitiesFactory.AsSingleton().SheetMusicCategories.Where(o => !o.Inactive).ToList();
        }

        public static List<UserGroup> GetUserGroupsLookup()
        {
            return EntitiesFactory.AsSingleton().UserGroups.ToList();
        }
    }
}