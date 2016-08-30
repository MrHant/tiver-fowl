namespace Tiver.Fowl.Core.Context
{
    public class NullContextItem
    {
        private object key;

        public NullContextItem()
        {
            this.key = null;
        }

        public NullContextItem(object key)
        {
            this.key = key;
        }

        public override string ToString()
        {
            string message = "Context item not found.";
            if (this.key != null)
            {
                message = string.Format("Context item not found. for key '{0}'.", this.key);
            }

            return message;
        }
    }
}
