﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeTroUIDemo
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Paking_SlotEntities : DbContext
    {
        public Paking_SlotEntities()
            : base("name=Paking_SlotEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<user_role> user_role { get; set; }
        public virtual DbSet<user> users { get; set; }
    
        public virtual int sp_laneIn(Nullable<int> loginID, string plateNumber, string cardNo, Nullable<System.DateTime> checkTime, ObjectParameter customerName, ObjectParameter message, ObjectParameter plateNum, ObjectParameter total_Amount, ObjectParameter time, ObjectParameter lanceTye, ObjectParameter photoCustomerIn, ObjectParameter photoLicensePlateNumberIN)
        {
            var loginIDParameter = loginID.HasValue ?
                new ObjectParameter("LoginID", loginID) :
                new ObjectParameter("LoginID", typeof(int));
    
            var plateNumberParameter = plateNumber != null ?
                new ObjectParameter("PlateNumber", plateNumber) :
                new ObjectParameter("PlateNumber", typeof(string));
    
            var cardNoParameter = cardNo != null ?
                new ObjectParameter("CardNo", cardNo) :
                new ObjectParameter("CardNo", typeof(string));
    
            var checkTimeParameter = checkTime.HasValue ?
                new ObjectParameter("CheckTime", checkTime) :
                new ObjectParameter("CheckTime", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_laneIn", loginIDParameter, plateNumberParameter, cardNoParameter, checkTimeParameter, customerName, message, plateNum, total_Amount, time, lanceTye, photoCustomerIn, photoLicensePlateNumberIN);
        }
    
        public virtual int sp_laneOut(Nullable<int> loginID, string plateNumber, string cardNo, Nullable<System.DateTime> checkTime, ObjectParameter customerName, ObjectParameter message, ObjectParameter plateNum, ObjectParameter total_Amount, ObjectParameter time, ObjectParameter lanceTye, ObjectParameter photoCustomerIn, ObjectParameter photoLicensePlateNumberIN, string photoCustomerOut, string photoLicensePlateNumberOut, ObjectParameter isMatch)
        {
            var loginIDParameter = loginID.HasValue ?
                new ObjectParameter("LoginID", loginID) :
                new ObjectParameter("LoginID", typeof(int));
    
            var plateNumberParameter = plateNumber != null ?
                new ObjectParameter("PlateNumber", plateNumber) :
                new ObjectParameter("PlateNumber", typeof(string));
    
            var cardNoParameter = cardNo != null ?
                new ObjectParameter("CardNo", cardNo) :
                new ObjectParameter("CardNo", typeof(string));
    
            var checkTimeParameter = checkTime.HasValue ?
                new ObjectParameter("CheckTime", checkTime) :
                new ObjectParameter("CheckTime", typeof(System.DateTime));
    
            var photoCustomerOutParameter = photoCustomerOut != null ?
                new ObjectParameter("PhotoCustomerOut", photoCustomerOut) :
                new ObjectParameter("PhotoCustomerOut", typeof(string));
    
            var photoLicensePlateNumberOutParameter = photoLicensePlateNumberOut != null ?
                new ObjectParameter("PhotoLicensePlateNumberOut", photoLicensePlateNumberOut) :
                new ObjectParameter("PhotoLicensePlateNumberOut", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_laneOut", loginIDParameter, plateNumberParameter, cardNoParameter, checkTimeParameter, customerName, message, plateNum, total_Amount, time, lanceTye, photoCustomerIn, photoLicensePlateNumberIN, photoCustomerOutParameter, photoLicensePlateNumberOutParameter, isMatch);
        }
    }
}
