using Starcounter;

namespace SafaArooj
{
    partial class CoorporationJson : Json
    {
        static CoorporationJson()
        {
            DefaultTemplate.Franchises.ElementType.InstanceType = typeof(FranchiseJson);
        }

        void Handle(Input.CoorporationSave action)
        {
            Transaction.Commit();
        }

        void Handle(Input.HousesSold action)
        {

            Franchises.Clear();
            var franchises = Db.SQL<Franchise>("SELECT f FROM SafaArooj.Franchise f WHERE f.Coorporation = ? ORDER BY f.NoOfHousesSold",Data);
            Franchises.Data = franchises;

        }

        void Handle(Input.TotalCommision action)
        {

            Franchises.Clear();
            var franchises = Db.SQL<Franchise>("SELECT f FROM SafaArooj.Franchise f WHERE f.Coorporation = ? ORDER BY f.Comision", Data);
            Franchises.Data = franchises;
        }

        void Handle(Input.AverageCommision action)
        {
            Franchises.Clear();
            var franchises = Db.SQL<Franchise>("SELECT f FROM SafaArooj.Franchise f WHERE f.Coorporation = ? ORDER BY f.AverageCommision", Data);
            Franchises.Data = franchises;
        }

        void Handle(Input.Trend action)
        {
            Franchises.Clear();
            var franchises = Db.SQL<Franchise>("SELECT f FROM SafaArooj.Franchise f WHERE f.Coorporation = ? ORDER BY f.Trend", Data);
            Franchises.Data = franchises;
        }
        void Handle(Input.NewFranchiseOfficeDetails action)
        {
            
            new Franchise()
            {
                Coorporation = this.Data as Coorporation,      
                FranchiseName = this.__bf__FranchiseName__,
                FranchiseAddress = new Address()
                {
                    Street = "",
                    Number = "",
                    ZipCode = "0",
                    City = "",
                    Country = ""
                },
                Trend = 0
            };
            Transaction.Commit();
        }
    }
}
