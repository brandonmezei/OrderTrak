using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Parts;
using OrderTrak.Client.Services.PO;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.PO
{
    public partial class POManager
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IPOService POService { get; set; } = default!;

        [Inject]
        private IPartService PartService { get; set; } = default!;

        protected PoDTO? PurchaseOrder { get; set; }

        protected bool DeletePO { get; set; }

        protected TableSearch POSearchFilter { get; set; } = new();

        protected int SortOrder { get; set; } = 1;

        protected int SortColumn { get; set; } = 1;

        protected List<POLineDTO>? FilteredPOList { get; set; }

        protected bool NewPart { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Purchase Order Admin", "Create and edit purchase orders.");

            try
            {
                PurchaseOrder = await POService.GetPOAsync(FormID);
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

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (PurchaseOrder != null)
                {

                    // Save Upper Level Info
                    await POService.UpdatePOAsync(MapperService.Map<POUpdateDTO>(PurchaseOrder));

                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
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
        }

        protected void Delete_Toggle()
        {
            Layout.ClearMessages();

            DeletePO = !DeletePO;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (PurchaseOrder != null)
                {
                    // Delete Part
                    await POService.DeletePOAsync(FormID);

                    Navigation.NavigateTo($"/po/search?Delete=true");
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
                DeletePO = false;
            }
        }

        protected void POSearch_Click()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            try
            {
                if (PurchaseOrder != null)
                {
                    // Get Purchase Line by filter
                    FilteredPOList = [.. PurchaseOrder.PoLines];

                    if (!string.IsNullOrEmpty(POSearchFilter.SearchText) && FilteredPOList != null)
                    {
                        var searchFilter = POSearchFilter.SearchText
                           .Split(',')
                           .Select(x => x.Trim())
                           .Where(x => !string.IsNullOrEmpty(x))
                           .ToList();

                        var query = FilteredPOList
                            .AsQueryable();

                        foreach (var filter in searchFilter)
                        {
                            query = query.Where(
                                 x => x.PartNumber.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                    || x.PartDescription.Contains(filter, StringComparison.OrdinalIgnoreCase)
                            );
                        }

                        FilteredPOList = [.. query];
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

            if (FilteredPOList != null)
            {
                // Sort Locally No API

                if (FilteredPOList.Count == 0)
                    return;

                SortOrder = SortOrder == 1 ? 2 : 1;

                FilteredPOList = SortOrder switch
                {
                    1 => column switch
                    {
                        1 => [.. FilteredPOList.OrderBy(x => x.PartNumber)],
                        2 => [.. FilteredPOList.OrderBy(x => x.PartDescription)],
                        3 => [.. FilteredPOList.OrderBy(x => x.Quantity)],
                        4 => [.. FilteredPOList.OrderBy(x => x.RecQuantity)],
                        _ => [.. FilteredPOList.OrderBy(x => x.PartNumber)],
                    },
                    2 => column switch
                    {
                        1 => [.. FilteredPOList.OrderByDescending(x => x.PartNumber)],
                        2 => [.. FilteredPOList.OrderByDescending(x => x.PartDescription)],
                        3 => [.. FilteredPOList.OrderByDescending(x => x.Quantity)],
                        4 => [.. FilteredPOList.OrderByDescending(x => x.RecQuantity)],
                        _ => [.. FilteredPOList.OrderByDescending(x => x.PartNumber)],
                    },
                    _ => [.. FilteredPOList.OrderBy(x => x.PartNumber)]
                };
            }
        }

        protected void AddPart_Toggle()
        {
            Layout.ClearMessages();
            NewPart = !NewPart;
        }

        protected async Task AddPart_Click(Guid? FormID)
        {
            Layout.ClearMessages();

            try
            {

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
                NewPart = false;
            }
        }
    }
}