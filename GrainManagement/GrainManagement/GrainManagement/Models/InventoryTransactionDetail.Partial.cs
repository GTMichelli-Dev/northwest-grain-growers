using System.Collections.Generic;

namespace GrainManagement.Models;

public partial class InventoryTransactionDetail
{
    public virtual ICollection<TransactionQuantitySource> TransactionQuantitySources { get; set; } = new List<TransactionQuantitySource>();
}
