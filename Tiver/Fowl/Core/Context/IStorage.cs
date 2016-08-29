namespace Tiver.Fowl.Core.Context
{
    public interface IStorage
    {
        void Add(object key, object value);

        object Get(object key);

        void Clear();
    }
}
