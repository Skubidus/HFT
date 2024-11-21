using HFTLibrary.Models;

namespace BlazorUI.Components.Pages
{
    public partial class Home
    {
        private List<FinancialPlanModel>? _financialPlans;

        protected override async Task OnInitializedAsync()
        {
            _financialPlans = await _db.GetFullFinancialPlanListAsync();
            await base.OnInitializedAsync();
        }
    }
}