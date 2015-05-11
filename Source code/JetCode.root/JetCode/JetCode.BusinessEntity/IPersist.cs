namespace JetCode.BusinessEntity
{
	public interface IPersist
	{
		Result Save();
		Result Save(SecurityToken token);
	}
}
