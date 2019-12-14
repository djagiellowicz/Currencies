using System;
using System.Collections.Generic;
using System.Text;

namespace WalutyBusinessLogic.Models.Generic
{
    public class PageModel<T> : GenericModel<T>
    {
        public Page Page { get; set; }

        public PageModel(T viewModel, Page page) : base(viewModel)
        {
            Page = page;
        }

        public PageModel() : base()
        {
           
        }
    }
}
