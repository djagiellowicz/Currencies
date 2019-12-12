using System;
using System.Collections.Generic;
using System.Text;

namespace WalutyBusinessLogic.Models.Generic
{
    class PageModel<T> : GenericModel<T>
    {
        public Page Page { get; set; }

        public PageModel(T model, Page page) : base(model)
        {
            Page = page;
        }
    }
}
