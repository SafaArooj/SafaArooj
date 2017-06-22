using Starcounter;

namespace SafaArooj
{
    partial class FranchiseJson : Json
    {
        static FranchiseJson()
        {
           
        }

        public string GetUrl
        {
            get { return "/SafaArooj/Franchise/" + this.Data.GetObjectID();  }
        }
    }
}
