using HFTLibrary.Data;
using HFTLibrary.Models;

namespace BlazorUI.Components.Pages
{
    public partial class Home
    {
        private List<FinancialPlanModel> _financialPlans;

        public Home()
        {
            var _data = new TestData();
            _financialPlans = _data.FinancialPlans;
            //_financialPlans = [];
        }
    }
}