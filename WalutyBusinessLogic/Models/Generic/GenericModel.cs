namespace WalutyBusinessLogic.Models.Generic
{
    public class GenericModel<T>
    {
        public T ViewModel { get; set; }

        public GenericModel(T viewModel)
        {
            ViewModel = viewModel;
        }

        public GenericModel()
        {

        }
    }
}
