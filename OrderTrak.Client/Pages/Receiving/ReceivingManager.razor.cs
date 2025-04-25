using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.PO;
using OrderTrak.Client.Services.Receiving;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Receiving
{
    public partial class ReceivingManager
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IReceivingService ReceivingService { get; set; } = default!;

        [Inject]
        private IPOService POService { get; set; } = default!;

        protected ReceivingDTO? Receiving { get; set; }

        protected List<ReceivingLineDTO>? FilteredRecList { get; set; }

        protected bool NewRec { get; set; }

        protected PagedTableOfPOLineSearchReturnDTO? NewReturnTable;
        protected SearchQueryDTO NewSearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };
        protected ReceivingLineCreateDTO? NewReceipt { get; set; }

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
                FilteredRecList = [.. Receiving.ReceivingLines];
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

        protected async Task AddRec_Toggle()
        {
            Layout.ClearMessages();
            NewRec = !NewRec;

            NewSearchFilters = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

            if (NewRec)
                NewReturnTable = await POService.SearchPOLineAsync(NewSearchFilters);
            else
                NewReturnTable = null;
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

        protected async Task AddSearch_Click()
        {
            if (IsCardLoading)
                return;

            Layout.ClearMessages();

            IsCardLoading = true;

            try
            {
                NewSearchFilters.Page = 1;

                // Get Po Lines from API
                NewReturnTable = await POService.SearchPOLineAsync(NewSearchFilters);

                if (NewReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
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
                IsCardLoading = false;
            }
        }

        protected async Task NewSortSwitch_Click(int column)
        {
            NewSearchFilters.SortColumn = column;
            NewSearchFilters.SortOrder = NewSearchFilters.SortOrder == 1 ? 2 : 1;

            try
            {
                // Get Po Lines from API
                NewReturnTable = await POService.SearchPOLineAsync(NewSearchFilters);

                if (NewReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
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

        protected async Task NewPageSwitch_Click(int page)
        {
            NewSearchFilters.Page = page;

            try
            {
                // Get Po Lines from API
                NewReturnTable = await POService.SearchPOLineAsync(NewSearchFilters);

                if (NewReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
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

        protected void SelectPOLine_Click(Guid? FormID)
        {
            Layout.ClearMessages();

            // Exit if Null Clearing Receipt
            if (FormID == null)
            {
                NewReceipt = null;
                return;
            }

            try
            {
                var selectedLine = NewReturnTable?.Data
                    .FirstOrDefault(x => x.FormID == FormID);

                if (Receiving != null && selectedLine != null)
                {
                    // Create New Receipt
                    NewReceipt = new ReceivingLineCreateDTO
                    {
                        RecID = Receiving.FormID,
                        PoLineID = selectedLine.FormID,
                        BoxLineList = []
                    };

                    // Add one BoxLineList if the part is not serailized if not add the difference between the quantity and the rec quantity

                    if (selectedLine.IsSerialized)
                    {
                        var remainQty = selectedLine.Quantity - selectedLine.RecQuantity;

                        for (int i = 0; i < remainQty; i++)
                        {
                            NewReceipt.BoxLineList.Add(new ReceivingBoxLineCreateDTO
                            {
                                Quantity = 1,
                            });
                        }
                    }
                    else
                    {
                        NewReceipt.BoxLineList.Add(new ReceivingBoxLineCreateDTO
                        {
                            Quantity = 1,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
        }

        protected void StockGroup_Change(Guid? FormID)
        {
            if (NewReceipt != null)
                NewReceipt.StockGroupID = FormID;
        }

        protected void AddBoxLine_Click()
        {
            Layout.ClearMessages();

            if (NewReceipt != null)
            {
                NewReceipt.BoxLineList.Add(new ReceivingBoxLineCreateDTO
                {
                    Quantity = 1,
                });
            }
        }

        protected async Task SaveRec_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Receiving != null && NewReceipt != null)
                {
                    // Filter Box Ids Qty > 0
                    NewReceipt.BoxLineList = [.. NewReceipt.BoxLineList
                        .Where(x => x.Quantity > 0)];

                    // Get PO Line
                    var selectedLine = NewReturnTable?.Data
                        .FirstOrDefault(x => x.FormID == NewReceipt.PoLineID);

                    if(selectedLine != null && selectedLine.IsSerialized)
                    {
                        // Filter Everything with a blank Serial
                        NewReceipt.BoxLineList = [.. NewReceipt.BoxLineList
                            .Where(x => !string.IsNullOrEmpty(x.SerialNumber))];
                    }

                    if (NewReceipt.BoxLineList.Count > 0)
                    {
                        // Create PO Line
                        await ReceivingService.CreateReceivingLineAsync(NewReceipt);
                        
                        // Clear Receipt
                        NewReceipt = null;

                        // Clear Search
                        await AddRec_Toggle();

                        // Refresh Rec Line
                        Receiving = await ReceivingService.GetReceivingAsync(Receiving.FormID);
                        // Get Rec Line by filter
                        FilteredRecList = [.. Receiving.ReceivingLines];

                        Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
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
        }

        protected async Task SaveUpperInfo_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Receiving != null)
                {
                    // Save PO
                    await ReceivingService.UpdateReceivingAsync(MapperService.Map<ReceivingUpdateDTO>(Receiving));
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
    }
}