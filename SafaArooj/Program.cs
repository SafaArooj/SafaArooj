using System;
using Starcounter;
using System.Collections.Generic;

namespace SafaArooj
{
    [Database]
    public class Coorporation
    {
        public string Name;

        public QueryResultRows<Franchise> Franchises => Db.SQL<Franchise>("SELECT f FROM SafaArooj.Franchise f WHERE f.Coorporation = ?", this);

    }

    [Database]
    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    [Database]
    public class HousesSold
    {
        public Address HouseAddress { get; set; }

        public String DateOfTransaction { get; set; }

        public decimal SalePrice { get; set; }

        public double Comision { get; set; }

        public Franchise Franchise { get; set; }
    }

    [Database]
    public class Franchise
    {
        public Address FranchiseAddress { get; set; }

        public Coorporation Coorporation { get; set; }

        public string FranchiseName { get; set; }

        public QueryResultRows<HousesSold> Houses => Db.SQL<HousesSold>("SELECT h FROM SafaArooj.HousesSold h WHERE h.Franchise = ?", this);

        public Int64 NoOfHousesSold => Db.SQL<Int64>("SELECT Count(h) FROM SafaArooj.HousesSold h WHERE h.Franchise = ?", this).First == 0 ? 0 : Db.SQL<Int64>("SELECT Count(h) FROM SafaArooj.HousesSold h WHERE h.Franchise = ?", this).First - 1; //To remove the redundant row

        public double Comision => Db.SQL<double>("SELECT SUM(h.Comision) FROM SafaArooj.HousesSold h WHERE h.Franchise = ?", this).First; 
        
        public double AverageCommision => NoOfHousesSold == 0 ? 0 :Db.SQL<double>("SELECT SUM(h.Comision) FROM SafaArooj.HousesSold h WHERE h.Franchise = ?", this).First / NoOfHousesSold; //To remove the redundant row

        public int Trend;
    }

    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                var anyone = Db.SQL<Coorporation>("SELECT c FROM Coorporation c").First;
                if (anyone == null)
                {
                    new Coorporation
                    {
                        Name = ""
                    };
                }
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/SafaArooj", () =>
             {
                 return Db.Scope(() =>
                 {
                     var coorporation = Db.SQL<Coorporation>("SELECT c FROM Coorporation c").First;
                     var json = new CoorporationJson()
                     {
                         Data = coorporation
                     };

                     if (Session.Current == null)
                     {
                         Session.Current = new Session(SessionOptions.PatchVersioning);
                     }

                     json.Session = Session.Current;
                     return json;
                 });
             });

            Handle.GET("/SafaArooj/Franchise/{?}", (string id) =>
            {
                return Db.Scope(() =>
                {
                    var franchise = Db.SQL<Franchise>("SELECT f FROM SafaArooj.Franchise f WHERE ObjectID=?", id).First;
                    
                    var HousesSold = Db.SQL<HousesSold>("SELECT h FROM SafaArooj.HousesSold h WHERE h.Franchise.ObjectID=?", id).First;

                    if(HousesSold == null)
                    {
                        HousesSold = new HousesSold() //will add a redundant HousesSold row per franchisse
                        {
                            Comision = 0,
                            SalePrice = 0,
                            DateOfTransaction = "",
                            Franchise = franchise
                        };
                    }
                    HousesSold.HouseAddress = new Address()
                    {
                        Street = "",
                        Number = "",
                        ZipCode = "0",
                        City = "",
                        Country = ""
                    };
                    

                    var json = new HousesSoldJson()
                    {
                        Data = HousesSold
                    };

                if (Session.Current == null)
                {
                    Session.Current = new Session(SessionOptions.PatchVersioning);
                }

                json.Session = Session.Current;
                return json;
            });
        });
        }
}
}


