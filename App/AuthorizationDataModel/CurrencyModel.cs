namespace AppModel.Model.BankDataModel
{
    [EntityLabel("Сумма денежных средств")]
    public class CurrencyModel : BaseEntity
    {
        public float CurrencyValue { get; set; }
        public int CurrencyUnitID { get; set; }
        public virtual CurrencyUnit CurrencyUnit { get; set; }

    }
}
