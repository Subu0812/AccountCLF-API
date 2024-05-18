using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model;

public partial class Entity
{
    [Key]

    public int Id { get; set; }

    public int? TypeId { get; set; }

    public DateTime? Date { get; set; }

    public int? AccountTypeId { get; set; }
    public virtual AccountGroup? AccountType { get; set; }

    public int? SessionId { get; set; }
    public virtual AccountSession? Session { get; set; }

    public int? ReferenceId { get; set; }
    public Entity? Reference { get; set; }


    public int? StaffId { get; set; }
    public virtual Entity? Staff { get; set; }

    public int? Status { get; set; }

    public int? IsActive { get; set; }





    public virtual MasterTypeDetail? Type { get; set; }
    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<BasicProfile> BasicProfiles { get; set; } = new List<BasicProfile>();

    public virtual ICollection<ContactProfile> ContactProfiles { get; set; } = new List<ContactProfile>();

    public virtual ICollection<DocumentProfile> DocumentProfiles { get; set; } = new List<DocumentProfile>();

    public virtual ICollection<Entity> InverseReference { get; set; } = new List<Entity>();

    public virtual ICollection<Entity> InverseStaff { get; set; } = new List<Entity>();

    public virtual ICollection<MasterLogin> MasterLogins { get; set; } = new List<MasterLogin>();

    public virtual ICollection<ProfileLink> ProfileLinkEntities { get; set; } = new List<ProfileLink>();

 

  
}
