using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Receiving;
using System.Net.Security;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Receiving
{
    public partial class ReceivingManager
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IReceivingService ReceivingService { get; set; } = default!;

        protected ReceivingDTO? Receiving { get; set; }

        protected List<ReceivingLineDTO>? FilteredRecList { get; set; }

        protected bool NewRec { get; set; }

        protected TableSearch RecSearchFilter { get; set; } = new();

        protected int SortOrder { get; set; } = 1;

        protected int SortColumn { get; set; } = 1;

        protected bool DeleteRec { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Receiving", "Receive inventory to stock.");

            try
            {
                Receiving = await ReceivingService.GetReceivingAsync(FormID);

                // Get Rec Line by filter
                FilteredRecList = [.. Receiving.ReceivingLines ];
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
        }

        protected void AddRec_Toggle()
        {
            Layout.ClearMessages();
            NewRec = !NewRec;
        }

        protected void RecSearch_Click()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            try
            {
                if (Receiving != null)
                {
                    // Get Rec Line by filter
                    FilteredRecList = [.. Receiving.ReceivingLines];

                    if (!string.IsNullOrEmpty(RecSearchFilter.SearchText) && FilteredRecList != null)
                    {
                        var searchFilter = RecSearchFilter.SearchText
                           .Split(',')
                           .Select(x => x.Trim())
                           .Where(x => !string.IsNullOrEmpty(x))
                           .ToList();

                        var query = FilteredRecList
                            .AsQueryable();

                        foreach (var filter in searchFilter)
                        {
                            query = query.Where(
                                 x => x.PartNumber.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                    || x.PartDescription.Contains(filter, StringComparison.OrdinalIgnoreCase)
                            );
                        }

                        FilteredRecList = [.. query];
                    }
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected void SortSwitch_Click(int column)
        {
            Layout.ClearMessages();

            SortColumn = column;

            if (FilteredRecList != null)
            {
                // Sort Locally No API

                if (FilteredRecList.Count == 0)
                    return;

                SortOrder = SortOrder == 1 ? 2 : 1;

                FilteredRecList = SortOrder switch
                {
                    1 => column switch
                    {
                        1 => [.. FilteredRecList.OrderBy(x => x.PartNumber)],
                        2 => [.. FilteredRecList.OrderBy(x => x.PartDescription)],
                        3 => [.. FilteredRecList.OrderBy(x => x.PurchaseOrder)],
                        4 => [.. FilteredRecList.OrderBy(x => x.Quantity)],
                        _ => [.. FilteredRecList.OrderBy(x => x.PartNumber)],
                    },
                    2 => column switch
                    {
                        1 => [.. FilteredRecList.OrderByDescending(x => x.PartNumber)],
                        2 => [.. FilteredRecList.OrderByDescending(x => x.PartDescription)],
                        3 => [.. FilteredRecList.OrderByDescending(x => x.PurchaseOrder)],
                        4 => [.. FilteredRecList.OrderByDescending(x => x.Quantity)],
                        _ => [.. FilteredRecList.OrderByDescending(x => x.PartNumber)],
                    },
                    _ => [.. FilteredRecList.OrderBy(x => x.PartNumber)]
                };
            }
        }

        protected void DeleteRec_Toggle()
        {
            Layout.ClearMessages();
            DeleteRec = !DeleteRec;
        }

        protected async Task DeleteRecConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Receiving != null)
                {
                    // Delete Part
                    await ReceivingService.DeleteReceivingAsync(Receiving.FormID);

                    Navigation.NavigateTo($"/receiving/search?Delete=true");
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                DeleteRec = false;
            }
        }
    }
}