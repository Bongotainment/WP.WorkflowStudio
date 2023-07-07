namespace WP.WorkflowStudio.Core.Interfaces;

public abstract class BaseDataAccess
{
    public virtual T Get<T>(int id)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<T> Get<T>()
    {
        throw new NotImplementedException();
    }

    public virtual void Set<T>(T value)
    {
        throw new NotImplementedException();
    }

    public virtual void Update<T>(T value)
    {
        throw new NotImplementedException();
    }

    public virtual void Delete<T>(T value)
    {
        throw new NotImplementedException();
    }
}