using System.Collections.Generic;

namespace WalutyBusinessLogic.Models.Generic
{
    public class MessageModel<T> : GenericModel<T>
    {
        public IList<string> Messages { get; set; }

        public MessageModel(T viewModel, IList<string> messages):base(viewModel) 
        {
            Messages = messages;
        }
        public MessageModel() : base()
        {

        }
    }
}
