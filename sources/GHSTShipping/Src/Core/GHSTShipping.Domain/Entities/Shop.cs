﻿using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(Shop))]
    public class Shop : AuditableBaseEntity
    {
        public bool IsVerified { get; set; }

        public DateTime? RegisterDate { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public decimal AvgMonthlyYield { get; set; }

        [MaxLength(200)]
        public string PhoneNumber { get; set; }

        [MaxLength(200)]
        public string BankName { get; set; }

        [MaxLength(200)]
        public string BankAccountNumber { get; set; }

        [MaxLength(200)]
        public string BankAccountHolder { get; set; }

        [MaxLength(200)]
        public string BankAddress { get; set; }

        [MaxLength(100)]
        public string UniqueCode { get; set; }

        private void GenUniqueCode()
        {
            // Format date to SddMM (S + Day + Month)
            string datePart = $"S{RegisterDate:ddMM}";

            // Take the first 6 characters of the phone number
            string phonePart = PhoneNumber.Substring(0, 6);

            // Combine date part and phone part
            this.UniqueCode = datePart + phonePart;
        }

        public Shop()
        {
        }

        public Shop(string name, string phoneNumber, decimal avgMonthlyYield, DateTime registerDate)
        {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.AvgMonthlyYield = avgMonthlyYield;
            this.RegisterDate = registerDate;
            this.GenUniqueCode();
        }
    }
}