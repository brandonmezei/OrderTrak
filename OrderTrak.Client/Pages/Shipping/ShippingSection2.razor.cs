using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Shipping
{
    public partial class ShippingSection2
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        protected OrderHeaderDTO? Order { get; set; }
        protected List<OrderPartListDTO>? PartList { get; set; }
        protected List<OrderPartListDTO>? FilteredPartList { get; set; }

        protected TableSearch SearchFilter { get; set; } = new();

        protected int SortOrder { get; set; } = 1;

        protected int SortColumn { get; set; } = 1;

        protected Guid? PickLineID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();

            try
            {
                Order = await OrderService.GetOrderHeaderAsync(FormID);

                Layout.UpdateHeader("Shipping", $"Order: {Order?.OrderID}");
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }

            IsCardLoading = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    // Sleep for 500ms to allow the page to render before loading the data
                    await Task.Delay(500);

                    PartList = await OrderService.GetOrderLineAsync(new OrderPartListSearchDTO { FormID = FormID, StockOnly = true });
                    FilteredPartList = PartList;
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
                    StateHasChanged();
                }
            }
        }

        protected void PartListSearch_Click()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            try
            {
                // Get Part by filter
                FilteredPartList = PartList;

                if (!string.IsNullOrEmpty(SearchFilter.SearchText) && FilteredPartList != null)
                {
                    var searchFilter = SearchFilter.SearchText
                       .Split(',')
                       .Select(x => x.Trim())
                       .Where(x => !string.IsNullOrEmpty(x))
                       .ToList();

                    var query = FilteredPartList
                        .AsQueryable();

                    foreach (var filter in searchFilter)
                    {
                        query = query.Where(
                             x => x.PartNumber.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                || x.PartDescription.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                || (!string.IsNullOrEmpty(x.Po) && x.Po.Contains(filter, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(x.StockGroup) && x.StockGroup.Contains(filter, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(x.SerialNumber) && x.SerialNumber.Contains(filter, StringComparison.OrdinalIgnoreCase))
                        );
                    }

                    FilteredPartList = [.. query];
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

            if (FilteredPartList != null)
            {
                // Sort Locally No API

                if (FilteredPartList.Count == 0)
                    return;

                SortOrder = SortOrder == 1 ? 2 : 1;

                switch (SortOrder)
                {
                    case 1:
                        FilteredPartList = column switch
                        {
                            1 => [.. FilteredPartList.OrderBy(x => x.PartNumber)],
                            2 => [.. FilteredPartList.OrderBy(x => x.PartDescription)],
                            3 => [.. FilteredPartList.OrderBy(x => x.Po)],
                            4 => [.. FilteredPartList.OrderBy(x => x.StockGroup)],
                            5 => [.. FilteredPartList.OrderBy(x => x.Quantity)],
                            6 => [.. FilteredPartList.OrderBy(x => x.PickedQuantity)],
                            7 => [.. FilteredPartList.OrderBy(x => x.CommittedQuantity)],
                            8 => [.. FilteredPartList.OrderBy(x => x.InStockQuantity)],
                            9 => [.. FilteredPartList.OrderBy(x => x.InStockQuantity - x.CommittedQuantity)],
                            10 => [.. FilteredPartList.OrderBy(x => x.SerialNumber)],
                            11 => [.. FilteredPartList.OrderBy(x => x.IsStock)],
                            _ => [.. FilteredPartList.OrderBy(x => x.PartNumber)],
                        };
                        break;

                    case 2:
                        FilteredPartList = column switch
                        {
                            1 => [.. FilteredPartList.OrderByDescending(x => x.PartNumber)],
                            2 => [.. FilteredPartList.OrderByDescending(x => x.PartDescription)],
                            3 => [.. FilteredPartList.OrderByDescending(x => x.Po)],
                            4 => [.. FilteredPartList.OrderByDescending(x => x.StockGroup)],
                            5 => [.. FilteredPartList.OrderByDescending(x => x.Quantity)],
                            6 => [.. FilteredPartList.OrderByDescending(x => x.PickedQuantity)],
                            7 => [.. FilteredPartList.OrderByDescending(x => x.CommittedQuantity)],
                            8 => [.. FilteredPartList.OrderByDescending(x => x.InStockQuantity)],
                            9 => [.. FilteredPartList.OrderByDescending(x => x.InStockQuantity - x.CommittedQuantity)],
                            10 => [.. FilteredPartList.OrderByDescending(x => x.SerialNumber)],
                            11 => [.. FilteredPartList.OrderByDescending(x => x.IsStock)],
                            _ => [.. FilteredPartList.OrderByDescending(x => x.PartNumber)],
                        };
                        break;
                }
            }
        }

        protected void PickLine_Toggle(Guid? PickID)
        {
            Layout.ClearMessages();

            PickLineID = PickID;
        }

        protected void PickLine_Toggle()
        {
            PickLineID = null;
        }
    }
}