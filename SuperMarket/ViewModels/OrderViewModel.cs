using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.DataAccess.Models;
using SuperMarket.Services;
using System.Collections.ObjectModel;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for order history and details
    /// </summary>
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;

        // Backing fields
        private ObservableCollection<Order> _orders = new();
        private Order? _selectedOrder;
        private string _message = string.Empty;
        private bool _isLoading;
        private bool _showDetails;
        private int _userId = 1; // TODO: Replace with actual user ID from auth system

        // Expression-bodied properties
        public ObservableCollection<Order> Orders { get => _orders; set => SetProperty(ref _orders, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        public bool ShowDetails { get => _showDetails; set => SetProperty(ref _showDetails, value); }

        // Property with side effects
        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (SetProperty(ref _selectedOrder, value))
                {
                    ShowDetails = value != null;

                    if (value != null)
                    {
                        LoadOrderDetailsCommand.ExecuteAsync(value.Id);
                    }
                }
            }
        }

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            LoadOrdersCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadOrders()
        {
            IsLoading = true;
            Message = string.Empty;

            try
            {
                var orders = await _orderService.GetOrdersAsync(_userId);
                Orders = new ObservableCollection<Order>(orders.OrderByDescending(o => o.OrderDate));
                Message = Orders.Count > 0 ? $"{Orders.Count} orders found" : "No orders found";
            }
            catch (Exception ex)
            {
                Message = $"Error loading orders: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task LoadOrderDetails(int orderId)
        {
            IsLoading = true;

            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);

                if (order != null)
                {
                    // Replace the selected order with the one containing full details
                    var existingOrder = Orders.FirstOrDefault(o => o.Id == order.Id);
                    if (existingOrder != null)
                    {
                        int index = Orders.IndexOf(existingOrder);

                        if (index >= 0)
                        {
                            Orders[index] = order;
                            SelectedOrder = order;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = $"Error loading order details: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void CloseDetails()
        {
            SelectedOrder = null;
            ShowDetails = false;
        }
    }
}