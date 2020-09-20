﻿using InstaOperatorOauthAPIS.Models.Pass;
using System;

namespace InstaOperatorOauthAPIS.Models.APIOutPutModel
{
    public class CustomerVehiclePass
    {
        public CustomerVehiclePass()
        {
            CustomerVehicleID = new CustomerVehicle();
            PrimaryLocationParkingLotID = new LocationParkingLot();
            PassCardTypeMapperID = new PassCardTypeMapper();
            PaymentTypeID = new PaymentType();
            CreatedBy = new User();
            PassPriceID = new PassPrice();
            LocationID = new Location();
            PassTypeID = new PassType();
            LocationID = new Location();
        }
        public int CustomerVehiclePassID { get; set; }
        public CustomerVehicle CustomerVehicleID { get; set; }
        public LocationParkingLot PrimaryLocationParkingLotID { get; set; }
        public PassCardTypeMapper PassCardTypeMapperID { get; set; }
        public PassPrice PassPriceID { get; set; }
        public PassType PassTypeID { get; }
        public Location LocationID { get; set; }
        public bool IsMultiLot { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IssuedCard { get; set; }
        public string CardNumber { get; set; }
        public string BarCode { get; set; }
        public decimal Amount { get; set; }
        public decimal CardAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionID { get; set; }
        public PaymentType PaymentTypeID { get; set; }
        public int StatusID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }


    }
}
