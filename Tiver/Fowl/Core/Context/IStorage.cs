namespace Tiver.Fowl.Core.Context
{
    public interface IStorage
    {
        void Write(object key, object value);

        object Read(object key);

        void Clear();
    }
}
