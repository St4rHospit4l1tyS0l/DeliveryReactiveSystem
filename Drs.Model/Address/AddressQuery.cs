namespace Drs.Model.Address
{
    public class AddressQuery
    {
        public string NextRegion { get; set; }
        public int ItemSelId { get; set; }

        public AddressQuery(string sNextRegion, int iItemSelId)
        {
            NextRegion = sNextRegion;
            ItemSelId = iItemSelId;
        }

    }
}
