using Starcounter;
using System;

namespace SafaArooj
{
    partial class HousesSoldJson : Json
    {
        static HousesSoldJson()
        {
            DefaultTemplate.Franchise.FranchiseAddress.InstanceType = typeof(AddressJson);
            DefaultTemplate.HouseAddress.InstanceType = typeof(AddressJson);
        }
        void Handle(Input.SaveTrigger action)
        {
            Transaction.Commit();
        }
        void Handle(Input.NewSaleTrigger action)
        {
            new HousesSold()
            {
                Franchise = this.Franchise.Data as Franchise,
                HouseAddress = new Address
                {
                    Street = this.__bf__HouseAddress__.Street,
                    Number = this.__bf__HouseAddress__.Number,
                    ZipCode = this.__bf__HouseAddress__.ZipCode,
                    City = this.__bf__HouseAddress__.City,
                    Country = this.__bf__HouseAddress__.Country,
                },     
            };
            Transaction.Commit();
        }

    }
}
