using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Order
{
    public partial class OrderSection2
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

        protected bool NewPart { get; set; }

        protected Guid? DeleteID { get; set; }

        protected OrderPartListUpdate? LineUpdate { get; set; }

        protected Guid? PickLineID { get; set; }

        protected Guid? RemovePickLineID { get; set; } 

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();

            try
            {
                Order = await OrderService.GetOrderHeaderAsync(FormID);

                Layout.UpdateHeader("Order Admin", $"Order: {Order?.OrderID}");
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

                    PartList = await OrderService.GetOrderLineAsync(new OrderPartListSearchDTO { FormID = FormID });
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

        protected void AddPart_Toggle()
        {
            Layout.ClearMessages();
            NewPart = !NewPart;
        }

        protected async Task AddPart_Click(Guid? PartID)
        {
            Layout.ClearMessages();

            if (PartID.HasValue)
            {
                try
                {
                    // Add Line
                    await OrderService.CreateOrderLineAsync(new OrderCreateLineDTO
                    {
                        OrderID = FormID,
                        PartID = PartID.Value
                    });

                    // Refresh
                    PartList = await OrderService.GetOrderLineAsync(new OrderPartListSearchDTO { FormID = FormID });
                    FilteredPartList = PartList;

                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
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

        protected void Delete_Toggle(Guid? FormID)
        {
            Layout.ClearMessages();

            DeleteID = FormID;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (DeleteID.HasValue)
                {
                    // Delete the Line
                    await OrderService.DeleteOrderLineAsync(DeleteID.Value);

                    // Refresh
                    PartList = await OrderService.GetOrderLineAsync(new OrderPartListSearchDTO { FormID = FormID });
                    FilteredPartList = PartList;

                    Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);
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
                DeleteID = null;
            }
        }

        protected void LineEdit_Toggle(Guid? FormID)
        {
            Layout.ClearMessages();

            if (FormID.HasValue)
            {
                var partLine = PartList?.FirstOrDefault(x => x.FormID == FormID);

                if (partLine != null)
                    LineUpdate = MapperService.Map<OrderPartListUpdate>(partLine);
            }
            else
                LineUpdate = null;
        }

        protected async Task LineEdit_Save()
        {
            Layout.ClearMessages();

            if (LineUpdate != null)
            {
                try
                {
                    // Update the Line
                    await OrderService.UpdateOrderLineAsync(LineUpdate);

                    // Refresh
                    PartList = await OrderService.GetOrderLineAsync(new OrderPartListSearchDTO { FormID = FormID });
                    FilteredPartList = PartList;

                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
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
                    LineUpdate = null;
                }
            }
        }

        protected void PO_Change(Guid? FormID)
        {
            if (LineUpdate != null)
                LineUpdate.Poid = FormID;
        }

        protected void StockGroup_Change(Guid? FormID)
        {
            if (LineUpdate != null)
                LineUpdate.StockGroupID = FormID;
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

        protected void PickRemove_Toggle(Guid? PickID)
        {
            RemovePickLineID = PickID;
        }

        protected async Task PickRemove_Click()
        {
            Layout.ClearMessages();

            if (RemovePickLineID.HasValue && PickLineID.HasValue)
            {
                try
                {
                    // Remove Line
                    await OrderService.RemovePickFromOrderAsync(new OrderPickRemoveDTO
                    {
                        OrderLineID = PickLineID.Value,
                        InventoryID = RemovePickLineID.Value
                    });

                    // Refresh
                    PartList = await OrderService.GetOrderLineAsync(new OrderPartListSearchDTO { FormID = FormID });
                    FilteredPartList = PartList;

                    Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);
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
                    PickLineID = null;
                    RemovePickLineID = null;
                }
            }
        }

    }
}