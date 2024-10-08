﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace FlamingFork.Models
{
    public partial class CustomerOrderModel: ObservableObject
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerContact {  get; set; }
        public string? CustomerAddress { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public string? StringifiedItems {  get; set; } 
        public int TotalPrice { get; set; }

        public string OrderDate { get; set; }

        [ObservableProperty]
        public string? _orderStatus = "Placed";

        public CustomerOrderModel(int customerId, string customerContact, string customerAddress, string orderStatus, List<OrderItemModel> orderItems, string orderDate)
        {
            CustomerId = customerId;
            CustomerContact = customerContact;
            CustomerAddress = customerAddress;
            OrderStatus = orderStatus;
            OrderItems = orderItems;
            OrderDate = orderDate;
        }
    }
}
