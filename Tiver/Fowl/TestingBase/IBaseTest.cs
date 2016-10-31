namespace Tiver.Fowl.TestingBase
{
    public interface IBaseTest
    {
        void Setup();

        void Teardown();

        int? Step
        {
            get;
            set;
        }
    }
}