using System.Collections.Generic;

namespace GrainManagement.Models;

public partial class InventoryTransactionDetail
{
    public virtual ICollection<TransactionQuantitySource> TransactionQuantitySources { get; set; } = new List<TransactionQuantitySource>();

    public virtual ICollection<InventoryTransactionDetailFromContainer> FromContainers { get; set; } = new List<InventoryTransactionDetailFromContainer>();

    public virtual ICollection<InventoryTransactionDetailToContainer> ToContainers { get; set; } = new List<InventoryTransactionDetailToContainer>();
}
