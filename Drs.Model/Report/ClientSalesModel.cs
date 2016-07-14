using System;

namespace Drs.Model.Report
{
    public class ClientSalesModel
    {
        private string _email;
        private string _companyName;
        private string _loyaltyCode;
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateTx {
            get
            {
                return BirthDate == null ? "ND" : BirthDate.Value.ToString(Constants.SharedConstants.REPORT_DATE_FORMAT);
            }
        }

        public string Email
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_email))
                    return Constants.SharedConstants.REPORT_NO_VALUE;
                return _email; 
                
            }
            set { _email = value; }
        }

        public string CompanyName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_companyName))
                    return Constants.SharedConstants.REPORT_NO_VALUE;
                return _companyName;
                
            }
            set { _companyName = value; }
        }

        public string LoyaltyCode
        {
            get
            {

                if (String.IsNullOrWhiteSpace(_loyaltyCode))
                    return Constants.SharedConstants.REPORT_NO_VALUE;
                return _loyaltyCode; 
            }
            set { _loyaltyCode = value; }
        }

        public decimal? TotalByProduct { get; set; }
        public string StoreName { get; set; }
        public string FranchiseName { get; set; }
        public int? TotalByConsume { get; set; }
        public string Period { get; set; }
    }
}