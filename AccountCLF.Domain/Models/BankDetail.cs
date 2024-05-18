using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Domain.Models
{
    public class BankDetail
    {
        [Key]
        public int Id { get; set; }
        public int EntityId { get; set; }  
        public Entity Entity { get; set; }
        public int? SrNo { get; set; }
        public string BeneficiaryName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public int? ParentId { get; set; } 
        public  BankDetail Parent { get; set; }
        public int BankId { get; set; }  
        public MasterTypeDetail Bank { get; set; }  
        public int PaymentModeId { get; set; }  
        public MasterTypeDetail PaymentMode { get; set; }  
        public bool IsActive { get; set; }
    }
}
