using System.Collections.Generic;

namespace WalutyBusinessLogic.Models.Generic
{
    class MessageModel<T> : GenericModel<T>
    {
        public IList<string> Messages { get; set; }

        public MessageModel(T model, IList<string> messages):base(model) 
        {
            Messages = messages;
        }
    }
}
